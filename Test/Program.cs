using MT.Tools.ICalendar;
using System;
using System.IO;
using System.Linq;
using System.Text;

namespace Test
{
    public class Program
    {
        public static void Main(string[] args)
        {
            string file = "testdata\\Probenplan Orchester St. Pankratius Sommer 2019.ics";

            using (var stream = File.Open(file, FileMode.Open))
            {
                var serializer = new CalendarSerializer();
                var parsedData = serializer.Deserialize(stream);
            }
        }
    }
}
