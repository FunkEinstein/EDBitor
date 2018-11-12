using System.Collections.Generic;
using System.Text.RegularExpressions;
using EDBitor.Parsers.SchemaElements;
using EDBitor.Parsers.SchemaElements.Builders;

namespace EDBitor.Parsers
{
    class XmlParser : Parser
    {
        private Regex _headerRx;
        private const string HeaderMarker = "<\\?xml[\\S\\s]+?\\?>";

        private Regex _elementOpenRx;
        private const string ElementOpenPattern = "<[a-zA-Z_][\\w\\._-]*?[^<&]*?\\s*/?>";
        private Regex _elementCloseImmediatelyRx;
        private const string ElementCloseImmediatelyPattern = "/>";
        private Regex _elementNameRx;
        private const string ElementNamePattern = "</?([a-zA-Z_][\\w\\._-]*?)[\\s/>]";
        private Regex _elementCloseRx;
        private const string ElementClosePattern = "</[a-zA-Z_][\\w\\._-]*?\\s*>";

        private Regex _attributeRx;
        private const string AttributePattern = "([a-zA-Z_]+)\\s*=\"([^<&]*?)\"";

        private Regex _constantRx;
        private const string ConstantPattern = "<[a-zA-Z_][\\w\\._-]*?[^<&]*?\\s*[^/]>([\\S\\s]*?)</";

        private Regex _commentRx;
        private const string CommentPattern = "<!--[^--]*-->";

        public XmlParser()
            : base(new XmlSchemaBuildingRules())
        {
            _headerRx = new Regex(HeaderMarker);
            _elementOpenRx = new Regex(ElementOpenPattern);
            _elementCloseImmediatelyRx = new Regex(ElementCloseImmediatelyPattern);
            _elementNameRx = new Regex(ElementNamePattern);
            _elementCloseRx = new Regex(ElementClosePattern);
            _attributeRx = new Regex(AttributePattern);
            _constantRx = new Regex(ConstantPattern);
            _commentRx = new Regex(CommentPattern);
        }

        #region Parse

        public override Schema Parse(string text)
        {
            SchemaHeader header;
            var position = ParseHeader(text, out header);

            List<SchemaComment> commentsBeforeRoot;
            position = ParseComments(text, position, out commentsBeforeRoot);

            SchemaElement root;
            position = ParseElement(text, position, out root);
            if (root == null)
                throw new ParseException("Xml document should contain root level element", position);

            List<SchemaComment> commentsAfterRoot;
            ParseComments(text, position, out commentsAfterRoot);

            return new XmlSchema(root, header, commentsBeforeRoot, commentsAfterRoot);
        }

        /// <summary>
        /// Parse header from <paramref name="text"/> string 
        /// and return position to next character after header.
        /// </summary>
        /// <param name="text">String that will be parsed</param>
        /// <param name="header">Result of parsing</param>
        /// <returns>Position to next character after header</returns>
        private int ParseHeader(string text, out SchemaHeader header)
        {
            header = null;

            var match = _headerRx.Match(text);
            if (!match.Success)
                return 0;

            if (match.Index != 0)
                throw new ParseException("Header must be first thing in file", match.Index);

            var builder = BuilderFactory.Create();
            builder.SetValue(match.Value);
            header = builder.BuildHeader();

            return match.Length;
        }

        /// <summary>
        /// Parse element from <paramref name="startPosition"/> position in <paramref name="text"/>
        /// string and return position to next character after element.
        /// </summary>
        /// <param name="text">String that will be parsed</param>
        /// <param name="startPosition">Position from which parsing will start</param>
        /// <param name="element">Result of parsing</param>
        /// <returns>Position to next character after element</returns>
        private int ParseElement(string text, int startPosition, out SchemaElement element)
        {
            element = null;

            var elementMatch = _elementOpenRx.Match(text, startPosition);
            if (!elementMatch.Success)
                return startPosition;

            var elementBuilder = BuilderFactory.Create();
            var elementStartPosition = elementMatch.Index;
            var nameMatch = _elementNameRx.Match(text, elementStartPosition);
            if (!nameMatch.Success)
                throw new ParseException("Impossible to detect xml element name", elementStartPosition);

            elementBuilder.SetName(nameMatch.Value);

            List<SchemaAttribute> attributes;
            ParseAttributes(text, elementStartPosition, out attributes);
            elementBuilder.AddAttributes(attributes);
            var nextPosition = NextPosition(elementMatch);

            // element closes immediately
            if (_elementCloseImmediatelyRx.IsMatch(text, elementStartPosition))
            {
                element = elementBuilder.BuildElement();
                return nextPosition;
            }

            // element has children
            List<SchemaElement> children;
            nextPosition = ParseElements(text, nextPosition, elementBuilder.Name, out children);
            elementBuilder.AddChildren(children);
            element = elementBuilder.BuildElement();

            return nextPosition;
        }

