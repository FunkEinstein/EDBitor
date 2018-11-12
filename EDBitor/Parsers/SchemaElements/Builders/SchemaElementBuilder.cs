using System;
using System.Collections.Generic;
using System.Linq;

namespace EDBitor.Parsers.SchemaElements.Builders
{
    class SchemaElementBuilder
    {
        public string Name { get; private set; }
        public string Value { get; private set; }
        public IReadOnlyList<SchemaAttribute> Attributes => _attributes;
        public IReadOnlyList<SchemaElement> Children => _children;

        private List<SchemaAttribute> _attributes;
        private List<SchemaElement> _children;

        private readonly IBuildingRules _buildingRules;

        public SchemaElementBuilder(IBuildingRules buildingRules)
        {
            _buildingRules = buildingRules;
        }

        #region Build

        public SchemaHeader BuildHeader()
        {
            _buildingRules.IsAvailableBuildHeader(this);

            return new SchemaHeader(Value);
        }

        public SchemaElement BuildElement()
        {
            _buildingRules.IsAvailableBuildElement(this);

            return new SchemaElement(Name, false, _attributes, _children);
        }

        public SchemaElement BuildArray()
        {
            _buildingRules.IsAvailableBuildArray(this);

            return new SchemaElement(Name, true, _attributes, _children);
        }

        public SchemaComment BuildComment()
        {
            _buildingRules.IsAvailableBuildComment(this);

            return new SchemaComment(Value);
        }

        public SchemaConstant BuildConstant()
        {
            _buildingRules.IsAvailableBuildConstant(this);

            return new SchemaConstant(Value);
        }

        #endregion

        #region Fill

        public SchemaElementBuilder SetName(string name)
        {
            Name = name;
            return this;
        }

        public SchemaElementBuilder SetValue(string value)
        {
            Value = value;
            return this;
        }

        public SchemaElementBuilder AddChild(SchemaElement child)
        {
            if (_children == null)
            {
                _children = new List<SchemaElement> { child };
                return this;
            }

            var isAlreadyExist = _children.Any(ch => ch == child);
            if (isAlreadyExist)
                throw new ArgumentException("The child is already exist", nameof(child));

            _children.Add(child);
            return this;
        }

        public SchemaElementBuilder AddChildren(List<SchemaElement> children)
        {
            if (_children == null)
            {
                _children = children;
                return this;
            }

            var intersection = _children.Intersect(children);
            if (!intersection.IsNullOrEmpty())
                throw new ArgumentException("The child(children) is already exist", nameof(children));

            _children.AddRange(children);
            return this;
        }

        public SchemaElementBuilder AddAttribute(SchemaAttribute attribute)
        {
            if (_attributes == null)
            {
                _attributes = new List<SchemaAttribute> { attribute };
                return this;
            }

            var isAlreadyExist = _attributes.Any(a => a.Name == attribute.Name);
            if (isAlreadyExist)
                throw new ArgumentException("The attribute is already exist", nameof(attribute));

            _attributes.Add(attribute);
            return this;
        }

        public SchemaElementBuilder AddAttributes(List<SchemaAttribute> attributes)
        {
            if (_attributes == null)
                _attributes = new List<SchemaAttribute>();

            var intersection = _attributes.Intersect(attributes);
            if (!intersection.IsNullOrEmpty())
                throw new ArgumentException("The attribute(s) is already exist", nameof(attributes));

            _attributes.AddRange(attributes);
            return this;
        }

        #endregion
    }
}
