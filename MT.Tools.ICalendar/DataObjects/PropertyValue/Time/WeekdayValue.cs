using System;
using System.Collections.Generic;
using System.Text;

namespace MT.Tools.ICalendar.DataObjects.PropertyValue.Time
{
    public class WeekdayValue : IPropertyValueImpl
    {
        #region Constructor

        public WeekdayValue() { }

        public WeekdayValue(DayOfWeek day)
        {
            Day = day;
        }

        #endregion Constructor

        #region Members

        public DayOfWeek Day { get; set; }

        public PropertyValueType Type => PropertyValueType.Weekday;

        #endregion Members

        #region Methods

        public void Deserialize(string content)
        {
            content = content.Trim();

            DayOfWeek day;

            switch (content)
            {
                case "SU": day = DayOfWeek.Sunday;    break;
                case "MO": day = DayOfWeek.Monday;    break;
                case "TU": day = DayOfWeek.Tuesday;   break;
                case "WE": day = DayOfWeek.Wednesday; break;
                case "TH": day = DayOfWeek.Thursday;  break;
                case "FR": day = DayOfWeek.Friday;    break;
                case "SA": day = DayOfWeek.Saturday;  break;
                default: throw new ArgumentException($"Invalid day of week content ({ content }) detected!");
            }

            Day = day;
        }

        public string Serialize()
        {
            return Day.ToString().Substring(0, 2).ToUpper();
        }

        #endregion Methods
    }
}
