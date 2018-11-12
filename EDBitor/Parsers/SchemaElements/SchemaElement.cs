using System;
using System.Collections.Generic;
using System.Linq;

namespace EDBitor.Parsers.SchemaElements
{
    class SchemaElement
    {
        public string Name { get; }
        public bool IsArray { get; }

        public virtual IReadOnlyList<SchemaAttribute> Attributes => _attributes;
        public virtual IReadOnlyList<SchemaElement> Children => _children;

        private List<SchemaAttribute> _attributes;
        private List<SchemaElement> _children;

        public SchemaElement(string name, bool isArray, List<SchemaAttribute> attributes, List<SchemaElement> children)
        {
            Name = name;
            IsArray = isArray;
            _attributes = attributes;
            _children = children;
        }
    }
}
