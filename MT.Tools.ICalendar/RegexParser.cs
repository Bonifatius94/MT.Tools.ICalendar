//using MT.Tools.ICalendar.DataObjects.PropertyParameter;
//using System;
//using System.Collections.Generic;
//using System.Text;
//using System.Text.RegularExpressions;

//namespace MT.Tools.ICalendar
//{
//    // ics definitions:   RFC 5545 pages 10+11
//    // UTF-8 definitions: RFC 3629 pages 5+6

//    public class RegexParser
//    {
//        #region Constants

//        // ASCII regex definitions
//        public const string REGEX_DIGIT = "(0-9)";
//        public const string REGEX_ALPHA = "(a-zA-Z)";
//        public const string REGEX_ALPHA_DIGIT = "(a-zA-Z0-9)";
//        public const string REGEX_ALPHA_DIGIT_HYPHEN = "(a-zA-Z0-9\\-)";
//        public const string REGEX_CONTROL = "(\\x00-\\x08\\x0A-\\x1F\\x7F)";
//        public const string REGEX_OPTIONAL_PLUS_MINUS = "((+?)|(-))";

//        // UTF-8 regex definitions
//        public const string REGEX_UTF8_CHAR = "(\\x80-\\xFFFF)";

//        // combined regex definitions
//        public const string REGEX_VALUE_CHAR = "(\\x21-\\x)";

//        // TODO: add missing regex definitions

//        // regex datatypes
//        public const string REGEX_URI = "";
//        public const string REGEX_INTEGER = REGEX_OPTIONAL_PLUS_MINUS + REGEX_DIGIT + "+";
//        public const string REGEX_FLOAT = REGEX_OPTIONAL_PLUS_MINUS + REGEX_DIGIT + "+(\\." + REGEX_DIGIT + "+)?";
//        public const string REGEX_DATE = REGEX_DIGIT + "{4}" + "(0(1-9)|1(0-2))(0(1-9)|(1-2)(0-9)|3(0-1))";

//        #endregion Constants

//        #region Members

//        private static readonly Dictionary<Type, string> _regexPerType = new Dictionary<Type, string>()
//        {
//            { typeof(AlternateTextRepresentationParamter), "" }
//        };

//        #endregion Members

//        #region Methods

//        public IEnumerable<Match> GetMatches<Type>(string content)
//        {
//            // get the type
//            var type = typeof(Type);

//            // make sure the regex exists for the given type
//            if (!_regexPerType.ContainsKey(type)) { throw new ArgumentException($"there was no regular expression found for type { type }!"); }

//            // create a new regex instance and search for matches
//            var regex = new Regex(_regexPerType[type]);
//            var matches = regex.Matches(content);

//            return matches;
//        }

//        #endregion Methods
//    }
//}
