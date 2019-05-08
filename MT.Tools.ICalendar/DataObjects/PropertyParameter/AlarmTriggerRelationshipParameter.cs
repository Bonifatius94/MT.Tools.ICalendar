using MT.Tools.ICalendar.DataObjects.PropertyValue.Primitive;
using System;
using System.Collections.Generic;
using System.Text;

namespace MT.Tools.ICalendar.DataObjects.PropertyParameter
{
    public enum AlarmTriggerRelationshipType
    {
        Start,
        End
    }

    public class AlarmTriggerRelationshipParameter : IPropertyParameter
    {
        #region Constructor

        public AlarmTriggerRelationshipParameter() { }

        public AlarmTriggerRelationshipParameter(AlarmTriggerRelationshipType type) { RelationshipType = type; }

        #endregion Constructor

        #region Members

        public PropertyParameterType Type => PropertyParameterType.AlarmTriggerRelationship;

        public AlarmTriggerRelationshipType RelationshipType { get; set; }

        #endregion Members

        #region Methods

        public void Deserialize(string content)
        {
            // remove heading and trailing white spaces
            content = content.Trim();

            // make sure that the parameter starts with CUTYPE
            if (!content.StartsWith("RELATED=")) { throw new ArgumentException("Invalid alarm trigger relationship parameter detected! Property parameter needs to start with RELATED keyword!"); }

            // parse relationship type
            string typeAsString = content.Substring(content.IndexOf('=') + 1).Trim();
            RelationshipType = ObjectSerializer.Deserialize<EnumValue<AlarmTriggerRelationshipType>>(typeAsString).Value;
        }

        public string Serialize()
        {
            return $"RELATED={ RelationshipType.ToString().ToUpper() }";
        }

        #endregion Methods
    }
}
