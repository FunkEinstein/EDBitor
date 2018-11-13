using System.Configuration;
using Model;

namespace EDBitor
{
    class EDBitorModel
    {
        public Storage Storage { get; }

        public EDBitorModel()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["DB"].ConnectionString;
            var compressor = new DataCompressor();
            Storage = new Storage(connectionString, compressor);
        }
    }
}
