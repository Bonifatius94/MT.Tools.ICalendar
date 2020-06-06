using MT.Tools.ICalendar.DataObjects.Factory;
using MT.Tools.ICalendar.DataObjects.PropertyParameter;
using MT.Tools.ICalendar.DataObjects.PropertyValue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MT.Tools.ICalendar.DataObjects.ComponentProperty
{
    public class CustomComponentProperty : IComponentProperty
    {
        #region Constructor

        public CustomComponentProperty() { }

        public CustomComponentProperty(TextValue customText) { CustomText = customText; }

        public CustomComponentProperty(TextValue customText, IEnumerable<IPropertyParameter> parameters) { CustomText = customText; Parameters = parameters; }

        #endregion Constructor

        #region Members

        public string Markup { get; private set; }
        public ComponentPropertyType Type => ComponentPropertyType.Description;

        public IEnumerable<IPropertyParameter> Parameters { get; private set; } = new List<IPropertyParameter>();

        public TextValue CustomText { get; set; }
        public IPropertyValue Value => CustomText;

        #endregion Members

        #region Methods

        public void Deserialize(string content)
        {
            // deserialize markup
            Markup = content.Substring(0, Math.Min(content.IndexOf(';'), content.IndexOf(':')));

            // deserialize parameters
            Parameters =
                content.Substring(Markup.Length, content.IndexOf(':') - Markup.Length)
                .Split(';', StringSplitOptions.RemoveEmptyEntries)
                .Select(x => CalendarFactory.DeserializePropertyParameter(x))
                .ToList();

            // extract the value content
            string valueContent = content.Substring(content.IndexOf(':') + 1).Trim();
            CustomText = ObjectSerializer.Deserialize<TextValue>(valueContent);
        }

        public string Serialize()
        {
            string paramsContent = Parameters.Select(x => x.Serialize()).Aggregate((x, y) => x + ";" + y);
            return $"{ Markup }{ (string.IsNullOrEmpty(paramsContent) ? "" : ";" + paramsContent) }:{ CustomText.Serialize() }";
        }

        #endregion Methods
    }
}
