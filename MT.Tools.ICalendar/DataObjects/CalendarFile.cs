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

        public void TryDeserialize(string content)
        {
            throw new NotImplementedException();
        }

        public string Serialize()
        {
            throw new NotImplementedException();
        }

        #endregion Methods
    }
}
