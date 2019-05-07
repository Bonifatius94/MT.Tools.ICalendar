using MT.Tools.ICalendar.DataObjects.CalendarComponent;
using System;
using System.Collections.Generic;
using System.Text;

namespace MT.Tools.ICalendar.DataObjects.Collection
{
    public interface IComponentCollection
    {
        #region Members

        IEnumerable<ICalendarComponent> Components { get; }

        #endregion Members
    }

    public interface IComponentCollection<CompT>
        where CompT : ICalendarComponent
    {
        #region Members

        IEnumerable<CompT> Components { get; }

        #endregion Members
    }
}
