using System;
using System.Collections.Generic;
using System.Text;

namespace MT.Tools.ICalendar.DataObjects.ComponentProperty
{
    public enum ComponentPropertyType
    {
        // TODO: define component property types
        Attachment,
        Categories,
        Classification,
        Comment,
        Description,
        GeoPosition,
        Location,
        PercentComplete,
        Priority,
        Other
    }

    public interface IComponentProperty : ISerializableObject
    {
        #region Members

        ComponentPropertyType Type { get; }

        #endregion Members
    }
}
