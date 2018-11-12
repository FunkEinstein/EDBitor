namespace EDBitor.Parsers.SchemaElements
{
    class Schema
    {
        public SchemaElement Root { get; }

        public Schema(SchemaElement root)
        {
            Root = root;
        }
    }
}
