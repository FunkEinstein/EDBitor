using System;

namespace EDBitor.Parsers.SchemaElements.Builders
{
    class XmlSchemaBuildingRules : IBuildingRules
    {
        public void IsAvailableBuildHeader(SchemaElementBuilder builder)
        {
            if (string.IsNullOrEmpty(builder.Value))
                throw new SchemaElementBuildingException("Xml header should have value");

            if (!string.IsNullOrEmpty(builder.Name))
                throw new SchemaElementBuildingException("Xml header can't have name");

            if (builder.Children.IsNullOrEmpty())
                throw new SchemaElementBuildingException("Xml header can't have children");
        }

        public void IsAvailableBuildElement(SchemaElementBuilder builder)
        {
            if (string.IsNullOrEmpty(builder.Name))
                throw new SchemaElementBuildingException("Xml element should have name");

            if (!string.IsNullOrEmpty(builder.Value))
                throw new SchemaElementBuildingException("Xml element can't have value");
        }

        public void IsAvailableBuildArray(SchemaElementBuilder builder)
        {
            throw new NotSupportedException("Arrays can't be declared in xml");
        }

        public void IsAvailableBuildComment(SchemaElementBuilder builder)
        {
            if (string.IsNullOrEmpty(builder.Value))
                throw new SchemaElementBuildingException("Xml comment should have value");

            if (!string.IsNullOrEmpty(builder.Name))
                throw new SchemaElementBuildingException("Xml comment can't have name");

            if (!builder.Attributes.IsNullOrEmpty())
                throw new SchemaElementBuildingException("Xml comment can't have attributes");

            if (!builder.Children.IsNullOrEmpty())
                throw new SchemaElementBuildingException("Xml comment can't have children");
        }

        public void IsAvailableBuildConstant(SchemaElementBuilder builder)
        {
            if (string.IsNullOrEmpty(builder.Value))
                throw new SchemaElementBuildingException("Xml comment should have value");

            if (!string.IsNullOrEmpty(builder.Name))
                throw new SchemaElementBuildingException("Xml comment can't have name");

            if (!builder.Attributes.IsNullOrEmpty())
                throw new SchemaElementBuildingException("Xml comment can't have attributes");

            if (!builder.Children.IsNullOrEmpty())
                throw new SchemaElementBuildingException("Xml comment can't have children");
        }
    }
}
