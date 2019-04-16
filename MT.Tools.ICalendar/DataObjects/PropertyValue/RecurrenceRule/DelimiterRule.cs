using System;
using System.Collections.Generic;
using System.Text;

namespace MT.Tools.ICalendar.DataObjects.PropertyValue.RecurrenceRule
{
    public enum DelimiterRuleType
    {
        Until,
        Count
    }

    public class DelimiterRule : ISerializableObject
    {
        #region Constructor

        public DelimiterRule() { }

        public DelimiterRule(int count)
        {
            RepetitionsCount = count;
            Type = DelimiterRuleType.Count;
        }

        public DelimiterRule(DateTime endDate)
        {
            EndDate = endDate;
            Type = DelimiterRuleType.Until;
        }

        #endregion Constructor

        #region Members

        public int RepetitionsCount { get; private set; }
        public DateTime EndDate { get; private set; }
        public DelimiterRuleType Type { get; private set; }

        #endregion Members

        #region Methods

        public void Deserialize(string content)
        {
            // remove heading and trailing white spaces
            content = content.Trim();

            // get value as string
            int start = content.IndexOf('=') + 1;
            string valueAsString = content.Substring(start, content.Length - start).Trim();

            if (content.StartsWith("COUNT"))
            {
                RepetitionsCount = int.Parse(valueAsString);
                Type = DelimiterRuleType.Count;
            }
            else if (content.StartsWith("UNTIL"))
            {
                var temp = new SimplePropertyValue();
                temp.Deserialize(valueAsString);
                EndDate = (DateTime)temp.Value;
                Type = DelimiterRuleType.Until;
            }
            else
            {
                throw new ArgumentException($"Illegal delimiter rule content ({ content }) detected!");
            }
        }

        public string Serialize()
        {
            return (Type == DelimiterRuleType.Count) ? $"COUNT={ RepetitionsCount }" : $"UNTIL={ new SimplePropertyValue(EndDate).Serialize() }";
        }

        #endregion Methods
    }
}
