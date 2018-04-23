using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AirTrafficMonitor.Events;
using AirTrafficMonitor.Interfaces;

namespace AirTrafficMonitor.Classes
{
    public class ConsoleOutput : IOutput
    {
        private const int TableWidthConsole = 176;
        
        public void OutputString(string str)
        {
            Console.WriteLine(str);
        }

        public void OutputDictionary(Dictionary<string, ITrack> trackDict)
        {
            CleanUp();

            string timestamp = "";

            OutputTableSeparator();
            OutputTableRow("Flight no.", "Altitude", "Velocity", "Course", "Position x,y", "Separate with", "Separate date", "Separate time");
            OutputTableSeparator();

            foreach (var track in trackDict)
            {
                string separationString = "";
                string separationTimestampDate;
                string separationTimestampTime;

                foreach (var separation in track.Value.SeparationTrackList)
                    separationString += separation.Tag + ",";

                if (track.Value.SeparationTimestamp != DateTime.MinValue)
                {
                    separationTimestampDate = track.Value.SeparationTimestamp.Date.ToString("dd/MM/yyyy");
                    separationTimestampTime = track.Value.SeparationTimestamp.ToString("HH:mm:ss");
                }
                else
                {
                    separationTimestampDate = "";
                    separationTimestampTime = "";
                }

                var latestTrack = track.Value;

                if (track.Value.UpdateTimestamp != DateTime.MinValue)
                    timestamp = latestTrack.UpdateTimestamp.Date.ToString("dd/MM/yyyy") + " " + latestTrack.UpdateTimestamp.ToString("HH:mm:ss");
                else
                    timestamp = "";

                OutputTableRow(
                    track.Value.Tag, 
                    track.Value.Altitude + " m.",
                    track.Value.Velocity.ToString("0.##") + " m/s",
                    track.Value.Course.ToString("0.##") + " °",
                    track.Value.CoordinateX + "," + track.Value.CoordinateY,
                    separationString,
                    separationTimestampDate,
                    separationTimestampTime
                    );
            }

            OutputTableSeparator();
            OutputTableRow("Latest update: " + timestamp);
            OutputTableSeparator();
        }

        public void OutputSeparationEvents(Dictionary<string, ITrack> trackDict)
        {
            throw new NotImplementedException();
        }

        public void CleanUp()
        {
            Console.Clear();
        }

        private static void OutputTableSeparator()
        {
            Console.WriteLine(new string('-', TableWidthConsole));
        }

        private static void OutputTableRow(params string[] columns)
        {
            int width = (TableWidthConsole - columns.Length) / columns.Length;
            string row = "|";

            foreach (string column in columns)
            {
                row += TableCellAlignCentre(column, width) + "|";
            }

            Console.WriteLine(row);
        }

        private static string TableCellAlignCentre(string text, int width)
        {
            text = text.Length > width ? text.Substring(0, width - 3) + "..." : text;
            return String.IsNullOrEmpty(text) ? new string(' ', width) : text.PadRight(width - (width - text.Length) / 2).PadLeft(width);
        }
    }
}
