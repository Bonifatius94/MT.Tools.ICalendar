using MT.Tools.ICalendar.DataObjects.PropertyValue.Other;
using System;
using System.Collections.Generic;
using System.Text;

namespace MT.Tools.ICalendar.DataObjects.PropertyParameter
{
    public class SentByParameter : UriBasedPropertyParameter
    {
        #region Constructor

        public SentByParameter() : base() { }

        public SentByParameter(CalendarUserAddressValue address) : base(address.Value) { }

        #endregion Constructor

        #region Members

        public override PropertyParameterType Type => PropertyParameterType.SentBy;

        public override string ParameterKeyword => "SENT-BY";

        public CalendarUserAddressValue Address { get => Uri as CalendarUserAddressValue; set => Uri = value; }

        #endregion Members
    }
}
