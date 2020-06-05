using MT.Tools.ICalendar.DataObjects.Factory;
using MT.Tools.ICalendar.DataObjects.PropertyParameter;
using MT.Tools.ICalendar.DataObjects.PropertyValue.Primitive;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MT.Tools.ICalendar.DataObjects.ComponentProperty
{
    public class PriorityProperty : IComponentProperty
    {
        #region Constructor

        public PriorityProperty() { }

        public PriorityProperty(IntegerValue priority) { Priority = priority; }

        public PriorityProperty(IntegerValue priority, IEnumerable<IPropertyParameter> parameters) { Priority = priority; Parameters = parameters; }

        #endregion Constructor

        #region Members

        public ComponentPropertyType Type => ComponentPropertyType.Priority;

        public IEnumerable<IPropertyParameter> Parameters { get; private set; } = new List<IPropertyParameter>();

        public IntegerValue Priority { get; set; } = new IntegerValue(0);

        #endregion Members

        #region Methods

        public void Deserialize(string content)
        {
            // make sure that the parameter starts with CATEGORIES
            if (!content.ToUpper().StartsWith("PRIORITY")) { throw new ArgumentException("Invalid priority detected! Component property needs to start with PRIORITY keyword!"); }

            // deserialize parameters
            Parameters =
                content.Substring("PRIORITY".Length, content.IndexOf(':') - "PRIORITY".Length)
                .Split(';', StringSplitOptions.RemoveEmptyEntries)
                .Select(x => CalendarFactory.DeserializePropertyParameter(x))
                .ToList();

            // extract the value content
            string valueContent = content.Substring(content.IndexOf(':')).Trim();
            Priority = ObjectSerializer.Deserialize<IntegerValue>(valueContent);

            if (Priority.Value > 9) { throw new ArgumentException("Invalid property value detected! Priority needs to be between 0 and 9 (both included)!"); }
        }

        public string Serialize()
        {
            string paramsContent = Parameters.Select(x => x.Serialize()).Aggregate((x, y) => x + ";" + y);
            return $"PRIORITY{ (string.IsNullOrEmpty(paramsContent) ? "" : ";" + paramsContent) }:{ Priority.Serialize() }";
        }

        #endregion Methods
    }
}
