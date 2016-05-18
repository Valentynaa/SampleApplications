using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MagentoConnect.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Utilities
{
	[TestClass]
	public class LogWriterTests
	{
		//Private variables go here

		[TestInitialize]
		public void SetUp()
		{
		}

		//Tests go here

		/// <summary>
		/// If this test fails, the message was unable to be written to the log files specified
		/// </summary>
		[TestMethod]
		public void LogWriter_Write()
		{
			LogWriter.Write("Test", Log.Sync);
			LogWriter.Write("Test", Log.Error);
		}

		/// <summary>
		/// This test ensures that the time of loggings can be extracted from the log file
		/// </summary>
		[TestMethod]
		public void LogWriter_TryGetLastLog()
		{
			LogWriter.Write("Test", Log.Sync);
			DateTime result;
			Assert.IsTrue(LogWriter.TryGetLastLog(Log.Sync, out result));
			Assert.IsTrue(result.AddMinutes(-1) < result);
		}
	}
}
