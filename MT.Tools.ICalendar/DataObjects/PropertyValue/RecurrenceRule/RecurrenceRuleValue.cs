using MT.Tools.ICalendar.DataObjects.PropertyValue.Primitive;
using MT.Tools.ICalendar.DataObjects.PropertyValue.Time;
using MT.Tools.ICalendar.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MT.Tools.ICalendar.DataObjects.PropertyValue.RecurrenceRule
{
    // frequency is defined to determine expand / limit by numeric comparison to rule type (limit: freq <= rule type, expand: freq > rule type)
    public enum RecurrenceFrequency
    {
        Secondly = 0,
        Minutely = 1,
        Hourly = 2,
        Daily = 5,
        Weekly = 7,
        Monthly = 8,
        Yearly = 9
    }

    public class RecurrenceRuleValue : IPropertyValueImpl
    {
        #region Constructor

        public RecurrenceRuleValue() { }

        public RecurrenceRuleValue(RecurrenceFrequency frequency, IEnumerable<IByListRule> listRules, 
            int interval = 1, DelimiterRule delimiter = null, DayOfWeek weekStart = DayOfWeek.Monday)
        {
            Frequency = frequency;
            ListRules = listRules.ToDictionary(x => x.RuleType);
            Interval = interval;
            Delimiter = delimiter;
            WeekStart = weekStart;
        }

        // TODO: add useful constructors

        #endregion Constructor

        #region Members

        public RecurrenceFrequency Frequency { get; set; }
        public int Interval { get; set; }
        public DelimiterRule Delimiter { get; set; } = null;
        public DayOfWeek WeekStart { get; set; } = DayOfWeek.Monday;
        public IDictionary<ByListRuleType, IByListRule> ListRules { get; } = new Dictionary<ByListRuleType, IByListRule>();

        // computed read-only properties
        public IByListRule BySecond { get { return ListRules.GetValueOrDefault(ByListRuleType.BySecond); } }
        public IByListRule ByMinute { get { return ListRules.GetValueOrDefault(ByListRuleType.ByMinute); } }
        public IByListRule ByHour { get { return ListRules.GetValueOrDefault(ByListRuleType.ByHour); } }
        public IByListRule ByDay { get { return ListRules.GetValueOrDefault(ByListRuleType.ByDay); } }
        public IByListRule ByMonthday { get { return ListRules.GetValueOrDefault(ByListRuleType.ByMonthday); } }
        public IByListRule ByYearday { get { return ListRules.GetValueOrDefault(ByListRuleType.ByYearday); } }
        public IByListRule ByWeekNo { get { return ListRules.GetValueOrDefault(ByListRuleType.ByWeekNo); } }
        public IByListRule ByMonth { get { return ListRules.GetValueOrDefault(ByListRuleType.ByMonth); } }
        public IByListRule BySetPos { get { return ListRules.GetValueOrDefault(ByListRuleType.BySetPos); } }

        #endregion Members

        #region Methods

        #region Occurance

        public IEnumerable<DateTime> GetOccurances(DateTime start, DateTime end)
        {
            // TODO: implement logic
            throw new NotImplementedException();
        }

        #endregion Occurance

        public void Deserialize(string content)
        {
            // retrieve the rule parts
            var rules = content.Split(';', StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim());

            // make sure the rule parts are valid, otherwise throw argument exception
            validateRules(rules);

            // parse rule parts
            foreach (string rule in rules)
            {
                parseRule(rule);
            }
        }

        private void validateRules(IEnumerable<string> ruleParts)
        {
            // make sure there is exactly one frequency tag
            if (!ruleParts.ExactlyOne(x => x.StartsWith("FREQ"))) { throw new ArgumentException("Frequency tag is missing or specified multiple times!"); }

            // make sure that there are no UNTIL + COUNT tags at once
            if (ruleParts.Where(x => x.StartsWith("COUNT") || x.StartsWith("UNTIL")).Take(2).Count() > 1) { throw new ArgumentException("Too many count / until tags detected!"); }
        }

        private void parseRule(string rule)
        {
            if (rule.StartsWith("BY"))
            {
                // parse ByList rule
                var bylistRule = rule.StartsWith("BYDAY") ? (IByListRule)ObjectSerializer.Deserialize<ByDayListRule>(rule) : ObjectSerializer.Deserialize<ByStandardListRule>(rule);
                ListRules[bylistRule.RuleType] = bylistRule;
            }
            else if (rule.StartsWith("COUNT") || rule.StartsWith("UNTIL"))
            {
                // parse delimiter rule
                Delimiter = ObjectSerializer.Deserialize<DelimiterRule>(rule);
            }
            else
            {
                string valueAsString = getPropertyValue(rule);

                if (rule.StartsWith("FREQ"))
                {
                    // parse frequency
                    Frequency = RecurrenceFrequencyHelper.DeserializeRecurrenceFrequency(valueAsString);
                }
                else if (rule.StartsWith("INTERVAL"))
                {
                    // parse interval
                    Interval = ObjectSerializer.Deserialize<IntegerValue>(valueAsString).Value;
                }
                else if (rule.StartsWith("WKST"))
                {
                    // parse week start
                    WeekStart = ObjectSerializer.Deserialize<WeekdayValue>(valueAsString).Day;
                }
                else
                {
                    // TODO: think about soft error handling (e.g. error output in console)
                    throw new ArgumentException($"Invalid recurrence rule part ({ rule }) detected!");
                }
            }
        }

        public string Serialize()
        {
            return new List<string>() {

                // serialize frequency
                $"FREQ={ RecurrenceFrequencyHelper.SerializeRecurrenceFrequency(Frequency) }",

                // serialize delimiter rule (if existing)
                (Delimiter != null) ? $"{ Delimiter.Serialize() }" : "",

                // serialize interval (if not default)
                (Interval > 1) ? $"INTERVAL={ Interval.ToString() }" : "",

                // serialize week start (if not default)
                (WeekStart != DayOfWeek.Monday) ? $"WKST={ new WeekdayValue(WeekStart).Serialize() }" : ""

            }.Where(x => !string.IsNullOrEmpty(x))

            // serialize ByList rules
            .Union(ListRules.Select(x => x.Value.Serialize()))

            // put all rules together
            .Aggregate((x, y) => $"{ x };{ y }");
        }

        #region Helpers

        private string getPropertyValue(string content)
        {
            int start = content.IndexOf('=') + 1;
            return content.Substring(start, content.Length - start).Trim();
        }

        #endregion Helpers

        #endregion Methods
    }

    public static class RecurrenceFrequencyHelper
    {
        #region Methods

        public static string SerializeRecurrenceFrequency(RecurrenceFrequency frequency)
        {
            return frequency.ToString().ToUpper();
        }

        public static RecurrenceFrequency DeserializeRecurrenceFrequency(string content)
        {
            return Enum.Parse<RecurrenceFrequency>(content, true);
        }

        #endregion Methods
    }
}
