using DocumentParsers.Schemas.Elements.Builders.BuildingRules;

namespace DocumentParsers.Schemas.Elements.Builders
{
    public class BuilderFactory
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
