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

        public AttachmentProperty(AttachmentContent content, IEnumerable<IPropertyParameter> parameters) { Content = content; Parameters = parameters; }

        #endregion Constructor

        #region Members

        public ComponentPropertyType Type => ComponentPropertyType.Attachment;

        public AttachmentContent Content { get; private set; }
        public IEnumerable<IPropertyParameter> Parameters { get; private set; } = new List<IPropertyParameter>();

        public InlineEncodingParameter Encoding => Parameters.FirstOrDefault(x => x.GetType() == typeof(InlineEncodingParameter)) as InlineEncodingParameter;
        public ValueTypeParameter ValueType => Parameters.FirstOrDefault(x => x.GetType() == typeof(ValueTypeParameter)) as ValueTypeParameter;
        public FormatTypeParameter FormatType => Parameters.FirstOrDefault(x => x.GetType() == typeof(FormatTypeParameter)) as FormatTypeParameter;

        #endregion Members

        public void Deserialize(string content)
        {
            // make sure that the parameter starts with ATTACH
            if (!content.ToUpper().StartsWith("ATTACH")) { throw new ArgumentException("Invalid attachment detected! Component property needs to start with ATTACH keyword!"); }

            // deserialize parameters
            Parameters = 
                content.Substring("ATTACH".Length, content.IndexOf(':') - "ATTACH".Length)
                .Split(';', StringSplitOptions.RemoveEmptyEntries)
                .Select(x => CalendarFactory.DeserializePropertyParameter(x))
                .ToList();

            // extract the value content
            string valueContent = content.Substring(content.IndexOf(':')).Trim();

            // deserialize inline attachment
            if (Encoding != null)
            {
                // make sure that encoding and value type parameter are valid
                if (Encoding.Encoding != InlineEncodingType.Base64) { throw new ArgumentException("Invalid parameter detected! Encoding needs to be set to base64!"); }
                if (ValueType.ValueType != PropertyValueType.Binary) { throw new ArgumentException("Invalid parameter detected! Value type needs to be set to binary!"); }

                // deserialize binary content
                Content = new AttachmentContent(ObjectSerializer.Deserialize<BinaryValue>(valueContent));
            }
            // deserialize relative attachment
            else
            {
                Content = new AttachmentContent(ObjectSerializer.Deserialize<UriValue>(valueContent));
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
                content = $"ATTACH{ (string.IsNullOrEmpty(leadingParamsContent) ? "" : "; " + leadingParamsContent) };{ Encoding.Serialize() };{ ValueType.Serialize() }:{ Content.Serialize() }";
            }
            else
            {
                string paramsContent = Parameters.Select(x => x.Serialize()).Aggregate((x, y) => x + ";" + y);
                content = $"ATTACH{ (string.IsNullOrEmpty(paramsContent) ? "" : ";" + paramsContent) }:{ Content.Serialize() }";
            }

            return content;
        }
    }

    public class AttachmentContent : IPropertyValue
    {
        #region Constructor

        public AttachmentContent(UriValue value) { _value = value; }

        public AttachmentContent(BinaryValue value) { _value = value; }

        #endregion Constructor

        #region Members

        private IPropertyValue _value;

        public PropertyValueType Type => _value.Type;

        #endregion Members

        #region Methods

        public void Deserialize(string content)
        {
            throw new InvalidOperationException("AttachmentContent mustn't be used that way. Use the constructor for putting the data instead!");
        }

        public string Serialize()
        {
            return _value.Serialize();
        }

        #endregion Methods
    }
}
