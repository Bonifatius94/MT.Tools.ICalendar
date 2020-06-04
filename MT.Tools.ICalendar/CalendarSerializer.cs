using MT.Tools.ICalendar.DataObjects;
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
            string foldedContent;

            // read text from stream (converts UTF-8 to Unicode)
            using (var reader = new StreamReader(inStream, Encoding.UTF8))
            {
                foldedContent = reader.ReadToEnd();
            }

            // unfold content
            string unfoldedContent = FoldHelper.UnfoldContent(foldedContent);

            // parse content and retrieve the object tree
            var calendar = ObjectSerializer.Deserialize<CalendarFile>(unfoldedContent);

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
            var foldedContent = FoldHelper.FoldContent(unfoldedContent);

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

    public static class FoldHelper
    {
        #region Constants

        public const int MAX_LINE_COUNT = 75;

        //public const byte CHAR_LF = 0x0A;
        //public const byte CHAR_CR = 0x0D;
        //public const byte CHAR_SPACE = 0x20;

        #endregion Constants

        #region Methods

        public static string FoldContent(string unfoldedContent)
        {
            var builder = new StringBuilder();
            var lines = unfoldedContent.Split("\r\n", StringSplitOptions.RemoveEmptyEntries);

            foreach (string line in lines)
            {
                string rest = line;
                int charsToTake = (rest.Length > MAX_LINE_COUNT) ? MAX_LINE_COUNT : rest.Length;

                while (charsToTake > MAX_LINE_COUNT)
                {
                    string append = rest.Substring(0, charsToTake);
                    builder.Append(append + "\r\n ");

                    rest = rest.Substring(charsToTake, rest.Length - charsToTake);
                    charsToTake = (rest.Length > MAX_LINE_COUNT) ? MAX_LINE_COUNT : rest.Length;
                }

                builder.Append(rest + "\r\n");
            }

            return builder.ToString();
        }

        public static string UnfoldContent(string foldedContent)
        {
            return foldedContent.Replace("\r\n ", "");
        }

        #endregion Methods
    }
}
