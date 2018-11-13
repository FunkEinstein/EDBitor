using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using DocumentParsers.Exceptions;
using DocumentParsers.Schemas;
using DocumentParsers.Schemas.Elements;
using DocumentParsers.Schemas.Elements.Builders;
using DocumentParsers.Schemas.Elements.Builders.BuildingRules;

namespace DocumentParsers.Parsers
{
    public class XmlDocumentParser : DocumentParser
    {
        private const string NamePattern = "[a-zA-Z_][\\w\\.-]*?";
        private const string SpecialSymbolsPattern = "(?:&lt;|&gt;|&amp;|&apos;|&quot;)";
        private const string TextPattern = "(\\s*(?:" + SpecialSymbolsPattern + "|[^<&])*?\\s*)";

        private readonly Regex _headerRx;
        private const string HeaderMarker = "<\\?xml[^<]+?\\?>";

        private readonly Regex _elementOpenTagRx;
        private const string ElementOpenTagPattern = "<" + NamePattern + "[^<]*?\\s*/?>";
        private readonly Regex _elementCloseTagRx;
        private const string ElementCloseTagPattern = "</" + NamePattern + "\\s*>";
        private readonly Regex _elementNameRx;
        private const string ElementNamePattern = "</?(" + NamePattern + ")[\\s/>]";
        private readonly Regex _elementCloseImmediatelyRx;
        private const string ElementCloseImmediatelyPattern = "/>";

        private readonly Regex _attributeRx;
        private const string AttributePattern = "(" + NamePattern + ")\\s*=\\s*\"(" + TextPattern + ")\"";

        private readonly Regex _constantRx;
        private const string ConstantPattern = "()<" + NamePattern + "[^<]*?\\s*[^/]>" + TextPattern + "()</";

        private readonly Regex _commentRx;
        private const string CommentPattern = "<!--[^--]*-->";

        private readonly Regex _nextElementRx;
        private const string NextElementPattern = "\\s*()<[^/]";
        
