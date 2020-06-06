using System;
using System.Collections.Generic;
using System.Text;

namespace MT.Tools.ICalendar.DataObjects.PropertyValue
{
    public class AttachmentValue : DualSwitchValue<UriValue, BinaryValue>
    {
        #region Constructor

        public AttachmentValue(UriValue uri) : base(uri) { }

        public AttachmentValue(BinaryValue binary) : base(binary) { }

        #endregion Constructor
    }
}
