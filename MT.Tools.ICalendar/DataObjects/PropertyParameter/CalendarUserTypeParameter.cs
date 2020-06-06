using MT.Tools.ICalendar.DataObjects.PropertyValue;
using System;
using System.Collections.Generic;
using System.Text;

namespace MT.Tools.ICalendar.DataObjects.PropertyParameter
{
    public enum CalendarUserType
    {
        // types defined in RFC 5545
        Individual,
        Group,
        Resource,
        Room,
        Unknown,

        // additional type for all custom types
        Custom
    }

    public class CalendarUserTypeParameter : IPropertyParameter
    {
        #region Constructor

        public CalendarUserTypeParameter() { }

        public CalendarUserTypeParameter(CalendarUserType type) { UserType = type; }

        public CalendarUserTypeParameter(string customType) { UserType = CalendarUserType.Custom; CustomUserType = customType; }

        #endregion Constructor

        #region Members

        public PropertyParameterType Type => PropertyParameterType.CalendarUserType;

        public CalendarUserType UserType { get; set; } = CalendarUserType.Individual;
        public string CustomUserType { get; set; } = null;

        #endregion Members

        #region Methods

        public void Deserialize(string content)
        {
            // remove heading and trailing white spaces
            content = content.Trim();

            // make sure that the parameter starts with CUTYPE
            if (!content.StartsWith("CUTYPE=")) { throw new ArgumentException("Invalid culture type content detected! Property parameter needs to start with CUTYPE keyword!"); }

            // get the user type as string
            string typeAsString = content.Substring(content.IndexOf('=') + 1).Trim();

            // parse user type and potential custom types
            EnumValue<CalendarUserType> type;
            UserType = ObjectSerializer.TryDeserialize(typeAsString, out type) ? type.Value : CalendarUserType.Custom;
            CustomUserType = (type.Value == CalendarUserType.Custom) ? typeAsString : null;
        }

        public string Serialize()
        {
            if (UserType == CalendarUserType.Custom && string.IsNullOrWhiteSpace(CustomUserType))
            {
                throw new ArgumentException("Invalid custom user type! Type needs to be custom as well when custom type name is explicitly set!");
            }

            return $"CUTYPE={ (UserType == CalendarUserType.Custom ? CustomUserType : UserType.ToString().ToUpper()) }";
        }

        #endregion Methods
    }
}
