using System;
using System.Diagnostics;
using System.IO;

namespace VideoPlayerApplication.StatusLogging
{

    #pragma warning disable CS8618
    public class Logger : IDisposable
    {
        public readonly Logger Log;

        private static Logger? m_inst;
        public static Logger? Instance
        {
            get => m_inst ?? (m_inst = new Logger());

            private set => m_inst = value;
        }

        private static readonly string EntrySeparator = @"!-!";
        private static string? m_logFilePath;
        private static readonly int ProcessId = Process.GetCurrentProcess().Id;

        private Logger()
        {
            var fileName = "MediaPlayer";
            m_logFilePath = Environment.CurrentDirectory + $"\\{fileName}App.log";
            try
            {
                if (File.Exists(m_logFilePath))
                {
                    string logFileBackup = Environment.CurrentDirectory + $"\\{fileName}App.bak";
                    if (File.Exists(logFileBackup))
                    {
                        File.Delete(logFileBackup);
                    }
                    File.Move(m_logFilePath, logFileBackup);
                }
            }
            catch
            {
                // Ignore. Not all that worried if a log backup is not created. 
            }
        }

        public virtual void AddEntry(string pEntryText)
        {
            // The lock is for thread safety. 
            if (m_logFilePath != null)
                lock (m_logFilePath)
                {
                    WriteToLogFile(m_logFilePath, pEntryText);
                }
        }
      
        /// <summary>
        /// Write an entry to the log file. 
        /// </summary>
        /// <param name="pLogFilePath">The path of the log file.</param>
        /// <param name="pEntryText">Log string value.</param>
        public static void WriteToLogFile(string pLogFilePath, string pEntryText)
        {
            StreamWriter fileWriter = File.AppendText(pLogFilePath);
            // Note: The process ID is used to allow sorting a log file in the rare case that the exe is run simultaneously in the same directory. 
            // To enable consistent entry widths: 'm_processId:D6' zero pads the PID to 6 chars and ',p_EntryLevel,-11' left justifies and pads the level.   
            fileWriter.WriteLine($@"{DateTime.UtcNow}{EntrySeparator}{ProcessId:D6}{EntrySeparator}{pEntryText}");
            fileWriter.Close();
        }

        public void Dispose()
        {
            Instance = null;
        }
    }
}
