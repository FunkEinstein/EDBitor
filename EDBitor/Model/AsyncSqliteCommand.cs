using System;
using System.Data.SQLite;
using System.Threading.Tasks;

namespace EDBitor.Model
{
    class AsyncSQLiteCommand : IDisposable
    {
        public SQLiteParameterCollection Parameters
        {
            get { return _command.Parameters; }
        }

        private readonly SQLiteCommand _command;

        public AsyncSQLiteCommand(SQLiteCommand command)
        {
            _command = command;
        }

        public async Task<SQLiteDataReader> ExecuteReader()
        {
            return await Task.Run(() => _command.ExecuteReader());
        }

        public async Task<object> ExecuteScalar()
        {
            return await Task.Run(() => _command.ExecuteScalar());
        }

        public async Task<int> ExecuteNonQuery()
        {
            return await Task.Run(() => _command.ExecuteNonQuery());
        }

        public void Dispose()
        {
            _command?.Dispose();
        }
    }
}
