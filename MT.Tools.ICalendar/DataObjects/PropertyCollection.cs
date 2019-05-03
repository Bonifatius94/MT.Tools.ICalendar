using MT.Tools.ICalendar.DataObjects.Property;
using MT.Tools.ICalendar.DataObjects.PropertyValue;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MT.Tools.ICalendar.DataObjects
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
        public SimplePropertyCollection(IEnumerable<CalendarProperty<ValueT>> props) { Properties = props; }
        
        #endregion Constructor

        #region Members

        public IEnumerable<ICalendarProperty<ValueT>> Properties { get; } = new List<CalendarProperty<ValueT>>();

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

    //public class PropertyDictionary<ValueT> : Dictionary<string, ValueT>
    //    where ValueT : IPropertyValueImpl
    //{
    //    #region Constructor

    //    public PropertyDictionary() : base() { }
    //    public PropertyDictionary(IDictionary<string, ValueT> dict) : base(dict) { }
    //    public PropertyDictionary(IEnumerable<KeyValuePair<string, ValueT>> pairs) : base(pairs) { }

    //    #endregion Constructor
    //}

    //public class PropertyDictionary : PropertyDictionary<IPropertyValueImpl>
    //{
    //    #region Constructor

    //    public PropertyDictionary() : base() { }
    //    public PropertyDictionary(IDictionary<string, IPropertyValueImpl> dict) : base(dict) { }
    //    public PropertyDictionary(IEnumerable<KeyValuePair<string, IPropertyValueImpl>> pairs) : base(pairs) { }

    //    #endregion Constructor
    //}
}
