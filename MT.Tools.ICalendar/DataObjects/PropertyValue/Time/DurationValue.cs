using System;
using System.Collections.Generic;
using System.Text;

namespace MT.Tools.ICalendar.DataObjects.PropertyValue.Time
{
    public class DurationValue : IPropertyValue
    {
        #region Constructor

        public DurationValue() { }

        public DurationValue(TimeSpan duration)
        {
            Duration = duration;
        }

        #endregion Constructor

        #region Members

        public TimeSpan Duration { get; private set; }

        public PropertyValueType Type => PropertyValueType.Duration;

        #endregion Members

        #region Methods

        public void Deserialize(string content)
        {
            // remove heading and trailing white spaces
            content = content.Trim();
            
            // remove '+' sign
            content = content.Replace("+", "");

            // check if '-' is set
            bool isNegative = content.Contains('-');
            content = content.Replace("-", "");
            content = content.Replace("P", "");

            TimeSpan newDuration;

            if (content.Contains('W'))
            {
                string contentAsString = content.Substring(0, content.IndexOf('W'));
                int weeks = int.Parse(contentAsString);
                newDuration = new TimeSpan(7 * weeks, 0, 0, 0);
            }
            else
            {
                int days = 0;
                int hours = 0;
                int minutes = 0;
                int seconds = 0;

                if (content.Contains('D'))
                {
                    string contentAsString = content.Substring(0, content.IndexOf('D'));
                    days = int.Parse(contentAsString);
                    content = content.Substring(content.IndexOf('D') + 1, content.Length - contentAsString.Length - 1);
                }

                if (content.Contains('H'))
                {
                    string contentAsString = content.Substring(0, content.IndexOf('H'));
                    hours = int.Parse(contentAsString);
                    content = content.Substring(content.IndexOf('H') + 1, content.Length - contentAsString.Length - 1);
                }

                if (content.Contains('M'))
                {
                    string contentAsString = content.Substring(0, content.IndexOf('M'));
                    minutes = int.Parse(contentAsString);
                    content = content.Substring(content.IndexOf('M') + 1, content.Length - contentAsString.Length - 1);
                }

                if (content.Contains('S'))
                {
                    string contentAsString = content.Substring(0, content.IndexOf('S'));
                    seconds = int.Parse(contentAsString);
                }

                newDuration = new TimeSpan(days, hours, minutes, seconds);
            }

            newDuration = isNegative ? newDuration.Negate() : newDuration;
            Duration = newDuration;
        }

        public string Serialize()
        {
            bool isNegative = Duration < TimeSpan.Zero;
            string content = $"{ (isNegative ? "-" : "") }P";
            
            if (Duration.Days % 7 == 0 && Duration.Hours == 0 && Duration.Minutes == 0 && Duration.Seconds == 0)
            {
                content += $"{ Duration.Days / 7 }W";
            }
            else
            {
                content += $"{ Duration.TotalDays }D{ Duration.Hours }H{ Duration.Minutes }M{ Duration.Seconds }S";
            }

            return content;
        }

        #endregion Methods
    }
}
