using MT.Tools.ICalendar.DataObjects.PropertyParameter;
using System;
using System.Collections.Generic;
using System.Text;

namespace MT.Tools.ICalendar.DataObjects.PropertyParameter
{
    public class DirectoryEntryReferenceParameter : UriBasedPropertyParameter
    {
        #region Constructor

        public DirectoryEntryReferenceParameter() : base() { }

        public DirectoryEntryReferenceParameter(string uri) : base(uri) { }

        public DirectoryEntryReferenceParameter(Uri uri) : base(uri) { }

        #endregion Constructor

        #region Members

        public override PropertyParameterType Type => PropertyParameterType.DirectoryEntryReference;

        public override string ParameterKeyword => "DIR";

        #endregion Members
    }
}
