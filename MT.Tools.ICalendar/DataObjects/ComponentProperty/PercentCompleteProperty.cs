using MT.Tools.ICalendar.DataObjects.Factory;
using MT.Tools.ICalendar.DataObjects.PropertyParameter;
using MT.Tools.ICalendar.DataObjects.PropertyValue.Primitive;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MT.Tools.ICalendar.DataObjects.ComponentProperty
{
    public class PercentCompleteProperty : IComponentProperty
    {
        #region Constructor

        public PercentCompleteProperty() { }

        public PercentCompleteProperty(IntegerValue percentage) { Percentage = percentage; }

        public PercentCompleteProperty(IntegerValue percentage, IEnumerable<IPropertyParameter> parameters) { Percentage = percentage; Parameters = parameters; }

        #endregion Constructor

        #region Members

        public ComponentPropertyType Type => ComponentPropertyType.PercentComplete;

        public IEnumerable<IPropertyParameter> Parameters { get; private set; } = new List<IPropertyParameter>();

        public IntegerValue Percentage { get; set; }

        #endregion Members

        #region Methods

        public void Deserialize(string content)
        {
            // make sure that the parameter starts with CATEGORIES
            if (!content.ToUpper().StartsWith("PERCENT-COMPLETE")) { throw new ArgumentException("Invalid percent complete detected! Component property needs to start with PERCENT-COMPLETE keyword!"); }

            // deserialize parameters
            Parameters =
                content.Substring("PERCENT-COMPLETE".Length, content.IndexOf(':') - "PERCENT-COMPLETE".Length)
                .Split(';', StringSplitOptions.RemoveEmptyEntries)
                .Select(x => CalendarFactory.DeserializePropertyParameter(x))
                .ToList();

            // extract the value content
            string valueContent = content.Substring(content.IndexOf(':')).Trim();
            Percentage = ObjectSerializer.Deserialize<IntegerValue>(valueContent);
        }

        public string Serialize()
        {
            string paramsContent = Parameters.Select(x => x.Serialize()).Aggregate((x, y) => x + ";" + y);
            return $"PERCENT-COMPLETE{ (string.IsNullOrEmpty(paramsContent) ? "" : ";" + paramsContent) }:{ Percentage.Serialize() }";
        }

        #endregion Methods
    }
}
