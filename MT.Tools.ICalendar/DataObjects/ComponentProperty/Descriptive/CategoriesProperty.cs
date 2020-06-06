using MT.Tools.ICalendar.DataObjects.ComponentProperty;
using MT.Tools.ICalendar.DataObjects.Factory;
using MT.Tools.ICalendar.DataObjects.PropertyParameter;
using MT.Tools.ICalendar.DataObjects.PropertyValue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;

namespace MT.Tools.ICalendar.DataObjects.ComponentProperty
{
    public class CategoriesProperty : IComponentProperty
    {
        #region Constructor

        public CategoriesProperty() { }

        public CategoriesProperty(IEnumerable<TextValue> categories) { Categories = categories; }

        public CategoriesProperty(IEnumerable<TextValue> categories, IEnumerable<IPropertyParameter> parameters) { Categories = categories; Parameters = parameters; }

        #endregion Constructor

        #region Members

        public ComponentPropertyType Type => ComponentPropertyType.Categories;

        public IEnumerable<IPropertyParameter> Parameters { get; private set; } = new List<IPropertyParameter>();

        public LanguageParameter Language => Parameters.Where(x => x.GetType() == typeof(LanguageParameter)).FirstOrDefault() as LanguageParameter;

        public IEnumerable<TextValue> Categories { get; private set; } = new List<TextValue>();

        #endregion Members

        #region Methods

        public void Deserialize(string content)
        {
            // make sure that the parameter starts with CATEGORIES
            if (!content.ToUpper().StartsWith("CATEGORIES")) { throw new ArgumentException("Invalid categories detected! Component property needs to start with CATEGORIES keyword!"); }

            // deserialize parameters
            Parameters =
                content.Substring("CATEGORIES".Length, content.IndexOf(':') - "CATEGORIES".Length)
                .Split(';', StringSplitOptions.RemoveEmptyEntries)
                .Select(x => CalendarFactory.DeserializePropertyParameter(x))
                .ToList();

            // extract the value content
            string valueContent = content.Substring(content.IndexOf(':')).Trim();
            Categories = content.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(x => ObjectSerializer.Deserialize<TextValue>(x)).ToList();

            // make sure that the language parameter only occurs once
            if (Parameters.Where(x => x.GetType() == typeof(LanguageParameter)).Count() > 1) { throw new ArgumentException("Invalid parameter detected! Language parameter needs to be unique!"); }
        }

        public string Serialize()
        {
            string paramsContent = Parameters.Select(x => x.Serialize()).Aggregate((x, y) => x + ";" + y);
            string valuesContent = Categories.Select(x => x.Value).Aggregate((x, y) => x + "," + y);
            return $"CATEGORIES{ (string.IsNullOrEmpty(paramsContent) ? "" : ";" + paramsContent) }:{ valuesContent }";
        }

        #endregion Methods
    }
}
