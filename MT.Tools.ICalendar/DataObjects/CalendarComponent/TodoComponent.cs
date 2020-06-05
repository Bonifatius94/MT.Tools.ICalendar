using MT.Tools.ICalendar.DataObjects.PropertyBase;
using System;
using System.Collections.Generic;
using System.Text;

namespace MT.Tools.ICalendar.DataObjects.CalendarComponent
{
    public class TodoComponent : ICalendarComponent
    {
        #region Constants

        public const string PROPERTY_DTSTAMP = "DTSTAMP";
        public const string PROPERTY_UID = "UID";
        public const string PROPERTY_DTSTART = "DTSTART";

        #endregion Constants

        #region Constructor

        public TodoComponent() { }
        // TODO: add other useful constructors (with parameters)

        #endregion Constructor

        #region Members

        public CalendarComponentType Type => CalendarComponentType.Todo;

        public IEnumerable<ICalendarProperty> Properties { get; } = new List<ICalendarProperty>();

        // required properties (S. 56):
        // ====================
        // dtstamp
        // uid

        // optional unique properties:
        // ===========================
        // class
        // completed 
        // created 
        // description
        // dtstart 
        // geo 
        // last-mod 
        // location 
        // organizer
        // percent 
        // priority 
        // recurid 
        // seq 
        // status
        // summary 
        // url

        // optional unique properties (only warning if not unique):
        // ========================================================
        // rrule

        // (either due or duration, not both; duration forces dtstart to exist)
        // due 
        // duration

        // optional non-unique properties:
        // ===============================
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