        public XmlDocumentParser()
            : base(new XmlBuildingRules())
        {
            _headerRx = new Regex(HeaderMarker);
            _elementOpenTagRx = new Regex(ElementOpenTagPattern);
            _elementCloseImmediatelyRx = new Regex(ElementCloseImmediatelyPattern);
            _elementNameRx = new Regex(ElementNamePattern);
            _elementCloseTagRx = new Regex(ElementCloseTagPattern);
            _attributeRx = new Regex(AttributePattern);
            _constantRx = new Regex(ConstantPattern);
            _commentRx = new Regex(CommentPattern);
            _nextElementRx = new Regex(NextElementPattern);
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
        /// <exception cref="ParseException">Throw if document has invalid structure</exception>
        private int ParseHeader(string text, out SchemaHeader header)
        {
            header = null;

            var startPosition = NextElementStartPosition(text, 0);
            if (IsEOF(text, startPosition))
                throw new ParseException("Xml document should contain root level element", startPosition);

            var match = _headerRx.Match(text);
            if (!match.Success)
                return 0;

            if (match.Index != 0 || match.Index != startPosition)
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
        /// <exception cref="ParseException">Throw if document has invalid structure</exception>
        private int ParseElement(string text, int startPosition, out SchemaElement element)
        {
            element = null;

            var nextPosition = NextElementStartPosition(text, startPosition);
            if (IsEOF(text, nextPosition)) // no one element left
                return startPosition;

            var openTagMatch = _elementOpenTagRx.Match(text, startPosition);
            if (!openTagMatch.Success) // no element found
                return startPosition;

            if (openTagMatch.Index != nextPosition) // means that next item is not an element
                return startPosition;

            var elementStartPosition = openTagMatch.Index;
            var nameMatch = _elementNameRx.Match(text, elementStartPosition);
            if (!nameMatch.Success)
                throw new ParseException("Impossible to detect xml element name", elementStartPosition);

            var name = nameMatch.Groups[1].Value;

            var elementBuilder = BuilderFactory.Create();
            elementBuilder.SetName(name);

            nextPosition = PositionAfterElement(openTagMatch);
            var attributes = ParseAttributes(text, elementStartPosition, nextPosition);
            if (!attributes.IsNullOrEmpty())
                elementBuilder.AddAttributes(attributes);

            // element closes immediately
            var closeElementMatch = _elementCloseImmediatelyRx.Match(text, elementStartPosition);
            if (closeElementMatch.Success && closeElementMatch.Index < nextPosition)
            {
                element = elementBuilder.BuildElement();
                return nextPosition;
            }
            
            // element has constant
            SchemaConstant constant;
            var afterParseConstantPosition = ParseConstant(text, elementStartPosition, name, out constant);
            if (constant != null)
            {
                nextPosition = afterParseConstantPosition;
                elementBuilder.AddChild(constant);
                element = elementBuilder.BuildElement();
                return nextPosition;
            }

            // element has children
            List<SchemaElement> children;
            nextPosition = ParseElements(text, nextPosition, name, out children);
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
        /// <exception cref="ParseException">Throw if document has invalid structure</exception>
        private int ParseElements(string text, int startPosition, string parentName, out List<SchemaElement> elements)
        {
            var nextPosition = startPosition;
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

                var closeTagMatch = _elementCloseTagRx.Match(text, nextPosition);
                if (!closeTagMatch.Success)
                    throw new ParseException("Opened element should be closed", nextPosition);

                nextPosition = NextElementStartPosition(text, nextPosition);
                var closeTagStartPosition = closeTagMatch.Index;
                if (closeTagStartPosition > nextPosition)
                    continue;

                var nameMatch = _elementNameRx.Match(text, closeTagStartPosition);
                if (!nameMatch.Success)
                    throw new ParseException("Invalid close element statement", closeTagStartPosition);

                var name = nameMatch.Groups[1].Value;
                if (name != parentName)
                    throw new ParseException("Close element doesn't match with open element", closeTagStartPosition);

                return PositionAfterElement(closeTagMatch);
            }
        }

        /// <summary>
        /// Parse constant from <paramref name="startElementPosition"/> position in <paramref name="text"/>
        /// string and return element's constant.
        /// </summary>
        /// <param name="text">String that will be parsed</param>
        /// <param name="startElementPosition">Start position of element that has constant value</param>
        /// <param name="elementName">Constant parent's name</param>
        /// <param name="constant">Builded constant or null if constant doesn't exist</param>
        /// <returns>Position after element close tag</returns>
        /// <exception cref="ParseException">Throw if document has invalid structure</exception>
        private int ParseConstant(string text, int startElementPosition, string elementName, out SchemaConstant constant)
        {
            constant = null;

            var constantMatch = _constantRx.Match(text, startElementPosition);
            if (!constantMatch.Success) // constant doesn't exist
                return startElementPosition;

            var constantTextGroup = constantMatch.Groups[2];

            // check open tag
            var beforeOpenTagGroup = constantMatch.Groups[1];
            var beforeOpenTagPosition = PositionAfterElement(beforeOpenTagGroup);

            if (beforeOpenTagPosition != startElementPosition)
                return startElementPosition; // means that found another element's comment

            var openTagNameMatch = _elementNameRx.Match(text, beforeOpenTagPosition);
            if (!openTagNameMatch.Success)
                throw new ParseException("Invalid or missing open tag", beforeOpenTagPosition);

            var openTagName = openTagNameMatch.Groups[1].Value;
            if (openTagName != elementName)
                throw new ParseException("Name of open tag doesn't match with name of parent element", beforeOpenTagPosition);

            // check close tag
            var beforeCloseTagGroup = constantMatch.Groups[3];
            var beforeCloseTagPosition = PositionAfterElement(beforeCloseTagGroup);

            var closeTagMatch = _elementCloseTagRx.Match(text, beforeCloseTagPosition);
            if (!closeTagMatch.Success)
                throw new ParseException("Opened element should be closed", beforeCloseTagPosition);

            var closeTagNameMatch = _elementNameRx.Match(text, beforeCloseTagPosition);
            if (!closeTagNameMatch.Success)
                throw new ParseException("Invalid or missing close tag", beforeCloseTagPosition);

            var closeTagName = closeTagNameMatch.Groups[1].Value;
            if (closeTagName != elementName)
                throw new ParseException("Name of close tag doesn't match with name of parent element", beforeCloseTagPosition);

            if (closeTagMatch.Index != beforeCloseTagPosition)
                throw new ParseException("Element should be closed after constant", beforeCloseTagPosition);

            var constantBuilder = BuilderFactory.Create();
            constantBuilder.SetValue(constantTextGroup.Value);
            constant = constantBuilder.BuildConstant();

            return PositionAfterElement(closeTagMatch);
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
            var nextPosition = NextElementStartPosition(text, startPosition);
            if (IsEOF(text, nextPosition)) // no one comment left
                return startPosition;

            var commentMatch = _commentRx.Match(text, nextPosition);
            if (!commentMatch.Success)
                return startPosition;

            if (commentMatch.Index != nextPosition) // means that next item is not a comment
                return startPosition;

            var builder = BuilderFactory.Create();
            builder.SetValue(commentMatch.Value);
            comment = builder.BuildComment();

            return PositionAfterElement(commentMatch);
        }

        /// <summary>
        /// Parse sequence of comments from <paramref name="startPosition"/> position 
        /// in <paramref name="text"/> string and return position to next character after element.
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
                if (comment == null) // means that next element is not a comment
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
        /// <param name="elementEndPosition">End of attributive's owner</param>
        /// <param name="attribute">Result of parsing</param>
        /// <returns>Position to next character after element</returns>
        private int ParseAttribute(string text, int startPosition, int elementEndPosition, out SchemaAttribute attribute)
        {
            attribute = null;

            var attributeMatch = _attributeRx.Match(text, startPosition);
            if (!attributeMatch.Success)
                return startPosition; // attribute doesn't exist

            if (attributeMatch.Index > elementEndPosition)
                return startPosition; // found attribute in another 

            var name = attributeMatch.Groups[1].Value;
            var value = attributeMatch.Groups[2].Value;
            attribute = new SchemaAttribute(name, value);
            return PositionAfterElement(attributeMatch);
        }

        /// <summary>
        /// Parse attributes from <paramref name="elementStartPosition"/> position in <paramref name="text"/>
        /// string and return list of attributes.
        /// </summary>
        /// <param name="text">String that will be parsed</param>
        /// <param name="elementStartPosition">Position from which parsing will start</param>
        /// <returns>List of attributes or null if element doesn't have them</returns>
        private List<SchemaAttribute> ParseAttributes(string text, int elementStartPosition, int elementEndPosition)
        {
            List<SchemaAttribute> attributes = null;

            var nextPosition = elementStartPosition;
            while (true)
            {
                SchemaAttribute attribute;
                nextPosition = ParseAttribute(text, nextPosition, elementEndPosition, out attribute);
                if (attribute == null)
                    return attributes;

                if (attributes == null)
                    attributes = new List<SchemaAttribute>();

                attributes.Add(attribute);
            }
        }

        #endregion

        #region To string

        /// <summary>
        /// Convert schema of document into string representation
        /// </summary>
        /// <param name="schema">Schema of document</param>
        /// <returns>String representation of schema</returns>
        public override string Stringify(Schema schema)
        {
            var xmlSchema = schema as XmlSchema;

            var builder = new StringBuilder();
            if (xmlSchema != null)
            {
                var header = StringifyHeader(xmlSchema.Header);
                builder.Append(header);
            }

            if (xmlSchema != null)
            {
                var comments = StringifyComments(xmlSchema.CommentsBeforeRoot);
                builder.Append(comments);
            }

            var root = StringifyRoot(schema.Root);
            builder.Append(root);

            if (xmlSchema != null)
            {
                var comments = StringifyComments(xmlSchema.CommentsAfterRoot);
                builder.Append(comments);
            }

            return builder.ToString();
        }

        private string StringifyHeader(SchemaHeader header)
        {
            if (header != null)
            {
                var headerValue = RemoveNewLineSymbols(header.Value);
                return $"{headerValue}{Environment.NewLine}";
            }

            return string.Empty;
        }

        private string StringifyRoot(SchemaElement element)
        {
            if (element == null)
                throw new ParseException("Invalid xml schema");

            return StringifyElement(element, 0);
        }

        private string StringifyElement(SchemaElement element, int depth)
        {
            var stringBuilder = new StringBuilder();

            var tabs = new string('\t', depth);
            stringBuilder.Append($"{tabs}<{element.Name}");

            if (!element.Attributes.IsNullOrEmpty())
            {
                foreach (var attribute in element.Attributes)
                {
                    var value = RemoveNewLineSymbols(attribute.Value);
                    stringBuilder.Append($" {attribute.Name}=\"{value}\"");
                }
            }

            if (element.Children.IsNullOrEmpty())
            {
                stringBuilder.Append(" />");
                stringBuilder.AppendLine();

                return stringBuilder.ToString();
            }

            stringBuilder.Append(">");
            if (element.Children.Count == 1)
            {
                var constant = element.Children[0] as SchemaConstant;
                if (constant != null)
                {
                    var stringConstant = StringifyConstant(constant);
                    stringBuilder.Append(stringConstant);
                    stringBuilder.Append($"</{element.Name}>");
                    stringBuilder.AppendLine();

                    return stringBuilder.ToString();
                }
            }

            stringBuilder.AppendLine();
            foreach (var child in element.Children)
            {
                string childString;

                var nextDepth = depth + 1;

                var comment = child as SchemaComment;
                if (comment != null)
                    childString = StringifyComment(comment, nextDepth);
                else
                    childString = StringifyElement(child, nextDepth);

                stringBuilder.Append(childString);
            }

            stringBuilder.AppendLine($"{tabs}</{element.Name}>");
            stringBuilder.AppendLine();

            return stringBuilder.ToString();
        }

        private string StringifyComments(List<SchemaComment> comments)
        {
            if (comments.IsNullOrEmpty())
                return string.Empty;

            var stringBuilder = new StringBuilder();
            foreach (var comment in comments)
                stringBuilder.Append(StringifyComment(comment, 0));

            return stringBuilder.ToString();
        }

        private string StringifyComment(SchemaComment comment, int depth)
        {
            var tabs = new string('\t', depth);

            return $"{tabs}{comment.Value}{Environment.NewLine}";
        }

        private string StringifyConstant(SchemaConstant constant)
        {
            return $"{constant.Value}";
        }

        private string RemoveNewLineSymbols(string text)
        {
            return Regex.Replace(text, "\t|\n|\r", "");
        }

        #endregion

        #region Helpers

        private int NextElementStartPosition(string text, int startPosition)
        {
            if (IsEOF(text, startPosition))
                return PositionAfterEndOf(text); // current element is last

            var nextElementMatch = _nextElementRx.Match(text, startPosition);
            if (!nextElementMatch.Success)
                return PositionAfterEndOf(text); // current element is last

            return nextElementMatch.Groups[1].Index;
        }

        private static int PositionAfterElement(Group group)
        {
            return group.Index + group.Length;
        }

        private static bool IsEOF(string text, int position)
        {
            return position == PositionAfterEndOf(text);
        }

        private static int PositionAfterEndOf(string text)
        {
            return text.Length;
        }

        #endregion
    }
}
