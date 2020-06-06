using MT.Tools.ICalendar.DataObjects.PropertyValue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MT.Tools.ICalendar.DataObjects.PropertyValue
{
    // rule types are defined to determine expand / limit by numeric comparison to frequency (limit: freq <= rule type, expand: freq > rule type)
    public enum ByListRuleType
    {
        BySecond = 0,
        ByMinute = 1,
        ByHour = 2,
        ByDay = 3,
        ByMonthday = 4,
        ByYearday = 5,
        ByWeekNo = 6,
        ByMonth = 8,
        BySetPos = 10
    }

    public interface IByListRule : ISerializableObject
    {
        #region Members

        ByListRuleType RuleType { get; set; }

        #endregion Members
    }

    public interface IByListRule<ListT> : IByListRule
    {
        #region Members

        IEnumerable<ListT> List { get; set; }

        #endregion Members
    }

    public class ByStandardListRule : IByListRule<int>
    {
        #region Constructor

        public ByStandardListRule() { }

        public ByStandardListRule(ByListRuleType type, IEnumerable<int> list)
        {
            // make sure the type is correct
            if (type == ByListRuleType.ByDay) { throw new ArgumentException("Standard rule must not be of type ByDay!"); }
        }

        #endregion Constructor

        #region Members

        public ByListRuleType RuleType { get; set; }
        public IEnumerable<int> List { get; set; } = new int[0];

        #endregion Members

        #region Methods

        public void Deserialize(string content)
        {
            // split content at '=' character
            var parts = content.Split('=', StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).ToArray();

            // parse rule type
            var ruleType = ObjectSerializer.Deserialize<EnumValue<ByListRuleType>>(parts[0]).Value;

            // parse list
            var listParts = parts[1].Split(',', StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).ToArray();
            List = listParts.Select(x => int.Parse(x.Replace("+", ""))).ToList();
        }

        public string Serialize()
        {
            string rule = new EnumValue<ByListRuleType>(RuleType).Serialize();
            string list = List.Select(x => x.ToString()).Aggregate((x, y) => x + "," + y);

            return $"{ rule }={ list }";
        }

        #endregion Methods
    }

    public class ByDayListRule : IByListRule<Tuple<DayOfWeek, int>>
    {
        #region Constructor

        public ByDayListRule() { }

        public ByDayListRule(ByListRuleType type, IEnumerable<Tuple<DayOfWeek, int>> list)
        {
            // make sure the type is correct
            if (type != ByListRuleType.ByDay) { throw new ArgumentException("Standard rule must not be of type ByDay!"); }
        }

        #endregion Constructor

        #region Members

        public ByListRuleType RuleType { get; set; }
        public IEnumerable<Tuple<DayOfWeek, int>> List { get; set; } = new Tuple<DayOfWeek, int>[0];

        #endregion Members

        #region Methods

        public void Deserialize(string content)
        {
            // split content at '=' character
            var parts = content.Split('=', StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).ToArray();

            // parse rule type
            var ruleType = ObjectSerializer.Deserialize<EnumValue<ByListRuleType>>(parts[0]).Value;

            // parse list
            var listParts = parts[1].Split(',', StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).ToArray();
            List = listParts.Select(x => parseDayListEntry(x.Replace("+", ""))).ToList();
        }

        private Tuple<DayOfWeek, int> parseDayListEntry(string content)
        {
            // parse weekday
            string weekdayAsString = content.Substring(content.Length - 3, 2);
            var weekday = ObjectSerializer.Deserialize<WeekdayValue>(weekdayAsString).Day;

            // parse week modificator
            int weekOfYear = (content.Length > 2) ? int.Parse(content.Substring(0, content.Length - 2)) : 0;

            // return the tuple of the parsed values
            return new Tuple<DayOfWeek, int>(weekday, weekOfYear);
        }

        public string Serialize()
        {
            string rule = new EnumValue<ByListRuleType>(RuleType).Serialize();
            string list = List.Select(x => $"{ ((x.Item2 != 0) ? x.Item2.ToString() : "") }{ new WeekdayValue(x.Item1).Serialize() }").Aggregate((x, y) => x + "," + y);

            return $"{ rule }={ list }";
        }

        #endregion Methods
    }
}
