using MT.Tools.ICalendar.DataObjects.PropertyValue.Primitive;
using System;
using System.Collections.Generic;
using System.Text;

namespace MT.Tools.ICalendar.DataObjects.PropertyParameter
{
    public enum RelationshipType
    {
        // types defined in RFC 5545
        Parent,
        Child,
        Sibling,

        // additional type for all custom types
        Custom
    }

    public class RelationshipTypeParameter : IPropertyParameter
    {
        #region Constructor

        public RelationshipTypeParameter() { }

        public RelationshipTypeParameter(RelationshipType type) { RelType = type; }

        public RelationshipTypeParameter(string customType) { RelType = RelationshipType.Custom; CustomRelType = customType; }

        #endregion Constructor

        #region Members

        public PropertyParameterType Type => PropertyParameterType.RelationshipType;

        public RelationshipType RelType { get; set; }
        public string CustomRelType { get; set; }

        #endregion Members

        #region Methods

        public void Deserialize(string content)
        {
            // remove heading and trailing white spaces
            content = content.Trim();

            // make sure that the parameter starts with CUTYPE
            if (!content.StartsWith("RELTYPE=")) { throw new ArgumentException("Invalid relationship type content detected! Property parameter needs to start with RELTYPE keyword!"); }

            // get the user type as string
            string typeAsString = content.Substring(content.IndexOf('=') + 1).Trim();

            // parse user type and potential custom types
            EnumValue<RelationshipType> type;
            RelType = ObjectSerializer.TryDeserialize(typeAsString, out type) ? type.Value : RelationshipType.Custom;
            CustomRelType = (type.Value == RelationshipType.Custom) ? typeAsString : null;
        }

        public string Serialize()
        {
            if (RelType == RelationshipType.Custom && string.IsNullOrWhiteSpace(CustomRelType))
            {
                throw new ArgumentException("Invalid custom relationship type! Type needs to be custom as well when custom type name is explicitly set!");
            }

            return $"RELTYPE={ (RelType == RelationshipType.Custom ? CustomRelType : RelType.ToString().ToUpper()) }";
        }

        #endregion Methods
    }
}
