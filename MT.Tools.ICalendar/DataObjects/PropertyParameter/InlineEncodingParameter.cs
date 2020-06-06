using MT.Tools.ICalendar.DataObjects.PropertyValue;
using System;
using System.Collections.Generic;
using System.Text;

namespace MT.Tools.ICalendar.DataObjects.PropertyParameter
{
    public enum InlineEncodingType
    {
        EightBit,
        Base64
    }

    public class InlineEncodingParameter : IPropertyParameter
    {
        #region Constructor

        public InlineEncodingParameter() { }

        public InlineEncodingParameter(InlineEncodingType encoding) { Encoding = encoding; }

        #endregion Constructor

        #region Members

        public PropertyParameterType Type => PropertyParameterType.InlineEncoding;

        public InlineEncodingType Encoding { get; set; }

        #endregion Members

        #region Methods

        public void Deserialize(string content)
        {
            // remove heading and trailing white spaces
            content = content.Trim();

            // make sure that the parameter starts with ENCODING
            if (!content.StartsWith("ENCODING=")) { throw new ArgumentException($"Invalid encoding parameter ({ content }) detected! Property parameter needs to start with ENCODING keyword!"); }

            // extract the encoding type from content
            string encodingType = content.Substring(content.IndexOf('=') + 1).Trim();

            switch (encodingType.ToUpper())
            {
                case "8BIT": Encoding = InlineEncodingType.EightBit; break;
                case "BASE64": Encoding = InlineEncodingType.Base64; break;
                default: throw new ArgumentException($"Invalid encoding type ({ encodingType }) detected! Encoding must be either '8BIT' or 'BASE64'!");
            }
        }

        public string Serialize()
        {
            return $"ENCODING={ (Encoding == InlineEncodingType.EightBit ? "8BIT" : "BASE64") }";
        }

        #endregion Methods
    }
}
