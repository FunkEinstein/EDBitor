using System;

namespace EDBitor.Parsers.SchemaElements.Builders
{
    class SchemaElementBuildingException : Exception
    {
        public SchemaElementBuildingException(string message)
            : base(message)
        { }
    }
}
