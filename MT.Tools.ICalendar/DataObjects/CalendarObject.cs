using MT.Tools.ICalendar.DataObjects.CalendarComponent;
using MT.Tools.ICalendar.DataObjects.PropertyValue.Primitive;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MT.Tools.ICalendar.DataObjects
{
    public class CalendarObject : ISerializableObject
    {
        #region Constructor

        public CalendarObject() { }

        public CalendarObject(CalendarProperty<TextValue> prodId, CalendarProperty<TextValue> version, IEnumerable<ICalendarComponent> components, 
            CalendarProperty<TextValue> calScale = null, CalendarProperty<TextValue> method = null, IEnumerable<CalendarProperty<TextValue>> additionalproperties = null)
        {
            // required parameters
            ProductId = prodId;
            Version = version;
            Components = components;

            // optional parameters
            CalScale = calScale;
            Method = method;
            AdditionalProperties = additionalproperties;
        }

        #endregion Constructor

        #region Members

        // required, unique properties
        public CalendarProperty<TextValue> ProductId { get; set; }
        public CalendarProperty<TextValue> Version { get; set; }

        // optional, unique properties
        public CalendarProperty<TextValue> CalScale { get; set; } = null;
        public CalendarProperty<TextValue> Method { get; set; } = null;

        // optional, non-unique properties
        public IEnumerable<CalendarProperty<TextValue>> AdditionalProperties { get; set; } = new List<CalendarProperty<TextValue>>();

        // the component of the calendar object
        public IEnumerable<ICalendarComponent> Components { get; set; } = new List<ICalendarComponent>();

        // computed list of all properties
        public IEnumerable<CalendarProperty<TextValue>> AllProperties
        {
            get { return new List<CalendarProperty<TextValue>>() { ProductId, Version, CalScale, Method }.Where(x => x != null).Union(AdditionalProperties); }
        }

        #endregion Members

        #region Methods

        public void Deserialize(string content)
        {
            // remove heading and trailing white spaces
            content = content.Trim();

            // retrieve content lines
            var contentLines = content.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim());

            // make sure that the first and last line are correct
            bool isFirstAndLastLineCorrect = contentLines.First().ToUpper().Equals("BEGIN:VCALENDAR") && contentLines.Last().ToUpper().Equals("END:VCALENDAR");
            if (!isFirstAndLastLineCorrect) { throw new ArgumentException("Invalid iCalendar markups at start / end of the calendar object!"); }

            // remove the iCalendar object markups (first and last line)
            contentLines = contentLines.Except(new List<string>() { contentLines.First(), contentLines.Last() });
            //contentLines = contentLines.ToList().GetRange(1, contentLines.Count() - 2);

            do
            {
                // find iCalendar component markups and parse the component from it
                var componentLines = contentLines.SkipWhile(x => !x.StartsWith("BEGIN:")).TakeWhile(x => !x.StartsWith("END:"));
                string componentContent = componentLines.Aggregate((x, y) => $"{ x }\r\n{ y }");
                Components.Append(deserializeComponent(componentContent));

                // remove already parsed lines
                contentLines = contentLines.Except(componentLines);
            }
            // BEGIN tag => begin of next iCalendar component, END tag => end of iCalendar object
            while (contentLines.First().StartsWith("BEGIN"));

            // parse the properties from the remaining content lines
            foreach (var line in contentLines)
            {
                // TODO: handle different property value types
                var property = ObjectSerializer.Deserialize<CalendarProperty<TextValue>>(line);

                // apply property to members
                switch (property.Key.ToUpper())
                {
                    case "VERSION":  Version = property;                    break;
                    case "PRODID":   ProductId = property;                  break;
                    case "CALSCALE": CalScale = property;                   break;
                    case "METHOD":   Method = property;                     break;
                    default:         AdditionalProperties.Append(property); break;
                }
            }
        }

        private ICalendarComponent deserializeComponent(string content)
        {
            ICalendarComponent component;

            // make sure that heading whitespaces get removed
            content = content.TrimStart();

            // find out the component type and parse the component accordingly
            if (content.StartsWith("BEGIN:VEVENT"))
            {
                component = ObjectSerializer.Deserialize<EventComponent>(content);
            }
            else if (content.StartsWith("BEGIN:VTODO"))
            {
                component = ObjectSerializer.Deserialize<TodoComponent>(content);
            }
            else if (content.StartsWith("BEGIN:VJOURNAL"))
            {
                component = ObjectSerializer.Deserialize<JournalComponent>(content);
            }
            else if (content.StartsWith("BEGIN:VFREEBUSY"))
            {
                component = ObjectSerializer.Deserialize<FreeBusyComponent>(content);
            }
            else if (content.StartsWith("BEGIN:VTIMEZONE"))
            {
                component = ObjectSerializer.Deserialize<TimezoneComponent>(content);
            }
            else if (content.StartsWith("BEGIN:VALARM"))
            {
                component = ObjectSerializer.Deserialize<AlarmComponent>(content);
            }
            else if (content.StartsWith("BEGIN:"))
            {
                component = ObjectSerializer.Deserialize<CustomComponent>(content);
            }
            else
            {
                throw new ArgumentException("Invalid iCalendar component detected! Missing BEGIN markup!");
            }

            return component;
        }

        public string Serialize()
        {
            // serialize properties and component
            string propertiesContent = AllProperties.Select(x => x.Serialize()).Aggregate((x, y) => x + "\r\n" + y);
            string componentContent = Components.Select(x => x.Serialize()).Aggregate((x, y) => x + "\r\n" + y);

            // put contents together and add begin / end tags
            string content = $"BEGIN:VCALENDAR\r\n{ propertiesContent }\r\n{ componentContent }\r\nEND:VCALENDAR";
            return content;
        }

        #endregion Methods
    }
}
