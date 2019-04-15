﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace MT.Tools.ICalendar.DataObjects.PropertyParameter
{
    public class AlternateTextRepresentationParamter : ISerializableObject
    {
        #region Members

        public Uri Uri { get; private set; }

        #endregion Members

        #region Methods
         
        public void Deserialize(string content)
        {
            // determine the start and end indices of the uri
            int uriStart = content.IndexOf('\"') + 1;
            int uriEnd = content.IndexOf('\"', uriStart) - 1;

            // get the uri string from content and create a new uri instance from it
            string uriContent = content.Substring(uriStart, uriEnd - uriStart);
            Uri = new Uri(uriContent);

            // TODO: create a value class that gets the content from the uri
        }

        public string Serialize()
        {
            throw new NotImplementedException();
        }

        #endregion Methods
    }
}
