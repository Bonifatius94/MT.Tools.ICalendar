using MT.Tools.ICalendar.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MT.Tools.ICalendar.DataObjects.PropertyValue.RecurrenceRule
{
    public enum RecurrenceFrequency
    {
        Secondly,
        Minutely,
        Hourly,
        Daily,
        Weekly,
        Monthly,
        Yearly
    }

    public class RecurrenceRuleValue : IPropertyValueImpl
    {
        #region Constructor

        public RecurrenceRuleValue() { }

        public RecurrenceRuleValue(RecurrenceFrequency frequency)
        {
            // TODO: add missing parameters
        }

        #endregion Constructor

        #region Members

        public RecurrenceFrequency Frequency { get; private set; }
        public DelimiterRule Delimiter { get; private set; } = null;

        #endregion Members

        #region Methods

        // individual parts must only accur once
        // frequency part has to be specified once and is always the first parameter (when serializing), but should be parsed correctly at another position (when deserializing)
        // 

        // TODO: implement missing parser logic

        public void Deserialize(string content)
        {
            // 
            var ruleParts = content.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim());
            var byListRuleParts = ruleParts.Where(x => x.StartsWith("BY"));
            var otherParts = ruleParts.Except(byListRuleParts);

            // make sure the rule parts are valid, otherwise throw argument exception
            validateRuleParts(ruleParts);

            foreach (var part in otherParts)
            {
                int start = content.IndexOf('=') + 1;
                string valueAsString = content.Substring(start, content.Length - start).Trim();

                if (part.StartsWith("FREQ"))
                {
                    Frequency = deserializeRecurrenceFrequency(valueAsString);
                }
                else if (part.StartsWith("COUNT") || content.StartsWith("UNTIL"))
                {
                    Delimiter = new DelimiterRule();
                    Delimiter.Deserialize(part);
                }
                else if (part.StartsWith("INTERVAL"))
                {

                }
                else if (part.StartsWith("WKST"))
                {

                }
                else
                {
                    throw new ArgumentException($"Invalid recurrence rule part ({ part }) detected!");
                }
            }
        }

        private void validateRuleParts(IEnumerable<string> ruleParts)
        {
            // make sure there is exactly one frequency tag
            if (!ruleParts.ExactlyOne(x => x.StartsWith("FREQ"))) { throw new ArgumentException("Frequency tag is missing or specified multiple times!"); }

            // make sure that there are no UNTIL + COUNT tags at once
            if (ruleParts.Where(x => x.StartsWith("COUNT") || x.StartsWith("UNTIL")).Take(2).Count() > 1) { throw new ArgumentException("Too many count / until tags detected!"); }
        }

        public string Serialize()
        {
            throw new NotImplementedException();
        }

        #region Helpers

        #region RecurrenceFrequency

        private string serializeRecurrenceFrequency(RecurrenceFrequency frequency)
        {
            string content;

            switch (frequency)
            {
                case RecurrenceFrequency.Secondly: content = "SECONDLY"; break;
                case RecurrenceFrequency.Minutely: content = "MINUTELY"; break;
                case RecurrenceFrequency.Hourly:   content = "HOURLY";   break;
                case RecurrenceFrequency.Daily:    content = "DAILY";    break;
                case RecurrenceFrequency.Weekly:   content = "WEEKLY";   break;
                case RecurrenceFrequency.Monthly:  content = "MONTHLY";  break;
                case RecurrenceFrequency.Yearly:   content = "YEARLY";   break;
                default: throw new ArgumentException($"Unsupported recurrence frequency type ({ frequency }) detected!");
            }

            return content;
        }

        private RecurrenceFrequency deserializeRecurrenceFrequency(string content)
        {
            RecurrenceFrequency frequency;

            switch (content)
            {
                case "SECONDLY": frequency = RecurrenceFrequency.Secondly; break;
                case "MINUTELY": frequency = RecurrenceFrequency.Minutely; break;
                case "HOURLY":   frequency = RecurrenceFrequency.Hourly;   break;
                case "DAILY":    frequency = RecurrenceFrequency.Daily;    break;
                case "WEEKLY":   frequency = RecurrenceFrequency.Weekly;   break;
                case "MONTHLY":  frequency = RecurrenceFrequency.Monthly;  break;
                case "YEARLY":   frequency = RecurrenceFrequency.Yearly;   break;
                default: throw new ArgumentException($"Invalid recurrence frequency type ({ content }) detected!");
            }

            return frequency;
        }

        #endregion RecurrenceFrequency

        #endregion Helpers

        #endregion Methods

        //recur = recur-rule-part* ( ";" recur-rule-part )
        //;
        //; The rule parts are not ordered in any
        //; particular sequence.
        //;
        //; The FREQ rule part is REQUIRED,
        //; but MUST NOT occur more than once.
        //;
        //; The UNTIL or COUNT rule parts are OPTIONAL,
        //; but they MUST NOT occur in the same ’recur’.
        //;
        //; The other rule parts are OPTIONAL,
        //; but MUST NOT occur more than once.

        //recur-rule-part = ( "FREQ" "=" freq )
        //                / ( "UNTIL" "=" enddate )
        //                / ( "COUNT" "=" 1*DIGIT )
        //                / ( "INTERVAL" "=" 1*DIGIT )
        //                / ( "BYSECOND" "=" byseclist )
        //                / ( "BYMINUTE" "=" byminlist )
        //                / ( "BYHOUR" "=" byhrlist )
        //                / ( "BYDAY" "=" bywdaylist )
        //                / ( "BYMONTHDAY" "=" bymodaylist )
        //                / ( "BYYEARDAY" "=" byyrdaylist )
        //                / ( "BYWEEKNO" "=" bywknolist )
        //                / ( "BYMONTH" "=" bymolist )
        //                / ( "BYSETPOS" "=" bysplist )
        //                / ( "WKST" "=" weekday )

        //freq = "SECONDLY" / "MINUTELY" / "HOURLY" / "DAILY" / "WEEKLY" / "MONTHLY" / "YEARLY"
        //enddate = date / date-time
        //byseclist = (seconds * ("," seconds) )
        //seconds = 1*2DIGIT ;0 to 60
        //byminlist = (minutes*("," minutes) )
        //minutes = 1*2DIGIT ;0 to 59
        //byhrlist = (hour*("," hour) )
        //hour = 1*2DIGIT ;0 to 23
        //bywdaylist = (weekdaynum*("," weekdaynum) )
        //weekdaynum = [[plus / minus] ordwk] weekday
        //plus = "+"
        //minus = "-"
        //ordwk = 1*2DIGIT ;1 to 53
        //weekday = "SU" / "MO" / "TU" / "WE" / "TH" / "FR" / "SA"
        //;Corresponding to SUNDAY, MONDAY, TUESDAY, WEDNESDAY, THURSDAY,
        //;FRIDAY, and SATURDAY days of the week.

        //bymodaylist = (monthdaynum * ("," monthdaynum) )
        //monthdaynum = [plus / minus] ordmoday
        //ordmoday = 1 * 2DIGIT ;1 to 31
        //byyrdaylist = (yeardaynum*("," yeardaynum) )
        //yeardaynum = [plus / minus] ordyrday
        //ordyrday = 1 * 3DIGIT ;1 to 366
        //bywknolist = (weeknum*("," weeknum) )
        //weeknum = [plus / minus] ordwk
        //bymolist = (monthnum * ("," monthnum) )
        //monthnum = 1*2DIGIT ;1 to 12
        //bysplist = (setposday*("," setposday) )
        //setposday = yeardaynum
    }
}
