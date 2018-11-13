using DocumentParsers.Schemas.Elements;

namespace DocumentParsers.Schemas
{
    public class Schema
    {
        public SchemaElement Root { get; }

        public Schema(SchemaElement root)
        {
            Root = root;
        }
    }
}
