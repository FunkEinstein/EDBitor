using System;

namespace DocumentParsers.Exceptions
{
    public class SchemaElementBuildingException : Exception
    {
        public SchemaElementBuildingException(string message)
            : base(message)
        { }
    }
}
