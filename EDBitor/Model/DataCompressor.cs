using System.IO;
using System.IO.Compression;
using System.Text;

namespace EDBitor.Model
{
    class DataCompressor
    {
        public byte[] Compress(string file)
        {
            using (var uncompressedStream = new MemoryStream(Encoding.Unicode.GetBytes(file)))
            using (var compressedStream = new MemoryStream((int)uncompressedStream.Length))
            using (var gzipStream = new GZipStream(compressedStream, CompressionMode.Compress))
            {
                TransferBetween(uncompressedStream, gzipStream);
                return compressedStream.GetBuffer();
            }
        }

        public string Decompress(byte[] file)
        {
            using (var compressedStream = new MemoryStream(file))
            using (var gzipStream = new GZipStream(compressedStream, CompressionMode.Decompress))
            using (var uncompressedStream = new MemoryStream(file.Length))
            {
                TransferBetween(gzipStream ,uncompressedStream);
                var text = Encoding.Unicode.GetString(uncompressedStream.GetBuffer());
                return text;
            }
        }

        #region Helpers

        private void TransferBetween(Stream from, Stream to)
        {
            const int bytesCount = 1024;
            var bytes = new byte[bytesCount];

            int readed;
            do
            {
                readed = from.Read(bytes, 0, bytesCount);
                to.Write(bytes, 0, readed);
            } while (readed == bytesCount);
        }

        #endregion
    }
}
