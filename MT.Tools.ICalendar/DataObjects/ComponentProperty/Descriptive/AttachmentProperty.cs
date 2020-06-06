using MT.Tools.ICalendar.DataObjects.ComponentProperty;
using MT.Tools.ICalendar.DataObjects.Factory;
using MT.Tools.ICalendar.DataObjects.PropertyParameter;
using MT.Tools.ICalendar.DataObjects.PropertyValue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;

namespace MT.Tools.ICalendar.DataObjects.ComponentProperty
{
    // definition starts at page 80 in RFC 5545
    public class AttachmentProperty : IComponentProperty
    {
        #region Constructor

        public AttachmentProperty() { }

        public AttachmentProperty(AttachmentValue content, IEnumerable<IPropertyParameter> parameters) { Content = content; Parameters = parameters; }

        #endregion Constructor

        #region Members

        public string Markup => "ATTACH";
        public ComponentPropertyType Type => ComponentPropertyType.Attachment;

        public IEnumerable<IPropertyParameter> Parameters { get; private set; } = new List<IPropertyParameter>();

        public InlineEncodingParameter Encoding => Parameters.FirstOrDefault(x => x.GetType() == typeof(InlineEncodingParameter)) as InlineEncodingParameter;
        public ValueTypeParameter ValueType => Parameters.FirstOrDefault(x => x.GetType() == typeof(ValueTypeParameter)) as ValueTypeParameter;
        public FormatTypeParameter FormatType => Parameters.FirstOrDefault(x => x.GetType() == typeof(FormatTypeParameter)) as FormatTypeParameter;

        public AttachmentValue Content { get; private set; }
        public IPropertyValue Value => Content;

        #endregion Members

        public void Deserialize(string content)
        {
            // make sure that the parameter starts with ATTACH
            if (!content.ToUpper().StartsWith(Markup)) { throw new ArgumentException($"Invalid attachment detected! Component property needs to start with { Markup } keyword!"); }

            // deserialize parameters
            Parameters = 
                content.Substring(Markup.Length, content.IndexOf(':') - Markup.Length)
                .Split(';', StringSplitOptions.RemoveEmptyEntries)
                .Select(x => CalendarFactory.DeserializePropertyParameter(x))
                .ToList();

            // extract the value content
            string valueContent = content.Substring(content.IndexOf(':') + 1).Trim();

            // deserialize inline attachment
            if (Encoding != null)
            {
                // make sure that encoding and value type parameter are valid
                if (Encoding.Encoding != InlineEncodingType.Base64) { throw new ArgumentException("Invalid parameter detected! Encoding needs to be set to base64!"); }
                if (ValueType.ValueType != PropertyValueType.Binary) { throw new ArgumentException("Invalid parameter detected! Value type needs to be set to binary!"); }

                // deserialize binary content
                Content = new AttachmentValue(ObjectSerializer.Deserialize<BinaryValue>(valueContent));
            }
            // deserialize relative attachment
            else
            {
                Content = new AttachmentValue(ObjectSerializer.Deserialize<UriValue>(valueContent));
            }

            // make sure that the format type parameter only occurs once
            if (Parameters.Where(x => x.GetType() == typeof(FormatTypeParameter)).Count() > 1) { throw new ArgumentException("Invalid parameter detected! Format type parameter needs to be unique!"); }
        }

        public string Serialize()
        {
            string content;

            if (Content.Type == PropertyValueType.Binary)
            {
                // make sure that the parameter order is correct
                var paramsToExclude = new List<IPropertyParameter>() { Encoding, ValueType };
                string leadingParamsContent = Parameters.Except(paramsToExclude).Select(x => x.Serialize()).Aggregate((x, y) => x + ";" + y);
                content = $"{ Markup }{ (string.IsNullOrEmpty(leadingParamsContent) ? "" : "; " + leadingParamsContent) };{ Encoding.Serialize() };{ ValueType.Serialize() }:{ Content.Serialize() }";
            }
            else
            {
                string paramsContent = Parameters.Select(x => x.Serialize()).Aggregate((x, y) => x + ";" + y);
                content = $"{ Markup }{ (string.IsNullOrEmpty(paramsContent) ? "" : ";" + paramsContent) }:{ Content.Serialize() }";
            }

            return content;
        }
    }
}
