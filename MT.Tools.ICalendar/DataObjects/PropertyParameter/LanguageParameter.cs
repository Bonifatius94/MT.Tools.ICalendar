using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace MT.Tools.ICalendar.DataObjects.PropertyParameter
{
    public class LanguageParameter : IPropertyParameter
    {
        #region Constructor

        public LanguageParameter() { }

        public LanguageParameter(CultureInfo language) { Language = language; }

        #endregion Constructor

        #region Members

        public PropertyParameterType Type => PropertyParameterType.Language;

        // TODO: make sure that CultureInfo supports all language formats
        public CultureInfo Language { get; set; }

        #endregion Members

        #region Methods

        public void Deserialize(string content)
        {
            // remove heading and trailing white spaces
            content = content.Trim();

            // make sure that the parameter starts with LANGUAGE
            if (!content.StartsWith("LANGUAGE=")) { throw new ArgumentException($"Invalid language parameter detected! Property parameter needs to start with LANGUAGE keyword!"); }

            // extract the encoding type from content
            string languageString = content.Substring(content.IndexOf('=') + 1).Trim();
            Language = CultureInfo.CreateSpecificCulture(languageString);
        }

        public string Serialize()
        {
            // TODO: make sure that CultureInfo.Name returns the desired data format
            return $"LANGUAGE={ Language.Name }";
        }

        #endregion Methods
    }
}
