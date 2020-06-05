using MT.Tools.ICalendar.DataObjects.PropertyValue;
using System;
using System.Collections.Generic;
using System.Text;

namespace MT.Tools.ICalendar.DataObjects.PropertyParameter
{
    public class ValueTypeParameter : IPropertyParameter
    {
        #region Constructor

        public ValueTypeParameter() { }

        public ValueTypeParameter(PropertyValueType type) { ValueType = type; }

        public ValueTypeParameter(string customType) { ValueType = PropertyValueType.Custom; CustomValueType = customType; }

        #endregion Constructor

        #region Members

        public PropertyParameterType Type => PropertyParameterType.PropertyValueDataType;

        public PropertyValueType ValueType { get; set; }
        public string CustomValueType { get; set; } = null;

        #endregion Members

        #region Methods

        public void Deserialize(string content)
        {
            // remove heading and trailing white spaces
            content = content.Trim();

            // make sure that the parameter starts with VALUE
            if (!content.StartsWith("VALUE=")) { throw new ArgumentException("Invalid value type parameter detected! Property parameter needs to start with VALUE keyword!"); }

            // get the type as string and parse it
            string typeAsString = content.Substring(content.IndexOf('=') + 1).Trim();
            ValueType = deserializeValueType(typeAsString);
            CustomValueType = ValueType == PropertyValueType.Custom ? typeAsString : null;
        }

        public string Serialize()
        {
            if (ValueType == PropertyValueType.Custom && string.IsNullOrWhiteSpace(CustomValueType))
            {
                throw new ArgumentException("Custom value data type requires the type to be custom and custom type name to be not empty!");
            }

            return $"VALUE={ serializeValueType(ValueType, CustomValueType) }";
        }

        #region Helpers

        private string serializeValueType(PropertyValueType status, string customStatus = null)
        {
            switch (status)
            {
                case PropertyValueType.Binary:              return "BINARY";
                case PropertyValueType.Boolean:             return "BOOLEAN";
                case PropertyValueType.CalendarUserAddress: return "CAL-ADDRESS";
                case PropertyValueType.Date:                return "DATE";
                case PropertyValueType.DateTime:            return "DATE-TIME";
                case PropertyValueType.Duration:            return "DURATION";
                case PropertyValueType.Float:               return "FLOAT";
                case PropertyValueType.Integer32:           return "INTEGER";
                case PropertyValueType.PeriodOfTime:        return "PERIOD";
                case PropertyValueType.RecurrenceRule:      return "RECUR";
                case PropertyValueType.Text:                return "TEXT";
                case PropertyValueType.Time:                return "TIME";
                case PropertyValueType.Uri:                 return "URI";
                case PropertyValueType.UtcOffset:           return "UTC-OFFSET";
                case PropertyValueType.Custom:              return customStatus;
                default: throw new NotImplementedException("Unknown property value type detected!");
            }
        }

        private PropertyValueType deserializeValueType(string content)
        {
            switch (content)
            {
                case "BINARY":      return PropertyValueType.Binary;
                case "BOOLEAN":     return PropertyValueType.Boolean;
                case "CAL-ADDRESS": return PropertyValueType.CalendarUserAddress;
                case "DATE":        return PropertyValueType.Date;
                case "DATE-TIME":   return PropertyValueType.DateTime;
                case "DURATION":    return PropertyValueType.Duration;
                case "FLOAT":       return PropertyValueType.Float;
                case "INTEGER":     return PropertyValueType.Integer32;
                case "PERIOD":      return PropertyValueType.PeriodOfTime;
                case "RECUR":       return PropertyValueType.RecurrenceRule;
                case "TEXT":        return PropertyValueType.Text;
                case "TIME":        return PropertyValueType.Time;
                case "URI":         return PropertyValueType.Uri;
                case "UTC-OFFSET":  return PropertyValueType.UtcOffset;
                default:            return PropertyValueType.Custom;
            }
        }

        #endregion Helpers

        #endregion Methods
    }
}