        /// <summary>
        /// Parse elements from <paramref name="startPosition"/> position in <paramref name="text"/>
        /// string and return position to next character after element.
        /// </summary>
        /// <param name="text">String that will be parsed</param>
        /// <param name="startPosition">Position from which parsing will start</param>
        /// <param name="parentName">Constant parent's name</param>
        /// <param name="elements">Result of parsing</param>
        /// <returns>Position to next character after element</returns>
        private int ParseElements(string text, int startPosition, string parentName, out List<SchemaElement> elements)
        {
            // element has constant
            SchemaConstant constant;
            var nextPosition = ParseConstant(text, startPosition, parentName, out constant);
            if (constant != null)
            {
                elements = new List<SchemaElement> { constant };
                return nextPosition;
            }

            nextPosition = startPosition;
            elements = new List<SchemaElement>();
            while (true)
            {
                SchemaElement element;
                nextPosition = ParseElement(text, nextPosition, out element);
                if (element != null)
                    elements.Add(element);

                SchemaComment comment;
                nextPosition = ParseComment(text, nextPosition, out comment);
                if (comment != null)
                    elements.Add(comment);

                if (IsEOF(text, nextPosition))
                    throw new ParseException("Opened element should be closed", nextPosition);

                var closeMatch = _elementCloseRx.Match(text, nextPosition);
                if (!closeMatch.Success)
                    continue;

                var nameMatch = _elementNameRx.Match(text, nextPosition);
                if (!nameMatch.Success)
                    throw new ParseException("Invalid close element statement", nextPosition);

                if (nameMatch.Value != parentName)
                    throw new ParseException("Close element doesn't match with open element", nextPosition);

                return nextPosition;
            }
        }

        /// <summary>
        /// Parse constant from <paramref name="startPosition"/> position in <paramref name="text"/>
        /// string and return position to next character after element.
        /// </summary>
        /// <param name="text">String that will be parsed</param>
        /// <param name="startPosition">Position from which parsing will start</param>
        /// <param name="parentName">Constant parent's name</param>
        /// <param name="constant">Result of parsing</param>
        /// <returns>Position to next character after element</returns>
        private int ParseConstant(string text, int startPosition, string parentName, out SchemaConstant constant)
        {
            constant = null;

            var constantMatch = _constantRx.Match(text, startPosition);
            if (!constantMatch.Success)
                return startPosition;

            var nextPosition = NextPosition(constantMatch.Groups[0]);
            var closeMatch = _elementCloseRx.Match(text, nextPosition);
            if (!closeMatch.Success)
                throw new ParseException("Opened element should be closed", nextPosition);

            var nameMatch = _elementNameRx.Match(text, nextPosition);
            if (!nameMatch.Success)
                throw new ParseException("Invalid close element statement", nextPosition);

            if (nameMatch.Value != parentName)
                throw new ParseException("Close element doesn't match with open element", nextPosition);

            var constantBuilder = BuilderFactory.Create();
            constantBuilder.SetValue(constantMatch.Value);

            constant = constantBuilder.BuildConstant();
            return NextPosition(closeMatch);
        }

        /// <summary>
        /// Parse comment from <paramref name="startPosition"/> position in <paramref name="text"/>
        /// string and return position to next character after element.
        /// </summary>
        /// <param name="text">String that will be parsed</param>
        /// <param name="startPosition">Position from which parsing will start</param>
        /// <param name="comment">Result of parsing</param>
        /// <returns>Position to next character after element</returns>
        private int ParseComment(string text, int startPosition, out SchemaComment comment)
        {
            comment = null;

            var match = _commentRx.Match(text, startPosition);
            if (!match.Success)
                return startPosition;

            var builder = BuilderFactory.Create();
            builder.SetValue(match.Value);
            comment = builder.BuildComment();

            return NextPosition(match);
        }

        /// <summary>
        /// Parse comments from <paramref name="startPosition"/> position in <paramref name="text"/>
        /// string and return position to next character after element.
        /// </summary>
        /// <param name="text">String that will be parsed</param>
        /// <param name="startPosition">Position from which parsing will start</param>
        /// <param name="comments">Result of parsing</param>
        /// <returns>Position to next character after element</returns>
        private int ParseComments(string text, int startPosition, out List<SchemaComment> comments)
        {
            comments = null;

            var nextPosition = startPosition;
            while (true)
            {
                SchemaComment comment;
                nextPosition = ParseComment(text, nextPosition, out comment);
                if (comment == null)
                    return nextPosition;

                if (comments == null)
                    comments = new List<SchemaComment>();

                comments.Add(comment);
            }
        }

        /// <summary>
        /// Parse attribute from <paramref name="startPosition"/> position in <paramref name="text"/>
        /// string and return position to next character after element.
        /// </summary>
        /// <param name="text">String that will be parsed</param>
        /// <param name="startPosition">Position from which parsing will start</param>
        /// <param name="attribute">Result of parsing</param>
        /// <returns>Position to next character after element</returns>
        private int ParseAttribute(string text, int startPosition, out SchemaAttribute attribute)
        {
            attribute = null;

            var attributeMatch = _attributeRx.Match(text, startPosition);
            if (!attributeMatch.Success)
                return startPosition;

            var name = attributeMatch.Groups[0].Value;
            var value = attributeMatch.Groups[1].Value;
            attribute = new SchemaAttribute(name, value);
            return NextPosition(attributeMatch);
        }

        /// <summary>
        /// Parse attributes from <paramref name="startPosition"/> position in <paramref name="text"/>
        /// string and return position to next character after element.
        /// </summary>
        /// <param name="text">String that will be parsed</param>
        /// <param name="startPosition">Position from which parsing will start</param>
        /// <param name="attributes">Result of parsing</param>
        /// <returns>Position to next character after element</returns>
        private int ParseAttributes(string text, int startPosition, out List<SchemaAttribute> attributes)
        {
            attributes = null;

            var nextPosition = startPosition;
            while (true)
            {
                SchemaAttribute attribute;
                nextPosition = ParseAttribute(text, nextPosition, out attribute);
                if (attribute == null)
                    return nextPosition;

                if (attributes == null)
                    attributes = new List<SchemaAttribute>();

                attributes.Add(attribute);
            }
        }

        #endregion

        #region Helpers

        private static int NextPosition(Group group)
        {
            return group.Index + group.Length;
        }

        private static bool IsEOF(string text, int position)
        {
            return position == text.Length;
        }

        #endregion
    }
}
