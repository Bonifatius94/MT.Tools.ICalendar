using System;
using System.Collections.Generic;
using System.Linq;
using MT.Tools.ICalendar.DataObjects.Collection;
using MT.Tools.ICalendar.DataObjects.ComponentProperty;
using MT.Tools.ICalendar.DataObjects.PropertyValue;

namespace MT.Tools.ICalendar.DataObjects.CalendarComponent
{
    public class EventComponent : ICalendarComponent, IComponentCollection<AlarmComponent>
    {
        #region Constructor

        public EventComponent() { }

        #endregion Constructor

        #region Members

        public CalendarComponentType Type => CalendarComponentType.Event;

        public IEnumerable<IComponentProperty> Properties { get; } = new List<IComponentProperty>();

        public IEnumerable<AlarmComponent> Components { get; } = new List<AlarmComponent>();


        // optional non-unique properties
        public IEnumerable<AttachmentProperty> Attachments => Properties.Where(x => x.GetType() == typeof(AttachmentProperty)).Cast<AttachmentProperty>().ToList();
        //public IEnumerable<CalendarUserAddressValue> Attendees => Properties.Where(x => x.Key.Equals(PROPERTY_ATTENDEE)).Cast<CalendarUserAddressValue>().ToList();

        // TODO: implement attributes for components

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

        #endregion Methods
    }
}
