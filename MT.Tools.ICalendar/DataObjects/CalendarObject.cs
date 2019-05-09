using MT.Tools.ICalendar.DataObjects.CalendarComponent;
using MT.Tools.ICalendar.DataObjects.Collection;
using MT.Tools.ICalendar.DataObjects.Property;
using MT.Tools.ICalendar.DataObjects.PropertyValue;
using MT.Tools.ICalendar.DataObjects.PropertyValue.Primitive;
using MT.Tools.ICalendar.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MT.Tools.ICalendar.DataObjects
{
    public class CalendarObject : ISerializableObject, IPropertyCollection
    {
        #region Constants

        public const string PROPERTY_PRODID = "PRODID";
        public const string PROPERTY_VERSION = "VERSION";
        public const string PROPERTY_CALSCALE = "CALSCALE";
        public const string PROPERTY_METHOD = "METHOD";

        public static readonly string[] NonCustomProperties = new string[] { PROPERTY_PRODID, PROPERTY_VERSION, PROPERTY_CALSCALE, PROPERTY_METHOD };

        #endregion Constants

        #region Constructor

        public CalendarObject() { }

        public CalendarObject(IEnumerable<ICalendarProperty> properties, IEnumerable<ICalendarComponent> components)
        {
            // TODO: create a new function that does the validation stuff

            // make sure that the properties are valid
            if (properties?.Count() < 2) { throw new ArgumentException("Insufficient properties specified! The list must at least contain PRODID and VERSION text property!"); }
            if (!properties.Any(x => x.Key.Equals(PROPERTY_PRODID))) { throw new ArgumentException("Invalid properties! Missing PRODID text property!"); }
            if (!properties.Any(x => x.Key.Equals(PROPERTY_VERSION))) { throw new ArgumentException("Invalid properties! Missing VERSION text property!"); }

            // make sure that the components are valid
            if (components?.Count() < 2) { throw new ArgumentException("Insufficient components specified! The list must at least contain one component!"); }

            Properties = properties;
            Components = components;
        }

        public CalendarObject(string prodId, string version, IEnumerable<ICalendarComponent> components, 
            string calScale = null, string method = null, IEnumerable<ICalendarProperty> additionalProperties = null)
            : this(prepareProperties(prodId, version, calScale, method, additionalProperties), components) { }

        #region Helpers

        private static IEnumerable<ICalendarProperty> prepareProperties(string prodId, string version, 
            string calScale = null, string method = null, IEnumerable<ICalendarProperty> additionalProperties = null)
        {
            // init properties list with required properties
            IEnumerable<ICalendarProperty> properties = new List<ICalendarProperty>()
            {
                new SimpleCalendarProperty<TextValue>(PROPERTY_VERSION, new TextValue(version)),
                new SimpleCalendarProperty<TextValue>(PROPERTY_PRODID, new TextValue(prodId)),
            };

            // append calscale and method property if specified (not null)
            if (!string.IsNullOrEmpty(calScale)) { properties.Append(new SimpleCalendarProperty<TextValue>(PROPERTY_CALSCALE, new TextValue(calScale))); }
            if (!string.IsNullOrEmpty(method)) { properties.Append(new SimpleCalendarProperty<TextValue>(PROPERTY_METHOD, new TextValue(method))); }
            
            // append the additional properties
            properties = properties.Union(additionalProperties ?? new ICalendarProperty[0]);

            return properties;
        }

        #endregion Helpers

        #endregion Constructor

        #region Members

        /// <summary>
        /// The properties of the iCalendar object.
        /// </summary>
        public IEnumerable<ICalendarProperty> Properties { get; } = new List<GenericCalendarProperty>();

        /// <summary>
        /// The components of the iCalendar object.
        /// </summary>
        public IEnumerable<ICalendarComponent> Components { get; set; } = new List<ICalendarComponent>();

        // required, unique properties
        public TextValue ProductId { get { return getPropertyValue(PROPERTY_PRODID) as TextValue; } }
        public TextValue Version { get { return getPropertyValue(PROPERTY_VERSION) as TextValue; } }

        // optional, unique properties
        public TextValue CalScale { get { return getPropertyValue(PROPERTY_CALSCALE) as TextValue; } }
        public TextValue Method { get { return getPropertyValue(PROPERTY_METHOD) as TextValue; } }

        // optional, non-unique properties
        public IEnumerable<ICalendarProperty> AdditionalProperties { get { return Properties.Where(x => !NonCustomProperties.Any(y => y.Equals(x?.Key.ToUpper()))); } }

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
            if (!isFirstAndLastLineCorrect) { throw new ArgumentException("Invalid iCalendar markups at start / end of the calendar object!"); }

            // remove the iCalendar object markups (first and last line)
            contentLines = contentLines.GetRange(1, contentLines.Count - 2);

            // make sure that the iCalendar object has at least one component
            bool hasComponent = contentLines.Any(x => x.ToUpper().StartsWith("BEGIN:"));
            if (!hasComponent) { throw new ArgumentException("Missing components! The iCalendar object has no components!"); }

            // determine the start of the first iCalendar component (= end of iCalendar object properties)
            int firstCompsStart = contentLines.IndexOf(contentLines.Where(x => x.ToUpper().StartsWith("BEGIN:")).First());

            // TODO: think about multi-threading optimizations (e.g. parsing properties and components at the same time)

            // parse the properties from the content lines
            var propertyLines = contentLines.GetRange(0, firstCompsStart);
            deserializeProperties(propertyLines);

            // parse iCalendar components
            var componentLines = contentLines.GetRange(firstCompsStart, contentLines.Count - propertyLines.Count);
            deserializeComponents(componentLines);
        }

        private void deserializeProperties(List<string> contentLines)
        {
            foreach (var line in contentLines)
            {
                var property = ObjectSerializer.Deserialize<GenericCalendarProperty>(line);
                Properties.Append(property);
            }
        }

        private void deserializeComponents(List<string> contentLines)
        {
            // TODO: think about multi-threading optimization (e.g. parsing components parallel)

            do
            {
                // extract the lines of the next iCalendar component
                var componentLines = contentLines.GetRange(0, indexOfClosingTag(contentLines) + 1);
                string componentContent = componentLines.Aggregate((x, y) => $"{ x }\r\n{ y }");

                // parse the component from the extracted lines
                Components.Append(deserializeComponent(componentContent));

                // remove already parsed lines
                contentLines = contentLines.GetRange(componentLines.Count, contentLines.Count - componentLines.Count);
            }
            // BEGIN tag => begin of next iCalendar component, END tag => end of iCalendar object
            while (contentLines.Count > 0 && contentLines.First().ToUpper().StartsWith("BEGIN:"));
        }

        private int indexOfClosingTag(List<string> contentLines)
        {
            int depth = 1;
            int index;

            // loop through content lines until the end is reached or the closing tag was found
            for (index = 1; index < contentLines.Count && depth > 0; index++)
            {
                // BEGIN tag => increase depth, END tag => decrease depth
                if (contentLines[index]?.ToUpper().StartsWith("BEGIN:") == true) { depth++; }
                if (contentLines[index]?.ToUpper().StartsWith("END:") == true) { depth--; }
            }

            return index;
        }

        private ICalendarComponent deserializeComponent(string content)
        {
            ICalendarComponent component;

            // make sure that heading whitespaces get removed
            content = content.TrimStart();

            // find out the component type and parse the component accordingly
            if (content.ToUpper().StartsWith("BEGIN:VEVENT"))
            {
                component = ObjectSerializer.Deserialize<EventComponent>(content);
            }
            else if (content.ToUpper().StartsWith("BEGIN:VTODO"))
            {
                component = ObjectSerializer.Deserialize<TodoComponent>(content);
            }
            else if (content.ToUpper().StartsWith("BEGIN:VJOURNAL"))
            {
                component = ObjectSerializer.Deserialize<JournalComponent>(content);
            }
            else if (content.ToUpper().StartsWith("BEGIN:VFREEBUSY"))
            {
                component = ObjectSerializer.Deserialize<FreeBusyComponent>(content);
            }
            else if (content.ToUpper().StartsWith("BEGIN:VTIMEZONE"))
            {
                component = ObjectSerializer.Deserialize<TimezoneComponent>(content);
            }
            else if (content.ToUpper().StartsWith("BEGIN:VALARM"))
            {
                component = ObjectSerializer.Deserialize<AlarmComponent>(content);
            }
            else if (content.ToUpper().StartsWith("BEGIN:"))
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
            string propertiesContent = Properties.Select(x => x.Serialize()).Aggregate((x, y) => x + "\r\n" + y);
            string componentContent = Components.Select(x => x.Serialize()).Aggregate((x, y) => x + "\r\n" + y);

            // put contents together and add begin / end tags
            string content = $"BEGIN:VCALENDAR\r\n{ propertiesContent }\r\n{ componentContent }\r\nEND:VCALENDAR";
            return content;
        }

        #region Helpers

        private IPropertyValue getPropertyValue(string key) => Properties.Where(x => x.Key.Equals(key)).FirstOrDefault()?.Value;

        #endregion Helpers

        #endregion Methods
    }
}
