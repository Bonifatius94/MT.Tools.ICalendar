using MT.Tools.ICalendar.DataObjects.CalendarComponent;
using MT.Tools.ICalendar.DataObjects.PropertyParameter;
using MT.Tools.ICalendar.DataObjects.PropertyValue;
using System;
using System.Collections.Generic;
using System.Text;

namespace MT.Tools.ICalendar.DataObjects.Factory
{
    public static class CalendarFactory
    {
        #region CalendarComponent

        public static ICalendarComponent DeserializeCalendarComponent(string content)
        {
            ICalendarComponent component;

            // make sure that heading whitespaces get removed
            content = content.TrimStart();

            // find out the component type and parse the component accordingly
            if (content.ToUpper().StartsWith("BEGIN:VEVENT"))
            {
                component = ObjectSerializer.Deserialize<EventComponent>(content);
            }
            else if (content.ToUpper().StartsWith("BEGIN:VTODO"))
            {
                component = ObjectSerializer.Deserialize<TodoComponent>(content);
            }
            else if (content.ToUpper().StartsWith("BEGIN:VJOURNAL"))
            {
                component = ObjectSerializer.Deserialize<JournalComponent>(content);
            }
            else if (content.ToUpper().StartsWith("BEGIN:VFREEBUSY"))
            {
                component = ObjectSerializer.Deserialize<FreeBusyComponent>(content);
            }
            else if (content.ToUpper().StartsWith("BEGIN:VTIMEZONE"))
            {
                component = ObjectSerializer.Deserialize<TimezoneComponent>(content);
            }
            else if (content.ToUpper().StartsWith("BEGIN:VALARM"))
            {
                component = ObjectSerializer.Deserialize<AlarmComponent>(content);
            }
            else if (content.ToUpper().StartsWith("BEGIN:"))
            {
                component = ObjectSerializer.Deserialize<CustomComponent>(content);
            }
            else
            {
                throw new ArgumentException("Invalid iCalendar component detected! Missing BEGIN markup!");
            }

            return component;
        }

        #endregion CalendarComponent

        #region PropertyParameter

        public static IPropertyParameter DeserializePropertyParameter(string content)
        {
            IPropertyParameter parameter;

            // make sure that heading whitespaces get removed
            content = content.TrimStart();

            // find out the component type and parse the component accordingly
            if (content.ToUpper().StartsWith("RELATED"))
            {
                parameter = ObjectSerializer.Deserialize<AlarmTriggerRelationshipParameter>(content);
            }
            else if (content.ToUpper().StartsWith("ALTREP"))
            {
                parameter = ObjectSerializer.Deserialize<AlternateTextRepresentationParamter>(content);
            }
            else if (content.ToUpper().StartsWith("CUTYPE"))
            {
                parameter = ObjectSerializer.Deserialize<CalendarUserTypeParameter>(content);
            }
            else if (content.ToUpper().StartsWith("CN"))
            {
                parameter = ObjectSerializer.Deserialize<CommonNameParameter>(content);
            }
            else if (content.ToUpper().StartsWith("DELEGATED-TO"))
            {
                parameter = ObjectSerializer.Deserialize<DelegateeParameter>(content);
            }
            else if (content.ToUpper().StartsWith("DELEGATED-FROM"))
            {
                parameter = ObjectSerializer.Deserialize<DelegatorParameter>(content);
            }
            else if (content.ToUpper().StartsWith("DIR"))
            {
                parameter = ObjectSerializer.Deserialize<DirectoryEntryReferenceParameter>(content);
            }
            else if (content.ToUpper().StartsWith("FMTTYPE"))
            {
                parameter = ObjectSerializer.Deserialize<FormatTypeParameter>(content);
            }
            else if (content.ToUpper().StartsWith("FBTYPE"))
            {
                parameter = ObjectSerializer.Deserialize<FreeBusyTimeTypeParameter>(content);
            }
            else if (content.ToUpper().StartsWith("MEMBER"))
            {
                parameter = ObjectSerializer.Deserialize<GroupOrListMembershipParameter>(content);
            }
            else if (content.ToUpper().StartsWith("ENCODING"))
            {
                parameter = ObjectSerializer.Deserialize<InlineEncodingParameter>(content);
            }
            else if (content.ToUpper().StartsWith("LANGUAGE"))
            {
                parameter = ObjectSerializer.Deserialize<LanguageParameter>(content);
            }
            else if (content.ToUpper().StartsWith("ROLE"))
            {
                parameter = ObjectSerializer.Deserialize<ParticipationRoleParameter>(content);
            }
            else if (content.ToUpper().StartsWith("PARTSTAT"))
            {
                parameter = ObjectSerializer.Deserialize<ParticipationStatusParameter>(content);
            }
            else if (content.ToUpper().StartsWith("RANGE"))
            {
                parameter = ObjectSerializer.Deserialize<RecurrenceIdRangeParameter>(content);
            }
            else if (content.ToUpper().StartsWith("RELTYPE"))
            {
                parameter = ObjectSerializer.Deserialize<RelationshipTypeParameter>(content);
            }
            else if (content.ToUpper().StartsWith("RSVP"))
            {
                parameter = ObjectSerializer.Deserialize<RsvpExpectationParameter>(content);
            }
            else if (content.ToUpper().StartsWith("SENT-BY"))
            {
                parameter = ObjectSerializer.Deserialize<SentByParameter>(content);
            }
            else if (content.ToUpper().StartsWith("TZID"))
            {
                parameter = ObjectSerializer.Deserialize<TimeZoneIdParameter>(content);
            }
            else if (content.ToUpper().StartsWith("VALUE"))
            {
                parameter = ObjectSerializer.Deserialize<ValueTypeParameter>(content);
            }
            else { throw new ArgumentException("Invalid iCalendar component detected! Missing BEGIN markup!"); }

            return parameter;
        }

        #endregion PropertyParameter

        #region PropertyValue

        //public static IPropertyValue DeserializePropertyValue(string content)
        //{
        //    IPropertyValue value;

        //    // make sure that heading whitespaces get removed
        //    content = content.TrimStart();

        //    // TODO: property values don't have specific markups. therefore either regex recognition or try-parse approach could work.

        //    throw new NotImplementedException();

        //    return value;
        //}

        #endregion PropertyValue
    }
}
