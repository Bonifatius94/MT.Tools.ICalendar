using MT.Tools.ICalendar.DataObjects.PropertyValue;
using MT.Tools.ICalendar.DataObjects.PropertyValue.Other;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace MT.Tools.ICalendar.DataObjects.PropertyParameter
{
    public class AlternateTextRepresentationParamter : UriBasedPropertyParameter
    {
        #region Constructor

        public AlternateTextRepresentationParamter() : base() { }

        public AlternateTextRepresentationParamter(string uri) : base(uri) { }

        public AlternateTextRepresentationParamter(Uri uri) : base(uri) { }

        #endregion Constructor

        #region Members

        public override string ParameterKeyword => "ALTREP";

        public override PropertyParameterType Type => PropertyParameterType.AlternateTextRepresentation;

        #endregion Members
    }
}
