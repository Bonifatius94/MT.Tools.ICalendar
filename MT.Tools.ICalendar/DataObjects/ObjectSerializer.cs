using System;
using System.Collections.Generic;
using System.Text;

namespace MT.Tools.ICalendar.DataObjects
{
    public static class ObjectSerializer
    {
        #region Methods

        public static T Deserialize<T>(string content) 
            where T : ISerializableObject, new()
        {
            var obj = new T();
            obj.Deserialize(content);
            return obj;
        }

        public static string Serialize<T>(ISerializableObject obj)
        {
            return obj.Serialize();
        }

        #endregion Methods
    }
}
