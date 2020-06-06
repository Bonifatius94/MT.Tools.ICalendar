using MT.Tools.ICalendar.DataObjects.Factory;
using MT.Tools.ICalendar.DataObjects.PropertyParameter;
using MT.Tools.ICalendar.DataObjects.PropertyValue;
using MT.Tools.ICalendar.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MT.Tools.ICalendar.DataObjects.ComponentProperty
{
    public class StatusProperty : IComponentProperty
    {
        #region Constructor

        public StatusProperty() { }

        public StatusProperty(StatusValue status) { Status = status; }

        public StatusProperty(StatusValue status, IEnumerable<IPropertyParameter> parameters) { Status = status; Parameters = parameters; }

        #endregion Constructor

        #region Members

        public string Markup => "STATUS";
        public ComponentPropertyType Type => ComponentPropertyType.Status;

        public IEnumerable<IPropertyParameter> Parameters { get; private set; } = new List<IPropertyParameter>();

        public StatusValue Status { get; set; }
        public IPropertyValue Value => Status;

        #endregion Members

        #region Methods

        public void Deserialize(string content)
        {
            // make sure that the parameter starts with STATUS
            if (!content.ToUpper().StartsWith(Markup)) { throw new ArgumentException($"Invalid status detected! Component property needs to start with { Markup } keyword!"); }

            // deserialize parameters
            Parameters =
                content.Substring(Markup.Length, content.IndexOf(':') - Markup.Length)
                .Split(';', StringSplitOptions.RemoveEmptyEntries)
                .Select(x => CalendarFactory.DeserializePropertyParameter(x))
                .ToList();

            // extract the value content
            string valueContent = content.Substring(content.IndexOf(':') + 1).Trim();
            Status = ObjectSerializer.Deserialize<StatusValue>(valueContent);
        }

        public string Serialize()
        {
            string paramsContent = Parameters.Select(x => x.Serialize()).Aggregate((x, y) => x + ";" + y);
            return $"{ Markup }{ (string.IsNullOrEmpty(paramsContent) ? "" : ";" + paramsContent) }:{ Status.Serialize() }";
        }

        #endregion Methods
    }
}
