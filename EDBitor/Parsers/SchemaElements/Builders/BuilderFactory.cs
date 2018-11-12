namespace EDBitor.Parsers.SchemaElements.Builders
{
    class BuilderFactory
    {
        private readonly IBuildingRules _rules;

        public BuilderFactory(IBuildingRules rules)
        {
            _rules = rules;
        }

        public SchemaElementBuilder Create()
        {
            return new SchemaElementBuilder(_rules);
        }
    }
}
