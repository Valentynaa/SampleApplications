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
	public class LogUtilityTests
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
