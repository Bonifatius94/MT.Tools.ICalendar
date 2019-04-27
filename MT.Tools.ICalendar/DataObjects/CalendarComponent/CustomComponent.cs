using MT.Tools.ICalendar.DataObjects.PropertyValue.Primitive;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MT.Tools.ICalendar.DataObjects.CalendarComponent
{
    // this class represents iana-comp and x-comp from RFC 5545
    public class CustomComponent : ICalendarComponent
    {
        #region Constructor

        public CustomComponent() { }

        public CustomComponent(string token, CalendarProperty<TextValue> property)
        {
            Token = token;
            Property = property;
        }

        #endregion Constructor

        #region Members

        public CalendarComponentType Type => CalendarComponentType.Custom;

        public string Token { get; set; }
        public CalendarProperty<TextValue> Property { get; set; }

        #endregion Members

        #region Methods

        public void Deserialize(string content)
        {
            // get the single lines
            var parts = content.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).ToArray();

            // make sure that the format is correct
            if (parts.Length != 3 || !parts[0].StartsWith("BEGIN:") || !parts[2].StartsWith("END:")) { throw new ArgumentException("Invalid custom component format!"); }

            // parse the token
            Token = parts[0].Substring(6, parts[0].Length - 6);
            string endToken = parts[2].Substring(4, parts[2].Length - 4);

            // make sure that the token are the same
            if (Token?.Equals(endToken) != true) { throw new ArgumentException("Begin and end token do not match!"); }

            // parse content as text
            Property = ObjectSerializer.Deserialize<CalendarProperty<TextValue>>(parts[1]);
        }

        public string Serialize()
        {
            return $"BEGIN:{ Token }\r\n{ Property.Serialize() }\r\nEND:{ Token }";
        }

        #endregion Methods
    }
}
