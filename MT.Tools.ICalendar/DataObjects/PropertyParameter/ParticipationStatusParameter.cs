using System;
using System.Collections.Generic;
using System.Text;

namespace MT.Tools.ICalendar.DataObjects.PropertyParameter
{
    public enum ParticipationStatus
    {
        // participation status as defined in RFC 5545
        // ===========================================

        // general status (journal, todo, event)
        NeedsAction = 0,
        Accepted    = 1,
        Declined    = 2,

        // todo and event specific status
        Tentative   = 3,
        Delegated   = 4,

        // todo specific status
        Completed   = 5,
        InProcess   = 6,

        // custom, experimental status (not defined in standard)
        // =====================================================
        Custom      = 7
    }

    // participation status for todo components
    public enum TodoParticipationStatus
    {
        NeedsAction = 0,
        Accepted    = 1,
        Declined    = 2,
        Tentative   = 3,
        Delegated   = 4,
        Completed   = 5,
        InProcess   = 6
    }

    // participation status for event components
    public enum EventParticipationStatus
    {
        NeedsAction = 0,
        Accepted    = 1,
        Declined    = 2,
        Tentative   = 3,
        Delegated   = 4
    }

    // participation status for calendar components
    public enum CalendarParticipationStatus
    {
        NeedsAction = 0,
        Accepted    = 1,
        Declined    = 2
    }

    public class ParticipationStatusParameter : IPropertyParameter
    {
        #region Constructor

        public ParticipationStatusParameter() { }

        public ParticipationStatusParameter(TodoParticipationStatus status) { Status = (ParticipationStatus)(int)status; }

        public ParticipationStatusParameter(EventParticipationStatus status) { Status = (ParticipationStatus)(int)status; }

        public ParticipationStatusParameter(CalendarParticipationStatus status) { Status = (ParticipationStatus)(int)status; }

        public ParticipationStatusParameter(string customStatus) { Status = ParticipationStatus.Custom; CustomStatus = customStatus; }

        #endregion Constructor

        #region Members

        public PropertyParameterType Type => PropertyParameterType.ParticipationStatus;

        public ParticipationStatus Status { get; set; } = ParticipationStatus.NeedsAction;
        public string CustomStatus { get; set; } = null;

        #endregion Members

        #region Methods

        public void Deserialize(string content)
        {
            // remove heading and trailing white spaces
            content = content.Trim();

            // make sure that the parameter starts with PARTSTAT
            if (!content.StartsWith("PARTSTAT=")) { throw new ArgumentException("Invalid participation status parameter detected! Property parameter needs to start with PARTSTAT keyword!"); }

            // get the type as string and parse it
            string typeAsString = content.Substring(content.IndexOf('=') + 1).Trim();
            Status = deserializeParticipationStatus(typeAsString);
            CustomStatus = Status == ParticipationStatus.Custom ? typeAsString : null;
        }

        public string Serialize()
        {
            if (Status == ParticipationStatus.Custom && string.IsNullOrWhiteSpace(CustomStatus))
            {
                throw new ArgumentException("Custom participation status requires the status to be custom and custom type name to be not empty!");
            }

            return $"PARTSTAT={ serializeParticipationStatus(Status, CustomStatus) }";
        }

        #region Helpers

        private string serializeParticipationStatus(ParticipationStatus status, string customStatus = null)
        {
            switch (status)
            {
                case ParticipationStatus.NeedsAction: return "NEEDS-ACTION";
                case ParticipationStatus.Accepted:    return "ACCEPTED";
                case ParticipationStatus.Declined:    return "DECLINED";
                case ParticipationStatus.Tentative:   return "TENTATIVE";
                case ParticipationStatus.Delegated:   return "DELEGATED";
                case ParticipationStatus.Completed:   return "COMPLETED";
                case ParticipationStatus.InProcess:   return "IN-PROCESS";
                case ParticipationStatus.Custom:      return customStatus;
                default: throw new NotImplementedException("Unknown participation status detected!");
            }
        }

        private ParticipationStatus deserializeParticipationStatus(string content)
        {
            switch (content)
            {
                case "NEEDS-ACTION": return ParticipationStatus.NeedsAction;
                case "ACCEPTED":     return ParticipationStatus.Accepted;
                case "DECLINED":     return ParticipationStatus.Declined;
                case "TENTATIVE":    return ParticipationStatus.Tentative;
                case "DELEGATED":    return ParticipationStatus.Delegated;
                case "COMPLETED":    return ParticipationStatus.Completed;
                case "IN-PROCESS":   return ParticipationStatus.InProcess;
                default:             return ParticipationStatus.Custom;
            }
        }

        #endregion Helpers

        #endregion Methods
    }
}
