using System.Collections.Generic;

namespace EDBitor.Parsers.SchemaElements
{
    class XmlSchema : Schema
    {
        public SchemaHeader Header { get; }
        public List<SchemaComment> CommentsBeforeRoot { get; }
        public List<SchemaComment> CommentsAfterRoot { get; }

        public XmlSchema(SchemaElement root, SchemaHeader header = null,
            List<SchemaComment> commentsBeforeRoot = null, List<SchemaComment> commentsAfterRoot = null) 
            : base(root)
        {
            Header = header;
            CommentsBeforeRoot = commentsBeforeRoot;
            CommentsAfterRoot = commentsAfterRoot;
        }
    }
}
