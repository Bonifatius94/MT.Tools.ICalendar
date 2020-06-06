using MT.Tools.ICalendar.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace MT.Tools.ICalendar.DataObjects.PropertyValue
{
    public enum EventStatus
    {
        Tentative   = 0,
        Confirmed   = 1,
        Cancelled   = 2
    }

    public enum TodoStatus
    {
        Cancelled   = 2,
        NeedsAction = 3,
        Completed   = 4,
        InProcess   = 5
    }

    public enum JournalStatus
    {
        Cancelled   = 2,
        Draft       = 7,
        Final       = 8
    }

    // TODO: find better enum name
    public enum Status
    {
        Tentative   = 0,
        Confirmed   = 1,
        Cancelled   = 2,
        NeedsAction = 3,
        Completed   = 4,
        InProcess   = 5,
        Draft       = 7,
        Final       = 8
    }

    public class StatusValue : IPropertyValue
    {
        #region Constants

        public const string STATUS_TENTATIVE = "TENTATIVE";
        public const string STATUS_CONFIRMED = "CONFIRMED";
        public const string STATUS_CANCELLED = "CANCELLED";
        public const string STATUS_NEEDS_ACTION = "NEEDS-ACTION";
        public const string STATUS_COMPLETED = "COMPLETED";
        public const string STATUS_IN_PROCESS = "IN-PROCESS";
        public const string STATUS_DRAFT = "DRAFT";
        public const string STATUS_FINAL = "FINAL";

        private static readonly Dictionary<Status, string> STATES_SERIALIZE = new Dictionary<Status, string>()
        {
            { Status.Tentative,   STATUS_TENTATIVE     },
            { Status.Confirmed,   STATUS_CONFIRMED     },
            { Status.Cancelled,   STATUS_CANCELLED     },
            { Status.NeedsAction, STATUS_NEEDS_ACTION  },
            { Status.Completed,   STATUS_COMPLETED     },
            { Status.InProcess,   STATUS_IN_PROCESS    },
            { Status.Draft,       STATUS_DRAFT         },
            { Status.Final,       STATUS_FINAL         },
        };

        private static readonly Dictionary<string, Status> STATES_DESERIALIZE = STATES_SERIALIZE.InversePairs();

        #endregion Constants

        #region Constructor

        public StatusValue() { }

        public StatusValue(EventStatus status) { Status = (Status)(int)status; }

        public StatusValue(TodoStatus status) { Status = (Status)(int)status; }

        public StatusValue(JournalStatus status) { Status = (Status)(int)status; }

        #endregion Constructor

        public PropertyValueType Type => PropertyValueType.Text;

        public Status Status { get; set; }

        public void Deserialize(string content)
        {
            Status = STATES_DESERIALIZE[content.Trim().ToUpper()];
        }

        public string Serialize()
        {
            return $"{ STATES_SERIALIZE[Status] }";
        }
    }
}
