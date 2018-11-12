using EDBitor.Parsers.SchemaElements;
using EDBitor.Parsers.SchemaElements.Builders;

namespace EDBitor.Parsers
{
    abstract class Parser
    {
        protected BuilderFactory BuilderFactory { get; }

        protected Parser(IBuildingRules rules)
        {
            BuilderFactory = new BuilderFactory(rules);
        }

        public abstract Schema Parse(string text);

        public bool TryParse(string text, out Schema schema)
        {
            try
            {
                schema = Parse(text);
                return true;
            }
            catch (ParseException)
            {
                schema = null;
                return false;
            }
        }
    }
}
