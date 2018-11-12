namespace EDBitor.Parsers.SchemaElements
{
    class SchemaAttribute
    {
        public string Name { get; }
        public string Value { get; }

        public SchemaAttribute(string name, string value)
        {
            Name = name;
            Value = value;
        }
    }
}
