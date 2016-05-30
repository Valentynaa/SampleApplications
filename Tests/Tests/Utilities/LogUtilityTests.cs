using System;
using MagentoConnect.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Utilities
{
    /// <summary>
    /// This test suite ensures the Logging utility is working
    /// </summary>
    [TestClass]
	public class LogUtilityTests
	{
		/// <summary>
		/// If this test fails, the message was unable to be written to the log files specified
		/// </summary>
		[TestMethod]
		public void LogWriter_Write()
		{
			LogUtility.Write(Log.ProductSync, "Test");
			LogUtility.Write(Log.Error, "Test");
		}

		/// <summary>
		/// This test ensures that the time of loggings can be extracted from the log file
		/// </summary>
		[TestMethod]
		public void LogWriter_TryGetLastLog()
		{
			LogUtility.Write(Log.OrderSync, "Test");
			DateTime result;
			Assert.IsTrue(LogUtility.TryGetLastLog(Log.OrderSync, out result));
			Assert.IsTrue(result.AddMinutes(-1) < result);
		}
	}
}
