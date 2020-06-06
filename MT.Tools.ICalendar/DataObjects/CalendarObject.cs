using MT.Tools.ICalendar.DataObjects.CalendarComponent;
using MT.Tools.ICalendar.DataObjects.Factory;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MT.Tools.ICalendar.DataObjects
{
    public class CalendarObject : ISerializableObject
    {
        #region Constants

        public static readonly string[] NonCustomProperties = new string[]
        {
            CalendarTextProperty.PROPERTY_PRODID,
            CalendarTextProperty.PROPERTY_VERSION,
            CalendarTextProperty.PROPERTY_CALSCALE,
            CalendarTextProperty.PROPERTY_METHOD
        };

        #endregion Constants

        #region Constructor

        public CalendarObject() { }

        public CalendarObject(IEnumerable<CalendarTextProperty> properties, IEnumerable<ICalendarComponent> components)
        {
            // TODO: create a new function that does the validation stuff

            // make sure that the properties are valid
            if (properties?.Count() < 2) { throw new ArgumentException("Insufficient properties specified! The list must at least contain PRODID and VERSION text property!"); }
            if (!properties.Any(x => x.Key.Equals(CalendarTextProperty.PROPERTY_PRODID))) { throw new ArgumentException("Invalid properties! Missing PRODID text property!"); }
            if (!properties.Any(x => x.Key.Equals(CalendarTextProperty.PROPERTY_VERSION))) { throw new ArgumentException("Invalid properties! Missing VERSION text property!"); }

            // make sure that the components are valid
            if (components?.Count() < 2) { throw new ArgumentException("Insufficient components specified! The list must at least contain one component!"); }

            Properties = properties;
            Components = components;
        }

        public CalendarObject(string prodId, string version, IEnumerable<ICalendarComponent> components, 
            string calScale = null, string method = null, IEnumerable<CalendarTextProperty> additionalProperties = null)
            : this(prepareProperties(prodId, version, calScale, method, additionalProperties), components) { }

        #region Helpers

        private static IEnumerable<CalendarTextProperty> prepareProperties(string prodId, string version, 
            string calScale = null, string method = null, IEnumerable<CalendarTextProperty> additionalProperties = null)
        {
            // init properties list with required properties
            IEnumerable<CalendarTextProperty> properties = new List<CalendarTextProperty>()
            {
                new CalendarTextProperty(CalendarTextProperty.PROPERTY_VERSION, version),
                new CalendarTextProperty(CalendarTextProperty.PROPERTY_PRODID, prodId),
            };

            // append calscale and method property if specified (not null)
            if (!string.IsNullOrEmpty(calScale)) { properties.Append(new CalendarTextProperty(CalendarTextProperty.PROPERTY_CALSCALE, calScale)); }
            if (!string.IsNullOrEmpty(method)) { properties.Append(new CalendarTextProperty(CalendarTextProperty.PROPERTY_METHOD, method)); }
            
            // append the additional properties
            properties = properties.Union(additionalProperties ?? new CalendarTextProperty[0]);

            return properties;
        }

        #endregion Helpers

        #endregion Constructor

        #region Members

        /// <summary>
        /// The properties of the iCalendar object.
        /// </summary>
        public IEnumerable<CalendarTextProperty> Properties { get; } = new List<CalendarTextProperty>();

        /// <summary>
        /// The components of the iCalendar object.
        /// </summary>
        public IEnumerable<ICalendarComponent> Components { get; set; } = new List<ICalendarComponent>();

        // required, unique properties
        public string ProductId { get { return getPropertyValue(CalendarTextProperty.PROPERTY_PRODID); } }
        public string Version { get { return getPropertyValue(CalendarTextProperty.PROPERTY_VERSION); } }

        // optional, unique properties
        public string CalScale { get { return getPropertyValue(CalendarTextProperty.PROPERTY_CALSCALE); } }
        public string Method { get { return getPropertyValue(CalendarTextProperty.PROPERTY_METHOD); } }

        // optional, non-unique properties
        public IEnumerable<CalendarTextProperty> AdditionalProperties { get { return Properties.Where(x => !NonCustomProperties.Any(y => y.Equals(x?.Key.ToUpper()))); } }

        #endregion Members

        #region Methods

        public void Deserialize(string content)
        {
            // remove heading and trailing white spaces
            content = content.Trim();

            // extract the first and the last line
            string firstLine = content.Substring(0, Math.Min(content.IndexOf('\r'), content.IndexOf('\n'))).Trim();
            string lastLine = content.Substring(content.LastIndexOf('\n') + 1).Trim();

            // make sure that the first and last line are correct
            bool isFirstAndLastLineCorrect = firstLine.ToUpper().Equals("BEGIN:VCALENDAR") && lastLine.ToUpper().Equals("END:VCALENDAR");
            if (!isFirstAndLastLineCorrect) { throw new ArgumentException("Invalid iCalendar markups at start / end of the calendar object!"); }

            // determine the start of the first iCalendar component (= end of iCalendar object properties)
            int firstCompsStart = content.IndexOf("BEGIN:", firstLine.Length);

            // make sure that the iCalendar object has at least one component
            if (firstCompsStart < 0) { throw new ArgumentException("Missing components! The iCalendar object has no components!"); }

            // TODO: think about multi-threading optimizations (e.g. parsing properties and components at the same time)

            // parse the properties from the content lines
            string propertiesContent = content.Substring(firstLine.Length, firstCompsStart - firstLine.Length); // TODO: make sure that this index is correct
            deserializeProperties(propertiesContent);

            // parse iCalendar components
            var componentLines = content.Substring(firstCompsStart, content.Length - firstCompsStart - lastLine.Length).Trim();
            deserializeComponents(componentLines);
        }

        private void deserializeProperties(string content)
        {
            // split content at line break
            var contentLines = content.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

            // deserialize each line as a property
            foreach (var line in contentLines)
            {
                var property = ObjectSerializer.Deserialize<CalendarTextProperty>(line);
                Properties.Append(property);
            }
        }

        private void deserializeComponents(string content)
        {
            // TODO: think about multi-threading optimization (e.g. parsing components parallel)

            int offset = 0;

            while (offset < content.Length && offset != -1)
            {
                // extract the lines of the next iCalendar component
                int endOfComponentIndex = endIndexOfElementBound(content.Substring(offset));
                var componentContent = content.Substring(offset, endOfComponentIndex - offset).Trim();
                // TODO: make sure that the indexing is correct and does not run into out-of-bounds error

                // parse the component from the extracted lines
                var component = CalendarFactory.DeserializeCalendarComponent(componentContent);
                Components.Append(component);

                // remove already parsed lines
                offset = endOfComponentIndex;
            }
        }

        private int endIndexOfElementBound(string content)
        {
            // init depth and offset, so the first begin flag gets skipped
            int depth = 1;
            int offset = content.IndexOf("BEGIN:") + 6;

            // go through content until closing flag is found
            while (offset < content.Length && depth > 0)
            {
                // find next beginning and ending flag
                int indexOfNextBeginFlag = content.IndexOf("\nBEGIN:", offset);
                int indexOfNextEndFlag = content.IndexOf("\nEND:", offset);

                // increment / decrement depth
                if (indexOfNextBeginFlag != -1 && indexOfNextBeginFlag < indexOfNextEndFlag) { depth++; } else { depth--; }

                // update offset
                offset = Math.Min(indexOfNextBeginFlag, indexOfNextEndFlag) + 1;
            }

            // find the end of the last ending flag (line break)
            return Math.Min(content.IndexOf('\r', offset), content.IndexOf('\n', offset));
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

        private string getPropertyValue(string key) => Properties.Where(x => x.Key.Equals(key)).FirstOrDefault()?.Value;

        #endregion Helpers

        #endregion Methods
    }
}
