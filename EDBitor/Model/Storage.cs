using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SQLite;
using System.IO;

namespace EDBitor.Model
{
    internal class Storage
    {
        private struct Scheme
        {
            public const string Table = "files";

            public struct Columns
            {
                public const string Id = "id";
                public const string File = "file";
                public const string Data = "data";
            }
            
        }

        private readonly DataCompressor _compressor;
        private readonly SQLiteConnectionFactory _connectionFactory;

        private bool _isStorageEdited;
        private Dictionary<int, FileInfo> _fileInfos;

        public Storage(DataCompressor compressor)
        {
            _compressor = compressor;

            var connectionString = ConfigurationManager.ConnectionStrings["DB"].ConnectionString;
            _connectionFactory = new SQLiteConnectionFactory(connectionString);
        }

        #region Operations

        public IReadOnlyDictionary<int, FileInfo> GetFileList()
        {
            if (!_isStorageEdited && _fileInfos != null)
                return _fileInfos;

            _fileInfos = new Dictionary<int, FileInfo>();

            using (var connection = _connectionFactory.CreateConnection().OpenAndReturn())
            using (var command = connection.CreateCommand()
                                            .Select(Scheme.Columns.Id, Scheme.Columns.File)
                                            .From(Scheme.Table))
            {
                Read(command, reader =>
                {
                    var id = reader.GetInt32(0);
                    var fileName = reader.GetString(1);
                    var info = new FileInfo { Id = id, FileName = fileName };
                    _fileInfos.Add(id, info);
                });
            }

            _isStorageEdited = false;
            return _fileInfos;
        }

        public void SaveFile(FileInfo info, string file)
        {
            if (string.IsNullOrEmpty(info.FileName))
                throw new ArgumentException("File name can't be null or empty string", nameof(info));

            if (string.IsNullOrEmpty(file))
                throw new ArgumentException("File can't be null or empty string", nameof(file));

            if (info.Id == null)
                AddFile(info, file);
            else
                UpdateFile(info, file);
        }

        public string GetFile(FileInfo info)
        {
            if (info.Id == null)
                throw new ArgumentException("File id can't be null for open operation.", nameof(info));

            var file = string.Empty;

            using (var connection = _connectionFactory.CreateConnection().OpenAndReturn())
            using (var command = connection.CreateCommand()
                                            .Select(Scheme.Columns.Data)
                                            .From(Scheme.Table)
                                            .Where("{0}={1}", Scheme.Columns.Id, info.Id.Value.ToString()))
            {
                Read(command, reader =>
                {
                    var blob = GetBytesFrom(reader);
                    file = _compressor.Decompress(blob);
                });
            }

            return file;
        }

        public void DeleteFile(FileInfo info)
        {
            if (info.Id == null)
                throw new ArgumentException("File id can't be null for delete operation.", nameof(info));

            using (var connection = _connectionFactory.CreateConnection().OpenAndReturn())
            using (var command = connection.CreateCommand()
                                            .Delete()
                                            .From(Scheme.Table)
                                            .Where("{0}={1}", Scheme.Columns.Id, info.Id.Value.ToString()))
            {
                command.ExecuteNonQuery();
            }

            _isStorageEdited = true;
        }

        private void AddFile(FileInfo info, string file)
        {
            const string fileParam = "@file";
            const string dataParam = "@data";

            using (var connection = _connectionFactory.CreateConnection().OpenAndReturn())
            using (var command = connection.CreateCommand()
                                            .InsertInto(Scheme.Table, Scheme.Columns.File, Scheme.Columns.Data)
                                            .Values(fileParam, dataParam))
            {
                command.Parameters.Add(fileParam, DbType.String).Value = info.FileName;
                command.Parameters.Add(dataParam, DbType.Binary).Value = _compressor.Compress(file);
                command.ExecuteNonQuery();
            }

            _isStorageEdited = true;
        }

        private void UpdateFile(FileInfo info, string file)
        {
            const string dataParam = "@data";

            using (var connection = _connectionFactory.CreateConnection().OpenAndReturn())
            using (var command = connection.CreateCommand()
                                            .Update(Scheme.Table)
                                            .Set(new[] { new Tuple<string, string>("data", dataParam) })
                                            .Where("{0}={1}", Scheme.Columns.Id, info.Id))
            {
                command.Parameters.Add(dataParam, DbType.Binary).Value = _compressor.Compress(file);
                command.ExecuteNonQuery();
            }

            _isStorageEdited = true;
        }

        private void Read(SQLiteCommand command, Action<SQLiteDataReader> forEach)
        {
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                    forEach(reader);
            }
        }

        #endregion

        #region Helpers

        private byte[] GetBytesFrom(SQLiteDataReader reader)
        {
            const int chunkSize = 1024;
            byte[] buffer = new byte[chunkSize];
            using (var stream = new MemoryStream())
            {
                long readedBytes;
                long fieldOffset = 0;
                do
                {
                    readedBytes = reader.GetBytes(0, fieldOffset, buffer, 0, chunkSize);
                    stream.Write(buffer, 0, (int)readedBytes);
                    fieldOffset += readedBytes;
                } while (readedBytes == chunkSize);

                return stream.GetBuffer();
            }
        }

        #endregion
    }
}
