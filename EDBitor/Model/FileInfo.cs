namespace EDBitor.Model
{
    struct FileInfo
    {
        public readonly int? Id;
        public readonly string FileName;

        public FileInfo(int? id, string fileName)
        {
            Id = id;
            FileName = fileName;
        }
    }
}
