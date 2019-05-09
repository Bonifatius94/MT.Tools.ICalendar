using System;
using System.Collections.Generic;
using System.Text;
using TimeZoneConverter;

namespace MT.Tools.ICalendar.DataObjects.PropertyParameter
{
    public class TimeZoneIdParameter : IPropertyParameter
    {
        #region Constructor

        public TimeZoneIdParameter() { }

        public TimeZoneIdParameter(TimeZoneInfo timezone, bool isUniqueId = false) { Timezone = timezone; IsUniqueId = isUniqueId; }

        #endregion Constructor

        #region Members

        public PropertyParameterType Type => PropertyParameterType.TimeZoneIdentifier;

        public bool IsUniqueId { get; set; } = false;
        public TimeZoneInfo Timezone { get; set; }

        #endregion Members

        #region Methods

        public void Deserialize(string content)
        {
            // remove heading and trailing white spaces
            content = content.Trim();

            // make sure that the parameter starts with TZID
            if (!content.StartsWith("TZID=")) { throw new ArgumentException("Invalid timezone identifier parameter detected! Property parameter needs to start with TZID keyword!"); }

            // get the value as string and parse it
            string valueAsString = content.Substring(content.IndexOf('=') + 1).Trim();

            // check if the iana timezone is unique
            IsUniqueId = valueAsString.StartsWith('/');

            // parse timezone info
            string timezoneAsString = IsUniqueId ? valueAsString.Substring(1) : valueAsString;
            string id = TZConvert.IanaToWindows(timezoneAsString);
            Timezone = TimeZoneInfo.FindSystemTimeZoneById(id);
        }

        public string Serialize()
        {
            return $"TZID={ (IsUniqueId ? "/" : "") }{ TZConvert.WindowsToIana(Timezone.Id) }";
        }

        #endregion Methods
    }
}
