using MT.Tools.ICalendar.DataObjects.PropertyValue.Other;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MT.Tools.ICalendar.DataObjects.PropertyParameter
{
    public abstract class UriBasedPropertyParameter : IPropertyParameter
    {
        #region Constructor

        public UriBasedPropertyParameter() { }

        public UriBasedPropertyParameter(string uri) { Uri = new UriValue(uri); }

        public UriBasedPropertyParameter(Uri uri) { Uri = new UriValue(uri); }

        #endregion Constructor

        #region Members

        public abstract string ParameterKeyword { get; }

        public abstract PropertyParameterType Type { get; }

        public virtual UriValue Uri { get; set; }

        #endregion Memebers

        public virtual void Deserialize(string content)
        {
            // remove heading and trailing white spaces
            content = content.Trim();

            // make sure that the parameter starts with the keyword followed by '='
            if (!content.StartsWith($"{ ParameterKeyword }="))
            {
                throw new ArgumentException($"Invalid { ParameterKeyword } content detected! Property parameter needs to start with { ParameterKeyword } keyword!");
            }

            // extract the uri part from content
            string uriPart = content.Substring(content.IndexOf('=') + 1).Trim();

            // make sure that the uri is surrounded by double quotes
            if (uriPart.First() != '"' || uriPart.Last() != '"') { throw new ArgumentException($"Invalid uri ({ uriPart }) detected! Uri needs to be surrounded with double quotes!"); }

            // create a new uri value instance from it
            string uriContent = uriPart.Substring(1, uriPart.Length - 2);
            Uri = ObjectSerializer.Deserialize<UriValue>(uriContent);
        }

        public virtual string Serialize()
        {
            return $"{ ParameterKeyword }=\"{ Uri.Serialize() }\"";
        }
    }
}
