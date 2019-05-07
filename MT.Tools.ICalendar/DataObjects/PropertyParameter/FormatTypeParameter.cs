using System;
using System.Collections.Generic;
using System.Net.Mime;
using System.Text;

namespace MT.Tools.ICalendar.DataObjects.PropertyParameter
{
    public class FormatTypeParameter : IPropertyParameter
    {
        #region Constructor

        public FormatTypeParameter() { }

        public FormatTypeParameter(string mimeType) : this(new ContentType(mimeType)) { }

        public FormatTypeParameter(ContentType mimeType) { Format = mimeType; }

        #endregion Constructor

        #region Members

        public PropertyParameterType Type => PropertyParameterType.FormatType;

        // RFC 4288 MIME-Type
        public ContentType Format { get; set; }

        // TODO: check if this MIME-type implementation meets the requirements

        #endregion Members

        #region Methods

        public void Deserialize(string content)
        {
            // remove heading and trailing white spaces
            content = content.Trim();

            // make sure that the parameter starts with FMTTYPE
            if (!content.StartsWith("FMTTYPE=")) { throw new ArgumentException("Invalid format type parameter detected! Property parameter needs to start with FMTTYPE keyword!"); }

            // extract mime type and create new content type instance with it
            string mimeTypeString = content.Substring(content.IndexOf('=') + 1);
            Format = new ContentType(mimeTypeString);
        }

        public string Serialize()
        {
            return $"FMTTYPE={ Format.MediaType }";
        }

        #endregion Methods
    }
}
