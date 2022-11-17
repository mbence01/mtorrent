using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Torrent.Common.Model.Exception;

namespace Torrent.Common.Logging
{
    public enum Log
    {
        Error,
        Warning,
        Info
    }

    public class Logger
    {
        private Log _logType;
        private Object _class;
        private string _method;
        private Exception _exception;
        private List<string> _userDefinedMessages;

        private readonly string         _logBaseDir;
        private readonly bool           _separateDifferentLogTypes;
        private readonly FolderDepth    _separateLogTypesFolderDepth;
        private readonly string         _dateFolderFormat;
        private readonly string         _timestampFormat;
        private readonly string         _logFilenameFormat;

        private Logger() 
        {
            if (!LogConfig.LoggerEnabled)
                throw new LoggerException();

            _userDefinedMessages = new List<string>();

            _logBaseDir = LogConfig.LogBaseDir;
            _separateDifferentLogTypes = LogConfig.SeparateDifferentLogTypes;
            _separateLogTypesFolderDepth = LogConfig.SeparateLogTypesFolderDepth;
            _dateFolderFormat = LogConfig.DateFolderFormat;
            _timestampFormat = LogConfig.TimestampFormat;
            _logFilenameFormat = LogConfig.LogFilenameFormat;
        }

        public static Logger As(Log type)
        {
            return new Logger
            {
                _logType = type
            };
        }

        public Logger From<T>(T cls)
        {
            _class = cls;
            return this;
        }

        public Logger Method(string methodName)
        {
            _method = methodName;
            return this;
        }

        public Logger Exception(Exception ex)
        {
            _exception = ex;
            return this;
        }

        public Logger Message(string message)
        {
            _userDefinedMessages.Add(message);
            return this;
        }

        /// <summary>
        /// Creates a list of lines based on the given information, then calls a function which will write all of the lines to a specified log file.
        /// </summary>
        /// <exception cref="LoggerException"></exception>
        public void Write()
        {
            string timestamp = $"[{DateTime.Now.ToString(_timestampFormat)}]";

            List<string> lines = new List<string>();

            if(_logType == Log.Error)
            {
                lines.Add($"{timestamp} Error occurred at {_class?.GetType()?.FullName}.{_method}...");
                lines.Add($"{timestamp} {_exception.GetType().Name} has been thrown. Message: {_exception.Message}, StackTrace: {_exception.StackTrace}");
                lines.Add($"{timestamp} TargetSite MethodName: {_exception.TargetSite?.Name}");

                Exception currEx = _exception;

                while(currEx.InnerException != null)
                {
                    currEx = currEx.InnerException;
                    lines.Add($"{timestamp} Inner exception ({currEx.GetType().Name}): {currEx.Message}");
                }

                lines.AddRange(_userDefinedMessages);
            }
            else
            {
                lines.AddRange(_userDefinedMessages.Select(msg => $"{timestamp} {_logType.ToString()}: "));
            }

            WriteToFile(lines);
        }

        /// <summary>
        /// Writes the given <paramref name="lines"/> to the log file.
        /// </summary>
        /// <param name="lines">Lines that will be written to the file</param>
        /// <exception cref="LoggerException"></exception>
        private void WriteToFile(List<string> lines)
        {
            string logPath = GetLogDirPath();
            string fileName = GetLogFileName();
            string fullPath = Path.Combine(logPath, fileName);

            try
            {
                if (!Directory.Exists(logPath))
                    Directory.CreateDirectory(logPath);

                using (StreamWriter writer = File.AppendText(fullPath))
                {
                    lines.ForEach(line => writer.WriteLine(line));
                }
            }
            catch (Exception ex) { throw new LoggerException(fullPath, ex); }
        }

        /// <summary>
        /// Returns the accurate path to the new log file based on the explicitly given folder information.
        /// </summary>
        /// <returns>The path</returns>
        private string GetLogDirPath()
        {
            string dateFolder = Path.Combine(DateTime.Now.ToString(_dateFolderFormat).Split('-'));

            
            if (!_separateDifferentLogTypes)
                return Path.Combine(_logBaseDir, dateFolder);

            
            switch(_separateLogTypesFolderDepth)
            {
                case FolderDepth.FirstDate:
                    return Path.Combine(_logBaseDir, dateFolder, _logType.ToString());
                
                case FolderDepth.FirstType:
                default:
                    return Path.Combine(_logBaseDir, _logType.ToString(), dateFolder);
            }
        }

        /// <summary>
        /// Returns the log filename based on the explicitly given format.
        /// </summary>
        /// <returns>The filename</returns>
        private string GetLogFileName()
        {
            return Path.GetFileNameWithoutExtension(_logFilenameFormat)
                            .Replace("ddd", DateTime.Now.Day.ToString("D2"))
                            .Replace("ttt", _logType.ToString().ToLower()) 
                 + Path.GetExtension(_logFilenameFormat);
        }
    }
}
