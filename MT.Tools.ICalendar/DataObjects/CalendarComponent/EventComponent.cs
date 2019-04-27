using System;
using System.Collections.Generic;
using System.Text;

namespace MT.Tools.ICalendar.DataObjects.CalendarComponent
{
    public class EventComponent : ICalendarComponent
    {
        #region Constructor



        #endregion Constructor

        #region Members

        public CalendarComponentType Type => CalendarComponentType.Event;

        // required unique properties

        // optional unique properties

        // optional non-unique properties

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
