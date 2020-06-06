using MT.Tools.ICalendar.DataObjects.PropertyValue;
using System;
using System.Collections.Generic;
using System.Text;

namespace MT.Tools.ICalendar.DataObjects.PropertyParameter
{
    public class RsvpExpectationParameter : IPropertyParameter
    {
        #region Constructor

        public RsvpExpectationParameter() { }

        public RsvpExpectationParameter(bool isSet) { IsSet = isSet; }

        #endregion Constructor

        #region Members

        public PropertyParameterType Type => throw new NotImplementedException();

        public bool IsSet { get; set; } = false;

        #endregion Members

        #region Methods

        public void Deserialize(string content)
        {
            // remove heading and trailing white spaces
            content = content.Trim();

            // make sure that the parameter starts with RSVP
            if (!content.StartsWith("RSVP=")) { throw new ArgumentException("Invalid RSVP expectation parameter detected! Property parameter needs to start with RSVP keyword!"); }

            // get the value as string and parse it
            string valueAsString = content.Substring(content.IndexOf('=') + 1).Trim();
            IsSet = ObjectSerializer.Deserialize<BooleanValue>(valueAsString).Value;
        }

        public string Serialize()
        {
            return $"RSVP={ new BooleanValue(IsSet).Serialize() }";
        }

        #endregion Methods
    }
}
