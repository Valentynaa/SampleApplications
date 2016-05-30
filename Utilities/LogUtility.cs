using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace MagentoConnect.Utilities
{
	public static class LogUtility
	{
		private const string LogSeparator = "-------------------------------";

		/// <summary>
		/// Writes the message to a log file.
		/// </summary>
		/// <param name="message">Message to write</param>
		/// <param name="logType">Type of log to write to</param>
		public static void Write(Log logType, string message)
		{
			try
			{
				using (var streamWriter = File.AppendText(Enums.GetPath(logType)))
				{
					streamWriter.WriteLine(LogSeparator);
					streamWriter.WriteLine("{0}| {1}", DateTime.Now.ToString(CultureInfo.CreateSpecificCulture("en-US")), message);
				}
			}
			catch (Exception)
			{
				if (logType != Log.Error)
					Write(Log.Error, string.Format("Error writing {0} to {1}", message, Enums.GetPath(logType)));
			}
		}

		/// <summary>
		/// Finds the last result of the specified log type and returns the DateTime value associated with that log.
		/// If no log is found, the value returned is DateTime.MinValue
		/// 
		/// NOTE:
		///		Time logged in log files is local time, while value is returned in UTC time
		/// </summary>
		/// <param name="logType">Type of log to search for</param>
		/// <param name="lastLogTime">Time of the last login UTC</param>
		/// <returns>Whether or not the last log could be found</returns>
		public static bool TryGetLastLog(Log logType, out DateTime lastLogTime)
		{
			//Create the log file if it does not exist
			if (!File.Exists(Enums.GetPath(logType)))
			{
				var stream = File.Create(Enums.GetPath(logType));
				stream.Close();
			}

			var lastLog = File.ReadLines(Enums.GetPath(logType)).LastOrDefault();

			if (string.IsNullOrEmpty(lastLog))
			{
				lastLogTime = DateTime.MinValue;
				return false;
			}
			var result = DateTime.TryParse(lastLog.Substring(0, lastLog.IndexOf("|", StringComparison.Ordinal)), out lastLogTime);

			//Don't convert to UTC if the result was false because DateTime.MinValue is changed when you call ToUniversalTime()
			if (result == false)
				return false;

			lastLogTime = lastLogTime.ToUniversalTime();
			return true;
		}

		/// <summary>
		/// Gets a DateTime value in the message of a log if that log matches the time provided.
		/// If the time provided does not match any log or if the log that matches that timestamp
		/// does not have a DateTime value in its message then DateTime.MinValue is returned.
		/// </summary>
		/// <param name="logType">Type of log</param>
		/// <param name="logTime">Time of log to search for</param>
		/// <param name="timeInformation">Time in message of log</param>
		/// <returns>If a time was found in the message of the log done at the time specified</returns>
		public static bool TryGetTimeInformationForLog(Log logType, DateTime logTime, out DateTime timeInformation)
		{
			//Create the log file if it does not exist
			if (!File.Exists(Enums.GetPath(logType)))
			{
				var stream = File.Create(Enums.GetPath(logType));
				stream.Close();
			}

			var logs = File.ReadLines(Enums.GetPath(logType)).ToList();
			var regex = new Regex(RegexPatterns.TimeStampPattern);
			
			foreach (var log in logs)
			{
				var matches = regex.Matches(log);

				//Ensure that there is a timestamp match in the log message
			    if (matches.Count <= 1 || DateTime.Parse(matches[0].Value) != logTime) continue;
			    timeInformation = DateTime.Parse(matches[1].Value);
			    return true;
			}
			timeInformation = DateTime.MinValue;
			return false;
		}
	}
}
