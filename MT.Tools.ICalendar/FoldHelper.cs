using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MT.Tools.ICalendar
{
    public interface IFoldHelper
    {
        /// <summary>
        /// Folds the given UTF-8 encoded data to retrieve lines with only less than 75 characters.
        /// </summary>
        /// <param name="utf8Bytes">UTF-8 encoded data to be folded</param>
        /// <returns>the folded UTF-8 data</returns>
        string FoldContent(string utf8Bytes);

        /// <summary>
        /// Unfolds the given UTF-8 encoded data and therefore removes additional line breaks caused by folding.
        /// </summary>
        /// <param name="utf8Bytes">UTF-8 encoded data to be unfolded</param>
        /// <returns>the unfolded UTF-8 data</returns>
        string UnfoldContent(string utf8Bytes);
    }

    public class FoldHelper : IFoldHelper
    {
        #region Constants

        public const int MAX_LINE_COUNT = 75;

        //public const byte CHAR_LF = 0x0A;
        //public const byte CHAR_CR = 0x0D;
        //public const byte CHAR_SPACE = 0x20;

        #endregion Constants

        #region Methods

        public string FoldContent(string unfoldedContent)
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

        public string UnfoldContent(string foldedContent)
        {
            return foldedContent.Replace("\r\n ", "");
        }

        //public byte[] FoldContent(byte[] data)
        //{
        //    // TODO: test this code

        //    byte[] output;
        //    byte[] buffer = new byte[75];
        //    byte[] lineBreakSequence = new byte[] { CHAR_CR, CHAR_LF, CHAR_SPACE };

        //    // create a memory stream for caching the output content
        //    using (var memoryStream = new MemoryStream())
        //    {
        //        int offset = 0;

        //        while (offset < data.Length)
        //        {
        //            // TODO: fix issue if content has a line break which is not the end of a content line

        //            int lineBreak = indexOfNextLineBreak(data, offset);

        //            while (offset < lineBreak)
        //            {
        //                // read bytes from input stream
        //                int bytesCount = (lineBreak - offset >= 75) ? 75 : (lineBreak - offset);
        //                Array.Copy(data, offset, buffer, 0, bytesCount);

        //                // write content bytes to the output stream
        //                memoryStream.Write(buffer, 0, bytesCount);

        //                // write line break to output stream
        //                bool writeWhiteSpace = (offset + bytesCount < lineBreak);
        //                memoryStream.Write(lineBreakSequence, 0, 2 + (writeWhiteSpace ? 1 : 0));

        //                // update offset
        //                offset += bytesCount;
        //            }
        //        }

        //        // convert the cached memory stream back to a byte stream of fixed length
        //        output = memoryStream.ToArray();
        //    }

        //    return output;
        //}

        //public byte[] UnfoldContent(byte[] data)
        //{
        //    // TODO: test this code

        //    byte[] output;
        //    byte[] buffer = new byte[75];
        //    byte[] lineBreakSequence = new byte[] { CHAR_CR, CHAR_LF };

        //    using (var memoryStream = new MemoryStream())
        //    {
        //        int offset = 0;

        //        while (offset < data.Length)
        //        {
        //            // find the next line break
        //            int lineBreak = indexOfNextLineBreak(data, offset);

        //            // copy all content bytes until line break to the stream
        //            int bytesCount = lineBreak - offset;
        //            Array.Copy(data, offset, buffer, 0, bytesCount);
        //            memoryStream.Write(buffer, 0, bytesCount);

        //            // handle line break
        //            bool isFoldedLineBreak = data[lineBreak + 2] == CHAR_SPACE;
        //            if (!isFoldedLineBreak) { memoryStream.Write(lineBreakSequence, 0, 2); }
        //        }

        //        output = memoryStream.ToArray();
        //    }

        //    return output;
        //}

        //private int indexOfNextLineBreak(byte[] data, int offset)
        //{
        //    int nextCR;
        //    int nextLF;

        //    do
        //    {
        //        nextCR = Array.IndexOf(data, CHAR_CR, offset);
        //        nextLF = Array.IndexOf(data, CHAR_LF, nextCR);
        //        offset = nextCR + 2;
        //    }
        //    while (offset < data.Length && nextCR + 1 != nextLF);

        //    return (offset < data.Length) ? nextCR : -1;
        //}

        #endregion Methods
    }
}
