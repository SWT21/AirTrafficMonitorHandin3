using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AirTrafficMonitor.Interfaces;

namespace AirTrafficMonitor.Classes
{
    public class LogfileOutput : IOutput
    {
        private readonly string _eventLogDirectory;
        private readonly string _eventLogDirectoryArchive;
        private readonly string _generalLogfileName;
        private readonly string _separationLogfileName;
        private readonly string _logFileType;
        private readonly string _filepathGeneralLogfile;
        private readonly string _filepathSeparationLogfile;
        private const int TableWidthGeneralLogfile = 110;
        private const int TableWidthSeparationLogfile = 112;
        private readonly List<ITrack> _separationEventList;
        
        public LogfileOutput()
        {
            _eventLogDirectory = @"..\..\..\EventLog\";
            _eventLogDirectoryArchive = @"..\..\..\EventLog\LogArchive\";
            _generalLogfileName = "GeneralLogfile";
            _separationLogfileName = "SeparationLogfile";
            _logFileType = ".txt";
            _filepathGeneralLogfile = _eventLogDirectory + _generalLogfileName + _logFileType;
            _filepathSeparationLogfile = _eventLogDirectory + _separationLogfileName + _logFileType;

            _separationEventList = new List<ITrack>();
        }

        ~LogfileOutput()
        {
            CleanUp();
        }

        public void OutputString(string str)
        {
            File.WriteAllText(_filepathGeneralLogfile, str);
        }

        public void OutputDictionary(Dictionary<string, ITrack> trackDict)
        {
            using (StreamWriter sw = File.CreateText(_filepathGeneralLogfile))
            {
                string timestamp = "";

                OutputTableSeparator(sw, TableWidthGeneralLogfile);
                OutputTableRow(sw, TableWidthGeneralLogfile, "Flight no.", "Altitude", "Velocity", "Course", "Position x,y");
                OutputTableSeparator(sw, TableWidthGeneralLogfile);

                foreach (var track in trackDict)
                {
                    var latestTrack = track.Value;

                    if (track.Value.UpdateTimestamp != DateTime.MinValue)
                        timestamp = latestTrack.UpdateTimestamp.Date.ToString("dd/MM/yyyy") + " " + latestTrack.UpdateTimestamp.ToString("HH:mm:ss");
                    else
                        timestamp = "";

                    OutputTableRow(
                        sw,
                        TableWidthGeneralLogfile,
                        track.Value.Tag, track.Value.Altitude + " m.",
                        track.Value.Velocity.ToString("0.##") + " m/s",
                        track.Value.Course.ToString("0.##") + " °",
                        track.Value.CoordinateX + "," + track.Value.CoordinateY
                        );
                }

                OutputTableSeparator(sw, TableWidthGeneralLogfile);
                OutputTableRow(sw, TableWidthGeneralLogfile, "Latest update: " + timestamp);
                OutputTableSeparator(sw, TableWidthGeneralLogfile);
            }
        }

        public void OutputSeparationEvents(Dictionary<string, ITrack> trackDict)
        {
            if (!File.Exists(_filepathSeparationLogfile))
            {
                using (StreamWriter sw = File.CreateText(_filepathSeparationLogfile))
                {
                }
            }

            using (StreamWriter sw = File.AppendText(_filepathSeparationLogfile))
            {
                if (new FileInfo(_filepathSeparationLogfile).Length == 0)
                {
                    OutputTableSeparator(sw, TableWidthSeparationLogfile);
                    OutputTableRow(sw, TableWidthSeparationLogfile, "Flight no.", "Separate with", "Separate date", "Separate time");
                    OutputTableSeparator(sw, TableWidthSeparationLogfile);
                }

                foreach (var track in trackDict)
                {
                    if (track.Value.SeparationTrackList.Count == 0) continue;

                    if (_separationEventList.Contains(track.Value))
                        _separationEventList.Remove(track.Value);

                    _separationEventList.Add(track.Value);
                }

                foreach (var separstionEvent in _separationEventList)
                {
                    if (separstionEvent.SeparationTrackList.Count == 0) continue;
                    if (!separstionEvent.IsSeparationTrackListChanged) continue;

                    string separationString = "";
                    string separationTimestampDate = "";
                    string separationTimestampTime = "";

                    foreach (var separation in separstionEvent.SeparationTrackList)
                    {
                        separationString += separation.Tag + ",";
                        separationTimestampDate = separation.SeparationTimestamp.Date.ToString("dd/MM/yyyy");
                        separationTimestampTime = separation.SeparationTimestamp.ToString("HH:mm:ss");
                    }

                    OutputTableRow(
                        sw,
                        TableWidthSeparationLogfile,
                        separstionEvent.Tag,
                        separationString,
                        separationTimestampDate,
                        separationTimestampTime
                    );
                        
                    separstionEvent.IsSeparationTrackListChanged = false;
                }
            }
        }

        public void CleanUp()
        {
            if (File.Exists(_filepathSeparationLogfile))
                File.Move(_filepathSeparationLogfile,
                    _eventLogDirectoryArchive + 
                    DateTime.Now.ToString("dd-MM-yyyy-HH-mm-ss-") + 
                    _separationLogfileName +
                    _logFileType);

            if (File.Exists(_filepathGeneralLogfile))
                File.Move(_filepathGeneralLogfile,
                    _eventLogDirectoryArchive + 
                    DateTime.Now.ToString("dd-MM-yyyy-HH-mm-ss-") + 
                    _generalLogfileName +
                    _logFileType);
        }

        private static void OutputTableSeparator(StreamWriter sw, int tableWidth)
        {
            sw.WriteLine(new string('-', tableWidth));
        }

        private static void OutputTableRow(StreamWriter sw, int tableWidth, params string[] columns)
        {
            int width = (tableWidth - columns.Length) / columns.Length;
            string row = "|";

            foreach (string column in columns)
            {
                row += TableCellAlignCentre(column, width) + "|";
            }

            sw.WriteLine(row);
        }

        private static string TableCellAlignCentre(string text, int width)
        {
            text = text.Length > width ? text.Substring(0, width - 3) + "..." : text;
            return String.IsNullOrEmpty(text) ? new string(' ', width) : text.PadRight(width - (width - text.Length) / 2).PadLeft(width);
        }
    }
}
