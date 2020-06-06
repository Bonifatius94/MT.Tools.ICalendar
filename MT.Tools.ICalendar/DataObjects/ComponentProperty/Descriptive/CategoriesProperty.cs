using MT.Tools.ICalendar.DataObjects.Factory;
using MT.Tools.ICalendar.DataObjects.PropertyParameter;
using MT.Tools.ICalendar.DataObjects.PropertyValue;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MT.Tools.ICalendar.DataObjects.ComponentProperty
{
    public class CategoriesProperty : IComponentProperty
    {
        #region Constructor

        public CategoriesProperty() { }

        public CategoriesProperty(TextListValue categories) { Categories = categories; }

        public CategoriesProperty(TextListValue categories, IEnumerable<IPropertyParameter> parameters) { Categories = categories; Parameters = parameters; }

        #endregion Constructor

        #region Members

        public string Markup => "CATEGORIES";
        public ComponentPropertyType Type => ComponentPropertyType.Categories;

        public IEnumerable<IPropertyParameter> Parameters { get; private set; } = new List<IPropertyParameter>();

        public LanguageParameter Language => Parameters.Where(x => x.GetType() == typeof(LanguageParameter)).FirstOrDefault() as LanguageParameter;

        public TextListValue Categories { get; private set; } = new TextListValue();
        public IPropertyValue Value => Categories;

        #endregion Members

        #region Methods

        public void Deserialize(string content)
        {
            // make sure that the parameter starts with CATEGORIES
            if (!content.ToUpper().StartsWith(Markup)) { throw new ArgumentException($"Invalid categories detected! Component property needs to start with { Markup } keyword!"); }

            // deserialize parameters
            Parameters =
                content.Substring(Markup.Length, content.IndexOf(':') - Markup.Length)
                .Split(';', StringSplitOptions.RemoveEmptyEntries)
                .Select(x => CalendarFactory.DeserializePropertyParameter(x))
                .ToList();

            // extract the value content
            string valueContent = content.Substring(content.IndexOf(':') + 1).Trim();
            Categories = ObjectSerializer.Deserialize<TextListValue>(valueContent);

            // make sure that the language parameter only occurs once
            if (Parameters.Where(x => x.GetType() == typeof(LanguageParameter)).Count() > 1) { throw new ArgumentException("Invalid parameter detected! Language parameter needs to be unique!"); }
        }

        public string Serialize()
        {
            string paramsContent = Parameters.Select(x => x.Serialize()).Aggregate((x, y) => x + ";" + y);
            return $"{ Markup }{ (string.IsNullOrEmpty(paramsContent) ? "" : ";" + paramsContent) }:{ Categories.Serialize() }";
        }

        #endregion Methods
    }
}
