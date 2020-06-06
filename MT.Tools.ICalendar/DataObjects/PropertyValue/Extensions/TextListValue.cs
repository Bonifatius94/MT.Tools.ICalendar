using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MT.Tools.ICalendar.DataObjects.PropertyValue
{
    public class TextListValue : IPropertyValue
    {
        #region Constructor

        public TextListValue() { }

        public TextListValue(IEnumerable<TextValue> texts) { Texts = texts; }

        #endregion Constructor

        public PropertyValueType Type => PropertyValueType.Text;

        public IEnumerable<TextValue> Texts { get; set; } = new List<TextValue>();

        #region Methods

        public void Deserialize(string content)
        {
            // deserialize content
            Texts = content.Trim().Split(',', StringSplitOptions.RemoveEmptyEntries).Select(x => new TextValue(x.Trim())).ToList();

            // make sure that there is at least one element on the list
            if (Texts?.Count() < 1) { throw new ArgumentException("Invalid texts list value detected! List needs to contain at least one element!"); }
        }

        public string Serialize()
        {
            return $"{ Texts.Select(x => x.Value).Aggregate((x, y) => x + "," + y) }";
        }

        #endregion Methods
    }
}
