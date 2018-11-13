using DocumentParsers.Parsers;
using DocumentParsers.Schemas.Elements;
using NUnit.Framework;
using Tests.Properties;

namespace Tests
{
    class Tests
    {
        [Test]
        public void GivenTwoElementsWithTwoAttributesEach_WhenParseDocument_ThenSchemaHasTwoElementsWithTwoAttributesEach()
        {
            // given
            var document = Resources.ResourceManager.GetString("XMLDocumentWithTwoElements");

            // when
            var parser = new XmlDocumentParser();
            var schema = parser.Parse(document);

            // then
            Assert.AreEqual(schema.Root.Children.Count, 2);
            var firstElement = schema.Root.Children[0];
            Assert.AreEqual(firstElement.Attributes.Count, 2);
            var secondElement = schema.Root.Children[1];
            Assert.AreEqual(secondElement.Attributes.Count, 2);
        }

        [Test]
        public void GivenDocumentWithConstant_WhenParseDocument_ThenSchemaHasRootWithConstant()
        {
            // given
            var document = Resources.ResourceManager.GetString("XMLDocumentWithConstant");

            // when
            var parser = new XmlDocumentParser();
            var schema = parser.Parse(document);

            // then
            Assert.AreEqual(schema.Root.Children.Count, 1);
            Assert.IsInstanceOf<SchemaConstant>(schema.Root.Children[0]);
        }

        [Test]
        public void GivenStrangeDocument_WhenParseDocument_ThenEverythingOk()
        {
            // given
            var document = Resources.ResourceManager.GetString("XMLDocument");

            // when
            var parser = new XmlDocumentParser();
            var schema = parser.Parse(document);

            // then
            Assert.IsNotNull(schema);
        }
    }
}
