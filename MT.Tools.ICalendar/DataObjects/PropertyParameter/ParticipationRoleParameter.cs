using System;
using System.Collections.Generic;
using System.Text;

namespace MT.Tools.ICalendar.DataObjects.PropertyParameter
{
    public enum ParticipationRole
    {
        // participation role as defined in RFC 5545
        Chair,
        RequiredParticipant,
        OptionalParticipant,
        NonParticipant,

        // custom, experimental status (not defined in standard)
        Custom
    }

    public class ParticipationRoleParameter : IPropertyParameter
    {
        #region Constructor

        public ParticipationRoleParameter() { }

        public ParticipationRoleParameter(ParticipationRole role) { Role = role; }

        public ParticipationRoleParameter(string customRole) { Role = ParticipationRole.Custom; CustomRole = customRole; }

        #endregion Constructor

        #region Members

        public PropertyParameterType Type => PropertyParameterType.ParticipationRole;

        public ParticipationRole Role { get; set; }
        public string CustomRole { get; set; }

        #endregion Members

        #region Methods

        public void Deserialize(string content)
        {
            // remove heading and trailing white spaces
            content = content.Trim();

            // make sure that the parameter starts with ROLE
            if (!content.StartsWith("ROLE=")) { throw new ArgumentException("Invalid participation role parameter detected! Property parameter needs to start with ROLE keyword!"); }

            // get the type as string and parse it
            string typeAsString = content.Substring(content.IndexOf('=') + 1).Trim();
            Role = deserializeParticipationRole(typeAsString);
            CustomRole = Role == ParticipationRole.Custom ? typeAsString : null;
        }

        public string Serialize()
        {
            if (Role == ParticipationRole.Custom && string.IsNullOrWhiteSpace(CustomRole))
            {
                throw new ArgumentException("Custom participation role requires the role to be custom and custom role name to be not empty!");
            }

            return $"ROLE={ serializeParticipationRole(Role, CustomRole) }";
        }

        #region Helpers

        private string serializeParticipationRole(ParticipationRole role, string customStatus = null)
        {
            switch (role)
            {
                case ParticipationRole.Chair:               return "CHAIR";
                case ParticipationRole.RequiredParticipant: return "REQ-PARTICIPANT";
                case ParticipationRole.OptionalParticipant: return "OPT-PARTICIPANT";
                case ParticipationRole.NonParticipant:      return "NON-PARTICIPANT";
                case ParticipationRole.Custom:              return customStatus;
                default: throw new NotImplementedException("Unknown participation status detected!");
            }
        }

        private ParticipationRole deserializeParticipationRole(string content)
        {
            switch (content.ToUpper())
            {
                case "CHAIR":           return ParticipationRole.Chair;
                case "REQ-PARTICIPANT": return ParticipationRole.RequiredParticipant;
                case "OPT-PARTICIPANT": return ParticipationRole.OptionalParticipant;
                case "NON-PARTICIPANT": return ParticipationRole.NonParticipant;
                default:                return ParticipationRole.Custom;
            }
        }

        #endregion Helpers

        #endregion Methods
    }
}
