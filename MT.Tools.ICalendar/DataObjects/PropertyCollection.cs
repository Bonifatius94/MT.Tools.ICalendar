using MT.Tools.ICalendar.DataObjects.PropertyValue;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MT.Tools.ICalendar.DataObjects
{
    public class PropertyDictionary<ValueT> : Dictionary<string, ValueT>, IDictionary<string, ValueT>
        where ValueT : IPropertyValueImpl
    {
        #region Constructor

        public PropertyDictionary() : base() { }
        public PropertyDictionary(IDictionary<string, ValueT> dict) : base(dict) { }
        public PropertyDictionary(IEnumerable<KeyValuePair<string, ValueT>> pairs) : base(pairs) { }

        #endregion Constructor
    }

    public class PropertyDictionary : PropertyDictionary<IPropertyValueImpl>
    {
        #region Constructor

        public PropertyDictionary() : base() { }
        public PropertyDictionary(IDictionary<string, IPropertyValueImpl> dict) : base(dict) { }
        public PropertyDictionary(IEnumerable<KeyValuePair<string, IPropertyValueImpl>> pairs) : base(pairs) { }

        #endregion Constructor
    }

    public interface IPropertyCollection<ValueT>
        where ValueT : IPropertyValueImpl
    {
        #region Members

        PropertyDictionary<ValueT> Dict { get; }

        #endregion Members
    }

    public interface IPropertyCollection : IPropertyCollection<IPropertyValueImpl> { }

    public class SimplePropertyCollection<ValueT> : IPropertyCollection<ValueT>
        where ValueT : IPropertyValueImpl
    {
        #region Constructor

        public SimplePropertyCollection() { }
        public SimplePropertyCollection(IDictionary<string, ValueT> dict) { Dict = new PropertyDictionary<ValueT>(dict); }

        #endregion Constructor

        #region Members

        public PropertyDictionary<ValueT> Dict { get; } = new PropertyDictionary<ValueT>();

        #endregion Members
    }

    public class SimplePropertyCollection : SimplePropertyCollection<IPropertyValueImpl>
    {
        #region Constructor

        public SimplePropertyCollection() { }
        public SimplePropertyCollection(IDictionary<string, IPropertyValueImpl> dict) : base(dict) { }

        #endregion Constructor
    }
}
