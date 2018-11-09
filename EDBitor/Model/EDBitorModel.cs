using System;

namespace EDBitor.Model
{
    class EDBitorModel
    {
        private Lazy<Storage> _storage = new Lazy<Storage>(() => new Storage(new DataCompressor()));
        public Storage Storage
        {
            get { return _storage.Value; }
        }
    }
}
