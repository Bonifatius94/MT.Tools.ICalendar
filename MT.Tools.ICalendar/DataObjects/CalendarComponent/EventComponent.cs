using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MT.Tools.ICalendar.DataObjects.Collection;
using MT.Tools.ICalendar.DataObjects.Property;
using MT.Tools.ICalendar.DataObjects.PropertyValue;
using MT.Tools.ICalendar.DataObjects.PropertyValue.Other;
using MT.Tools.ICalendar.DataObjects.PropertyValue.Primitive;
using MT.Tools.ICalendar.DataObjects.PropertyValue.Time;

namespace MT.Tools.ICalendar.DataObjects.CalendarComponent
{
    public class EventComponent : ICalendarComponent, IPropertyCollection, IComponentCollection<AlarmComponent>
    {
        #region Constants

        public const string PROPERTY_DTSTAMP = "DTSTAMP";
        public const string PROPERTY_UID = "UID";
        public const string PROPERTY_DTSTART = "DTSTART";

        public const string PROPERTY_ATTACH = "ATTACH";
        public const string PROPERTY_ATTENDEE = "ATTENDEE";

        #endregion Constants

        #region Constructor

        public EventComponent() { }

        #endregion Constructor

        #region Members

        public CalendarComponentType Type => CalendarComponentType.Event;

        public IEnumerable<ICalendarProperty> Properties { get; } = new List<ICalendarProperty>();

        public IEnumerable<AlarmComponent> Components { get; } = new List<AlarmComponent>();

        // required unique properties
        public DateTimeValue DtStamp => getPropertyValue(PROPERTY_DTSTAMP) as DateTimeValue;
        public TextValue Uid => getPropertyValue(PROPERTY_UID) as TextValue;
        public DateTimeValue DtStart => getPropertyValue(PROPERTY_DTSTART) as DateTimeValue;

        // optional unique properties


        // optional non-unique properties
        public IEnumerable<TextValue> Attachments => Properties.Where(x => x.Key.Equals(PROPERTY_ATTACH)).Cast<TextValue>();
        public IEnumerable<CalendarUserAddressValue> Attendees => Properties.Where(x => x.Key.Equals(PROPERTY_ATTENDEE)).Cast<CalendarUserAddressValue>();


        // required unique properties
        // ==========================
        // dtstamp
        // uid

        // optional unique properties
        // ==========================
        // dtstart (only required if METHOD is not defined in iCalendar object)
        // class
        // created
        // description
        // geo
        // last-mod
        // location
        // organizer
        // priority
        // seq
        // status
        // summary
        // transp
        // url
        // recurid
        // rrule (only warning at multiple occurances)
        // dtend or duration

        // optional non-unique properties
        // ==============================
        // attach
        // attendee
        // categories
        // comment
        // contact
        // exdate
        // rstatus
        // related
        // resources
        // rdate

        // custom additional properties (iana-prop / x-prop)

        #endregion Members

        #region Methods

        public void Deserialize(string content)
        {
            // TODO: implement component
            throw new NotImplementedException();
        }

        public string Serialize()
        {
            // TODO: implement component
            throw new NotImplementedException();
        }

        #region Helpers

        private IPropertyValue getPropertyValue(string key) => Properties.Where(x => x.Key.Equals(key)).First().Value;

        #endregion Helpers

        #endregion Methods
    }
}
