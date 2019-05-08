using MT.Tools.ICalendar.DataObjects.PropertyValue.Other;
using System;
using System.Collections.Generic;
using System.Text;

namespace MT.Tools.ICalendar.DataObjects.PropertyParameter
{
    public class DelegateeParameter : AddressCollectionPropertyParameter
    {
        #region Constructor

        public DelegateeParameter() : base() { }

        public DelegateeParameter(CalendarUserAddressValue address) : base(address) { }

        public DelegateeParameter(IEnumerable<CalendarUserAddressValue> addresses) : base(addresses) { }

        #endregion Constuctor

        #region Members

        public override PropertyParameterType Type => PropertyParameterType.Delegatee;

        public override string ParameterKeyword => "DELEGATED-TO";

        #endregion Members
    }
}
