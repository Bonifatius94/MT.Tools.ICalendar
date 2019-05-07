using MT.Tools.ICalendar.DataObjects.Property;
using MT.Tools.ICalendar.DataObjects.PropertyValue;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MT.Tools.ICalendar.DataObjects.Collection
{
    public interface IPropertyCollection<ValueT>
        where ValueT : IPropertyValueImpl, new()
    {
        #region Members

        IEnumerable<ICalendarProperty<ValueT>> Properties { get; }

        #endregion Members
    }

    public interface IPropertyCollection
    {
        #region Members

        IEnumerable<ICalendarProperty> Properties { get; }

        #endregion Members
    }

    public class SimplePropertyCollection<ValueT> : IPropertyCollection<ValueT>
        where ValueT : IPropertyValueImpl, new()
    {
        #region Constructor

        public SimplePropertyCollection() { }
        public SimplePropertyCollection(IEnumerable<SimpleCalendarProperty<ValueT>> props) { Properties = props; }
        
        #endregion Constructor

        #region Members

        public IEnumerable<ICalendarProperty<ValueT>> Properties { get; } = new List<SimpleCalendarProperty<ValueT>>();

        #endregion Members
    }

    public class SimplePropertyCollection : IPropertyCollection
    {
        #region Constructor

        public SimplePropertyCollection() { }
        public SimplePropertyCollection(IEnumerable<GenericCalendarProperty> props) { Properties = props; }

        #endregion Constructor

        #region Members

        public IEnumerable<ICalendarProperty> Properties { get; } = new List<GenericCalendarProperty>();

        #endregion Members
    }
}
