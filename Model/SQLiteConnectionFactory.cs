using System.Data.SQLite;

namespace Model
{
    internal class SqLiteConnectionFactory
    {
        private readonly string _connectionString;

        public SqLiteConnectionFactory(string connectionString)
        {
            _connectionString = connectionString;
        }

        public SQLiteConnection CreateConnection()
        {
            return new SQLiteConnection(_connectionString);
        }
    }
}
