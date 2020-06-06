using MT.Tools.ICalendar.DataObjects.PropertyValue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MT.Tools.ICalendar.DataObjects.PropertyParameter
{
    public class DelegatorParameter : AddressCollectionPropertyParameter
    {
        #region Constructor

        public DelegatorParameter() : base() { }

        public DelegatorParameter(CalendarUserAddressValue address) : base(address) { }

        public DelegatorParameter(IEnumerable<CalendarUserAddressValue> addresses) : base(addresses) { }

        #endregion Constuctor

        #region Members

        public override PropertyParameterType Type => PropertyParameterType.Delegator;

        public override string ParameterKeyword => "DELEGATED-FROM";

        #endregion Members
    }
}
