using System;
using System.Collections.Generic;
using System.Text;

namespace MT.Tools.ICalendar.DataObjects.ComponentProperty
{
    public enum ComponentPropertyType
    {
        // TODO: define component property types

        // descriptive properties
        Attachment,
        Categories,
        Classification,
        Comment,
        Description,
        GeoPosition,
        Location,
        PercentComplete,
        Priority,
        Resources,
        Status,
        Summary,

        // date-time properties
        Completed,
        DateTimeEnd
    }

    public interface IComponentProperty : ISerializableObject
    {
        #region Members

        ComponentPropertyType Type { get; }

        #endregion Members
    }
}
