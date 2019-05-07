using MT.Tools.ICalendar.DataObjects.PropertyValue.Primitive;
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

        public CalendarUserTypeParameter(CalendarUserType type, string customType = null)
        {
            // make sure that custom type is specified when customType is not null
            if (customType != null && type != CalendarUserType.Custom)
            {
                throw new ArgumentException("Invalid custom user type! Type needs to be custom as well when custom type name is explicitly set!");
            }

            UserType = new EnumValue<CalendarUserType>(type);
            CustomType = new TextValue(customType);
        }

        #endregion Constructor

        #region Members

        public PropertyParameterType Type => PropertyParameterType.CalendarUserType;

        public EnumValue<CalendarUserType> UserType { get; set; } = new EnumValue<CalendarUserType>(CalendarUserType.Individual);
        public TextValue CustomType { get; set; } = null;

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

            // try to deserialize the content as enum
            EnumValue<CalendarUserType> type;
            bool isStandardType = ObjectSerializer.TryDeserialize(typeAsString, out type);

            // check if a custom value is specified
            if (!isStandardType || type.Value == CalendarUserType.Custom)
            {
                type = new EnumValue<CalendarUserType>(CalendarUserType.Custom);
                string typeString = (type.Value == CalendarUserType.Custom) ? "CUSTOM" : typeAsString;
                CustomType = new TextValue(typeString);
            }

            UserType = type;
        }

        public string Serialize()
        {
            return $"CUTYPE={ ((UserType.Value == CalendarUserType.Custom) ? CustomType.Serialize() : UserType.Serialize()) }";
        }

        #endregion Methods
    }
}
