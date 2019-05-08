using MT.Tools.ICalendar.DataObjects.PropertyValue.Other;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MT.Tools.ICalendar.DataObjects.PropertyParameter
{
    public abstract class AddressCollectionPropertyParameter : IPropertyParameter
    {
        #region Constructor

        public AddressCollectionPropertyParameter() { }

        public AddressCollectionPropertyParameter(CalendarUserAddressValue address) : this(new List<CalendarUserAddressValue>() { address }) { }

        public AddressCollectionPropertyParameter(IEnumerable<CalendarUserAddressValue> addresses)
        {
            // make sure that the list is not empty
            if (addresses?.Count() == 0 || addresses.Any(x => x == null)) { throw new ArgumentException("Invalid addresses list! List and its' elements must not be null or empty!"); }

            Addresses = addresses;
        }

        #endregion Constuctor

        #region Members

        public abstract PropertyParameterType Type { get; }

        public abstract string ParameterKeyword { get; }

        public virtual IEnumerable<CalendarUserAddressValue> Addresses { get; set; } = new List<CalendarUserAddressValue>();

        #endregion Members

        #region Methods

        public void Deserialize(string content)
        {
            // remove heading and trailing white spaces
            content = content.Trim();

            // make sure that the parameter starts with DELEGATED-FROM or DELEGATED-TO
            if (!content.ToUpper().StartsWith(ParameterKeyword.ToUpper()))
            {
                throw new ArgumentException("Invalid delegator content detected! Property parameter needs to start with DELEGATED-FROM or DELEGATED-TO keyword!");
            }

            // parse addresses
            string addressesContent = content.Substring(content.IndexOf('=') + 1);
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

            return $"{ ParameterKeyword }={ Addresses.Select(x => x.Serialize()).Aggregate((x, y) => x + "," + y) }";
        }

        #endregion Methods
    }
}
