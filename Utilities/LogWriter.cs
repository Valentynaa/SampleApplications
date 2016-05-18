using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MagentoConnect.Utilities
{
	public static class LogWriter
	{
		/// <summary>
		/// Writes the message to a log file.
		/// </summary>
		/// <param name="message">Message to write</param>
		/// <param name="logType">Type of log to write to</param>
		public static void Write(string message, Log logType)
		{
			try
			{
				using (StreamWriter streamWriter = File.AppendText(Enums.GetPath(logType)))
				{
					streamWriter.WriteLine("-------------------------------");
					streamWriter.WriteLine("{0}| {1}", DateTime.Now, message);
				}
			}
			catch (Exception)
			{
				if (logType != Log.Error)
					Write(string.Format("Error writing {0} to {1}", message, Enums.GetPath(logType)), Log.Error);
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

			string lastLog = File.ReadLines(Enums.GetPath(logType)).LastOrDefault();

			if (string.IsNullOrEmpty(lastLog))
			{
				lastLogTime = DateTime.MinValue;
				return false;
			}
			bool result = DateTime.TryParse(lastLog.Substring(0, lastLog.IndexOf("|", StringComparison.Ordinal)), out lastLogTime);

			//Don't convert to UTC if the result was false because DateTime.MinValue is changed when you call ToUniversalTime()
			if (result == false)
				return false;

			lastLogTime = lastLogTime.ToUniversalTime();
			return true;
		}
	}
}
