using MT.Tools.ICalendar.DataObjects.PropertyValue.Other;
using System;
using System.Collections.Generic;
using System.Text;

namespace MT.Tools.ICalendar.DataObjects.PropertyParameter
{
    public class GroupOrListMembershipParameter : AddressCollectionPropertyParameter
    {
        #region Constructor

        public GroupOrListMembershipParameter() : base() { }

        public GroupOrListMembershipParameter(CalendarUserAddressValue address) : base(address) { }

        public GroupOrListMembershipParameter(IEnumerable<CalendarUserAddressValue> addresses) : base(addresses) { }

        #endregion Constuctor

        #region Members

        public override PropertyParameterType Type => PropertyParameterType.GroupOrListMembership;

        public override string ParameterKeyword => "MEMBER";

        #endregion Members
    }
}
