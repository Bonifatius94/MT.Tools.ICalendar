using System;
using System.Collections.Generic;
using System.Text;

namespace MT.Tools.ICalendar.DataObjects.PropertyValue.Time
{
    public class DateValue : IPropertyValueImpl
    {
        #region Constructor

        public DateValue() { }

        public DateValue(DateTime date)
        {
            Date = date;
        }

        #endregion Constructor

        #region Members

        public DateTime Date { get; private set; }

        #endregion Members

        #region Methods

        public void Deserialize(string content)
        {
            // remove heading and trailing white spaces
            content = content.Trim();

            // make sure that the length is correct
            if (content.Length != 8) { throw new ArgumentException($"Date content with invalid length detected. ({ content })"); }

            // get year, month and day
            int year = int.Parse(content.Substring(0, 4));
            int month = int.Parse(content.Substring(4, 2));
            int day = int.Parse(content.Substring(6, 2));

            // create new date instance with the given year, month and day
            Date = new DateTime(year, month, day);
        }

        public string Serialize()
        {
            return $"{ Date.Year }{ Date.Month }{ Date.Day }";
        }

        #endregion Methods
    }
}
