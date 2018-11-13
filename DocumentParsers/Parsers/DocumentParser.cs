using DocumentParsers.Exceptions;
using DocumentParsers.Schemas;
using DocumentParsers.Schemas.Elements.Builders;
using DocumentParsers.Schemas.Elements.Builders.BuildingRules;

namespace DocumentParsers.Parsers
{
    public abstract class DocumentParser
    {
        protected BuilderFactory BuilderFactory { get; }

        protected DocumentParser(IBuildingRules rules)
        {
            BuilderFactory = new BuilderFactory(rules);
        }

        public abstract Schema Parse(string text);

        public bool TryParse(string text, out Schema schema)
        {
            schema = null;

            try
            {
                schema = Parse(text);
                return true;
            }
            catch (ParseException)
            {
                return false;
            }
        }

        public abstract string Stringify(Schema schema);
    }
}
