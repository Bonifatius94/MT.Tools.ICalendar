using System;
using System.Collections.Generic;
using System.Text;

namespace MT.Tools.ICalendar.DataObjects.CalendarComponent
{
    public class EventComponent : ICalendarComponent
    {
        #region Constructor

        public EventComponent() { }

        public EventComponent()
        {

        }

        #endregion Constructor

        #region Members

        public CalendarComponentType Type => CalendarComponentType.Event;

        // required unique properties
        // ==========================
        // dtstamp
        // uid
        // dtstart (only required if METHOD is not defined in iCalendar object)

        // optional unique properties
        // ==========================
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
            
        }

        public string Serialize()
        {
            throw new NotImplementedException();
        }

        #endregion Methods
    }
}
