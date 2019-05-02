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
            // create a new, empty instance
            var obj = new T();

            // try to deserialize the given content
            obj.Deserialize(content);

            return obj;
        }

        public static bool TryDeserialize<T>(string content, out T value)
            where T : ISerializableObject, new()
        {
            // init ret and value for failure
            bool ret = false;
            value = default(T);

            try
            {
                // try to deserialize the value
                value = Deserialize<T>(content);

                // deserialization successful!
                ret = true;
            }
            catch (Exception) { }

            return ret;
        }

        public static string Serialize<T>(ISerializableObject obj)
        {
            return obj.Serialize();
        }

        #endregion Methods
    }
}
