using System;
using System.Collections.Generic;
using System.Text;

namespace MT.Tools.ICalendar.DataObjects.PropertyParameter
{
    public enum PropertyParameterType
    {
        AlternateTextRepresentation,
        CommonName,
        CalendarUserType,
        Delegator,
        Delegatee,
        DirectoryEntryReference,
        InlineEncoding,
        FormatType,
        FreeBusyTimeType,
        Language,
        GroupOrListMembership,
        ParticipationStatus,
        RecurrenceIdentifierRange,
        AlarmTriggerRelationship,
        RelationshipType,
        RelationshipRole,
        RsvpExpectation,
        SentBy,
        ReferenceToTimeZoneObject,
        PropertyValueDataType,
        Other
    }

    public interface IPropertyParameter : ISerializableObject
    {
        #region Members

        PropertyParameterType Type { get; }

        #endregion Members
    }
}
