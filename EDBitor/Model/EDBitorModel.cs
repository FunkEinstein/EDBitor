namespace EDBitor.Model
{
    class EDBitorModel
    {
        public Storage Storage { get; }

        public EDBitorModel()
        {
            var compressor = new DataCompressor();
            Storage = new Storage(compressor);
        }
    }
}
