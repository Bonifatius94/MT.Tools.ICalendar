//using MT.Tools.ICalendar.DataObjects.PropertyBase;
//using MT.Tools.ICalendar.DataObjects.Value;
//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace MT.Tools.ICalendar.DataObjects.Collection
//{
//    public interface IPropertyCollection<ValueT>
//        where ValueT : IPropertyValue, new()
//    {
//        #region Members

//        IEnumerable<ICalendarProperty<ValueT>> Properties { get; }

//        #endregion Members
//    }

//    public interface IPropertyCollection
//    {
//        #region Members

//        IEnumerable<ICalendarProperty> Properties { get; }

//        #endregion Members
//    }

//    public class SimpleCalendarPropertyCollection<ValueT> : IPropertyCollection<ValueT>
//        where ValueT : IPropertyValue, new()
//    {
//        #region Constructor

//        public SimpleCalendarPropertyCollection() { }
//        public SimpleCalendarPropertyCollection(IEnumerable<SimpleCalendarProperty<ValueT>> props) { Properties = props; }
        
//        #endregion Constructor

//        #region Members

//        public IEnumerable<ICalendarProperty<ValueT>> Properties { get; } = new List<SimpleCalendarProperty<ValueT>>();

//        #endregion Members
//    }

//    public class SimpleCalendarPropertyCollection : IPropertyCollection
//    {
//        #region Constructor

//        public SimpleCalendarPropertyCollection() { }
//        public SimpleCalendarPropertyCollection(IEnumerable<GenericCalendarProperty> props) { Properties = props; }

//        #endregion Constructor

//        #region Members

//        public IEnumerable<ICalendarProperty> Properties { get; } = new List<GenericCalendarProperty>();

//        #endregion Members
//    }
//}
