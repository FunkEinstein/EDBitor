using System;
using System.Collections.Generic;

namespace EDBitor.Parsers.SchemaElements
{
    class SchemaValue : SchemaElement
    {
        public string Value { get; }

        public override IReadOnlyList<SchemaAttribute> Attributes =>
            throw new NotSupportedException("Constant element can't has attributes");
        public override IReadOnlyList<SchemaElement> Children =>
            throw new NotSupportedException("Constant element can't has children");

        public SchemaValue(string value)
            : base(null, false, null, null)
        {
            Value = value;
        }
    }
}
