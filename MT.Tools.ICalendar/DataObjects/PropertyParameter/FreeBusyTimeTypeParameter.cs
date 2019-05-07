using MT.Tools.ICalendar.DataObjects.PropertyValue.Primitive;
using System;
using System.Collections.Generic;
using System.Text;

namespace MT.Tools.ICalendar.DataObjects.PropertyParameter
{
    public enum FreeBusyTimeType
    {
        // types defined in RFC 5545
        Free,
        Busy,
        BusyUnavailable,
        BusyTentative,

        // represents all other custom types
        Custom
    }

    public class FreeBusyTimeTypeParameter : IPropertyParameter
    {
        #region Constructor

        public FreeBusyTimeTypeParameter() { }

        public FreeBusyTimeTypeParameter(FreeBusyTimeType type, string customName = null)
        {
            // make sure that custom type is specified when customName is not null
            if (customName != null && type != FreeBusyTimeType.Custom)
            {
                throw new ArgumentException("Invalid free/busy time type! Type needs to be custom as well when custom type name is explicitly set!");
            }

            FreeBusyType = type;
            CustomFreeBusyType = customName;
        }

        #endregion Constructor

        #region Members

        public PropertyParameterType Type => PropertyParameterType.FreeBusyTimeType;

        public FreeBusyTimeType FreeBusyType { get; set; }
        public string CustomFreeBusyType { get; set; }

        #endregion Members

        #region Methods

        public void Deserialize(string content)
        {
            // remove heading and trailing white spaces
            content = content.Trim();

            // make sure that the parameter starts with FBTYPE
            if (!content.StartsWith("FBTYPE=")) { throw new ArgumentException("Invalid free/busy time type parameter detected! Property parameter needs to start with FBTYPE keyword!"); }

            // get the type as string and parse it
            string typeAsString = content.Substring(content.IndexOf('=') + 1).Trim();
            FreeBusyType = deserializeFreeBusyTimeType(typeAsString);
            CustomFreeBusyType = FreeBusyType == FreeBusyTimeType.Custom ? typeAsString : null;
        }

        public string Serialize()
        {
            return $"FBTYPE={ serializeFreeBusyTimeType(FreeBusyType, CustomFreeBusyType) }";
        }

        #region Helpers

        private string serializeFreeBusyTimeType(FreeBusyTimeType value, string customName = null)
        {
            switch (value)
            {
                case FreeBusyTimeType.Free: return "FREE";
                case FreeBusyTimeType.Busy: return "BUSY";
                case FreeBusyTimeType.BusyUnavailable: return "BUSY-UNAVAILABLE";
                case FreeBusyTimeType.BusyTentative: return "BUSY-TENTATIVE";
                default: return customName;
            }
        }

        private FreeBusyTimeType deserializeFreeBusyTimeType(string value)
        {
            switch (value.ToUpper())
            {
                case "FREE": return FreeBusyTimeType.Free;
                case "BUSY": return FreeBusyTimeType.Busy;
                case "BUSY-UNAVAILABLE": return FreeBusyTimeType.BusyUnavailable;
                case "BUSY-TENTATIVE": return FreeBusyTimeType.BusyTentative;
                default: return FreeBusyTimeType.Custom;
            }
        }

        #endregion Helpers

        #endregion Methods
    }
}
