using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MT.Tools.ICalendar.DataObjects.Collection;
using MT.Tools.ICalendar.DataObjects.Property;
using MT.Tools.ICalendar.DataObjects.PropertyValue;
using MT.Tools.ICalendar.DataObjects.PropertyValue.Primitive;
using MT.Tools.ICalendar.DataObjects.PropertyValue.Time;

namespace MT.Tools.ICalendar.DataObjects.CalendarComponent
{
    public class EventComponent : ICalendarComponent, IPropertyCollection, IComponentCollection<AlarmComponent>
    {
        #region Constants

        public const string PROPERTY_DTSTAMP = "DTSTAMP";
        public const string PROPERTY_UID = "UID";

        public const string PROPERTY_ATTACH = "ATTACH";

        #endregion Constants

        #region Constructor

        public EventComponent() { }

        //public EventComponent()
        //{

        //}

        #endregion Constructor

        #region Members

        public CalendarComponentType Type => CalendarComponentType.Event;

        public IEnumerable<ICalendarProperty> Properties { get; } = new List<ICalendarProperty>();

        public IEnumerable<AlarmComponent> Components { get; } = new List<AlarmComponent>();

        // required unique properties
        public DateTimeValue DtStamp => getPropertyValue(PROPERTY_DTSTAMP) as DateTimeValue;
        public TextValue Uid => getPropertyValue(PROPERTY_UID) as TextValue;

        // optional unique properties



        // optional non-unique properties
        public TextValue Attach => getPropertyValue(PROPERTY_ATTACH) as TextValue;


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
            throw new NotImplementedException();
        }

        public string Serialize()
        {
            throw new NotImplementedException();
        }

        #region Helpers

        private IPropertyValueImpl getPropertyValue(string key) => Properties.Where(x => x.Key.Equals(key)).First().Value;

        #endregion Helpers

        #endregion Methods
    }
}
