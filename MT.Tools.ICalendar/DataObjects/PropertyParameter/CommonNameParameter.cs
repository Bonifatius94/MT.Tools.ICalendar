using MT.Tools.ICalendar.DataObjects.PropertyValue.Primitive;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MT.Tools.ICalendar.DataObjects.PropertyParameter
{
    public class CommonNameParameter : IPropertyParameter
    {
        #region Constructor

        public CommonNameParameter() { }

        public CommonNameParameter(string value)
        {

        }

        #endregion Constructor

        #region Members

        public PropertyParameterType Type => PropertyParameterType.CommonName;

        public TextValue Text { get; set; }

        #endregion Members

        #region Methods

        public void Deserialize(string content)
        {
            // remove heading and trailing white spaces
            content = content.Trim();

            // make sure that the parameter starts with ALTREP
            if (!content.StartsWith("CN=")) { throw new ArgumentException("Invalid common name content detected! Property parameter needs to start with CN keyword!"); }

            // double-quoted parameter value
            if (content.Contains('"'))
            {
                // make sure that the input contains exactly 2 double-quotes
                if (content.Where(x => x == '"').Count() != 2) { throw new ArgumentException($"Invalid content ({ content }) detected! The text must be surrounded by double quotes!"); }

                // determine the start and end indices of the text
                int textStart = content.IndexOf('\"') + 1;
                int textEnd = content.IndexOf('\"', textStart) - 1;

                // extract the text string from the content
                string textContent = content.Substring(textStart, textEnd - textStart);

                // create a new text value instance from it
                Text = ObjectSerializer.Deserialize<TextValue>(textContent);
            }
            // simple parameter value
            else
            {
                // extract the text string from the content
                string textContent = content.Substring(3, content.Length - 3);

                // create a new text value instance from it
                Text = ObjectSerializer.Deserialize<TextValue>(textContent);
            }
        }

        public string Serialize()
        {
            return $"CN=\"{ Text.Serialize() }\"";
        }

        #endregion Methods
    }
}
