using System;
using System.Collections.Generic;
using System.Text;

namespace MT.Tools.ICalendar.DataObjects
{
    public interface ISerializableObject
    {
        #region Methods

        /// <summary>
        /// Get the unfolded content of the serializable iCalendar item.
        /// </summary>
        /// <returns>the unfolded representation of the serializable item</returns>
        string Serialize();

        /// <summary>
        /// Deserializes the given unfolded content into a iCalendar object tree
        /// </summary>
        /// <param name="content">The content to be deserialized</param>
        void Deserialize(string content);

        // TODO: add the validation function below and introduce it for every implementation

        ///// <summary>
        ///// Validates the serializable object. If the data is invalid an exception will be thrown.
        ///// </summary>
        //void Validate();

        #endregion Methods
    }
}
