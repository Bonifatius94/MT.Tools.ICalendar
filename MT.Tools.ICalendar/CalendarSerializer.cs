﻿using MT.Tools.ICalendar.DataObjects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MT.Tools.ICalendar
{
    public interface ICalendarSerializer
    {
        #region Methods

        CalendarFile Deserialize(string calendarFilePath);

        CalendarFile Deserialize(Stream inStream);

        void Serialize(CalendarFile calendarFile, string outFilePath, bool overwrite = true);

        void Serialize(CalendarFile calendarFile, Stream outStream);

        #endregion Methods
    }

    public class CalendarSerializer : ICalendarSerializer
    {
        #region Methods

        #region Deserialize

        public CalendarFile Deserialize(string calendarFilePath)
        {
            CalendarFile data = null;

            // check if the calendar file exists
            if (!File.Exists(calendarFilePath)) { throw new FileNotFoundException("input file not found", calendarFilePath); }

            // open a stream that is reading in the calendar file
            using (var stream = File.OpenRead(calendarFilePath))
            {
                // make sure the stream starts at the beginning of the file
                stream.Seek(0, SeekOrigin.Begin);

                // deserialize the content from the file stream
                data = Deserialize(stream);
            }

            return data;
        }

        public CalendarFile Deserialize(Stream inStream)
        {
            byte[] utf8Bytes;

            using (var reader = new BinaryReader(inStream))
            {
                // get all bytes from the stream at once
                utf8Bytes = reader.ReadBytes((int)inStream.Length);
            }

            // convert UTF-8 bytes to unicode string
            var unicodeBytes = Encoding.Convert(Encoding.UTF8, Encoding.Unicode, utf8Bytes);
            var foldedContent = Encoding.Default.GetString(unicodeBytes);

            // unfold content
            string unfoldedContent = new FoldHelper().UnfoldContent(foldedContent);

            // parse content and retrieve the object tree
            var calendar = new CalendarFile();
            calendar.TryDeserialize(unfoldedContent);

            return calendar;
        }

        #endregion Deserialize

        public void Serialize(CalendarFile calendarFile, string outFilePath, bool overwrite = true)
        {
            // make sure that overwriting works as expected
            if (!overwrite && File.Exists(outFilePath)) { throw new InvalidOperationException("Cannot overwrite already existing file! Please make sure that overwriting is permitted!"); }

            // create a new output file stream
            using (var outStream = new FileStream(outFilePath, FileMode.CreateNew, FileAccess.Write, FileShare.Write))
            {
                // write the bytes to the output file
                Serialize(calendarFile, outStream);
            }
        }

        public void Serialize(CalendarFile calendarFile, Stream outStream)
        {
            // get the content as string (unicode)
            string unfoldedContent = calendarFile.Serialize();

            // fold the unfolded content
            var foldedContent = new FoldHelper().FoldContent(unfoldedContent);

            // convert unicode string to UTF-8 bytes
            var unicodeBytes = Encoding.Default.GetBytes(foldedContent);
            var utf8Bytes = Encoding.Convert(Encoding.Default, Encoding.UTF8, unicodeBytes);

            // write the bytes to the output file
            using (var writer = new BinaryWriter(outStream))
            {
                writer.Write(utf8Bytes);
            }
        }

        #endregion Methods
    }
}