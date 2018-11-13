using System;

namespace DocumentParsers.Exceptions
{
    public class ParseException : Exception
    {
        public ParseException(string message)
            : base(message)
        { }

        public ParseException(string message, int position)
            : base($"{message} Position: {position.ToString()}")
        { }
    }
}
