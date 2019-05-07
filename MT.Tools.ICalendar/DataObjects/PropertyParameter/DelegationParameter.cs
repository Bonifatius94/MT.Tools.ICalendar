using MT.Tools.ICalendar.DataObjects.PropertyValue.Other;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MT.Tools.ICalendar.DataObjects.PropertyParameter
{
    public class DelegationParameter : IPropertyParameter
    {
        #region Constructor

        public DelegationParameter() { }

        public DelegationParameter(CalendarUserAddressValue address, PropertyParameterType type) : this(new List<CalendarUserAddressValue>() { address }, type) { }

        public DelegationParameter(IEnumerable<CalendarUserAddressValue> addresses, PropertyParameterType type)
        {
            // make sure that the list is not empty
            if (addresses?.Count() == 0) { throw new ArgumentException("Invalid addresses list! List must not be null or empty!"); }

            // make sure that the type is either 'Delegator' or 'Delegatee'
            if (type != PropertyParameterType.Delegator && type != PropertyParameterType.Delegatee)
            {
                throw new ArgumentException("Invalid property parameter type! 'Delegator' or 'Delegatee' expected!");
            }

            Type = type;
            Addresses = addresses;
        }

        #endregion Constuctor

        #region Members

        public PropertyParameterType Type { get; private set; }

        public IEnumerable<CalendarUserAddressValue> Addresses { get; set; } = new List<CalendarUserAddressValue>();

        #endregion Members

        #region Methods

        public void Deserialize(string content)
        {
            // remove heading and trailing white spaces
            content = content.Trim();

            // make sure that the parameter starts with DELEGATED-FROM or DELEGATED-TO
            if (!content.StartsWith("DELEGATED-FROM=") && !content.StartsWith("DELEGATED-TO="))
            {
                throw new ArgumentException("Invalid delegator content detected! Property parameter needs to start with DELEGATED-FROM or DELEGATED-TO keyword!");
            }

            // parse delegation type
            string typeAsString = content.Substring(0, content.IndexOf('='));
            Type = "DELEGATED-FROM=".Equals(typeAsString) ? PropertyParameterType.Delegator : PropertyParameterType.Delegatee;

            // parse addresses
            string addressesContent = content.Substring(typeAsString.Length + 1);
            var addressParts = addressesContent.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim());
            var addresses = new List<CalendarUserAddressValue>();

            foreach (var addressPart in addressParts)
            {
                // make sure that the address is surrounded by double quotes
                if (addressPart.First() != '"' || addressPart.Last() != '"')
                {
                    throw new ArgumentException($"Invalid address { addressPart } detected! Address needs to be surrounded with double quotes!");
                }

                // create a new calendar user address value and add it to the addresses list
                string addressWithoutQuotes = addressPart.Substring(1, addressPart.Length - 2);
                addresses.Add(new CalendarUserAddressValue(new Uri(addressWithoutQuotes)));
            }

            Addresses = addresses;
        }

        public string Serialize()
        {
            // make sure that the addresses list contains at least one element
            if (Addresses?.Count() == 0) { throw new ArgumentException("Invalid addresses list! List must not be null or empty!"); }

            string delegationType = Type == PropertyParameterType.Delegator ? "DELEGATED-FROM" : "DELEGATED-TO";
            string serializedAddresses = Addresses.Select(x => x.Serialize()).Aggregate((x, y) => x + "," + y);

            return $"{ delegationType }={ serializedAddresses }";
        }

        #endregion Methods
    }
}
