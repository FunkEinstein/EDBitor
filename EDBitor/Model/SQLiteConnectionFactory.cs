using System.Data.SQLite;

namespace EDBitor.Model
{
    class SQLiteConnectionFactory
    {
        private readonly string _connectionString;

        public SQLiteConnectionFactory(string connectionString)
        {
            _connectionString = connectionString;
        }

        public SQLiteConnection CreateConnection()
        {
            return new SQLiteConnection(_connectionString);
        }
    }
}
