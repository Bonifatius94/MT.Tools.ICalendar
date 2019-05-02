using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MT.Tools.ICalendar.DataObjects
{
    public class CalendarFile : ISerializableObject
    {
        #region Constructor

        public CalendarFile() { }

        public CalendarFile(IEnumerable<CalendarObject> objects)
        {
            Objects = objects;
        }

        #endregion Constructor

        #region Members

        public IEnumerable<CalendarObject> Objects { get; set; } = new List<CalendarObject>();

        #endregion Members

        #region Methods

        public void Deserialize(string content)
        {
            // remove heading and trailing white spaces
            content = content.Trim();

            // retrieve content lines
            var contentLines = content.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).ToList();

            // make sure that the first and last line are correct
            bool isFirstAndLastLineCorrect = contentLines.First().ToUpper().Equals("BEGIN:VCALENDAR") && contentLines.Last().ToUpper().Equals("END:VCALENDAR");
            if (!isFirstAndLastLineCorrect) { throw new ArgumentException("Invalid iCalendar markups at start / end of the iCalendar file!"); }

            // TODO: implement multi-threading for parsing the iCalendar objects parallel

            do
            {
                // find iCalendar object markups
                var objectLines = contentLines/*.SkipWhile(x => !x.StartsWith("BEGIN:VCALENDAR"))*/.TakeWhile(x => !x.StartsWith("END:VCALENDAR"));
                string objectContent = objectLines.Aggregate((x, y) => $"{ x }\r\n{ y }");

                // parse the iCalendar object from the extracted content
                var calObject = ObjectSerializer.Deserialize<CalendarObject>(objectContent);
                Objects.Append(calObject);

                // remove already parsed lines
                contentLines = contentLines.GetRange(objectLines.Count(), contentLines.Count - objectLines.Count());
            }
            // BEGIN tag => begin of next iCalendar component, END tag => end of iCalendar object
            while (contentLines.First().StartsWith("BEGIN:VCALENDAR"));

            // write warning that not all content lines could be parsed as iCalendar objects
            if (contentLines.Count > 0) { Console.WriteLine("WARNING: Not all content lines could be parsed as iCalendar objects! Make sure that everything was parsed!"); }
        }

        public string Serialize()
        {
            return Objects.Select(x => x.Serialize()).Aggregate((x, y) => x + "\r\n" + y);
        }

        #endregion Methods
    }
}
