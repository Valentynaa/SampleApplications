using System;
using MagentoConnect.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Utilities
{
    /// <summary>
    /// This test suite ensures the Filter utility is working
    /// </summary>
	[TestClass]
	public class FilterTests
	{
		private DateTime _date;
		private UrlFormatter _formatter;

		[TestInitialize]
		public void SetUp()
		{
			_date = DateTime.MinValue;
			_formatter = new UrlFormatter();
		}

		/// <summary>
		/// If this test fails, the message was unable to be written to the log files specified
		/// </summary>
		[TestMethod]
		public void Filter_ToString_DateTime()
		{
			var f = new Filter("CreatedDateUtc", _date, FilterCondition.LessThanOrEqual);
			Assert.AreEqual("?$filter=CreatedDateUtc le datetime\'0001-01-01T00:00:00\'", _formatter.HypermediaFilterUrl(string.Empty, f));
		}

		/// <summary>
		/// This test ensures that the time of loggings can be extracted from the log file
		/// </summary>
		[TestMethod]
		public void Filter_ToString_Int()
		{
			var f = new Filter("OrderTypeId", 1, FilterCondition.GreaterThanOrEqual);
			Assert.AreEqual("?$filter=OrderTypeId ge 1", _formatter.HypermediaFilterUrl(string.Empty, f));
		}

		/// <summary>
		/// If this test fails, the message was unable to be written to the log files specified
		/// </summary>
		[TestMethod]
		public void Filter_ToString_String()
		{
			var f = new Filter("Name", "iPhone 5 Order", FilterCondition.GreaterThanOrEqual);
			Assert.AreEqual("?$filter=Name ge \'iPhone 5 Order\'", _formatter.HypermediaFilterUrl(string.Empty, f));
		}

		/// <summary>
		/// If this test fails, the message was unable to be written to the log files specified
		/// </summary>
		[TestMethod]
		public void Filter_ToString_MultipleFilters()
		{
			var f1 = new Filter("Name", "iPhone 5 Order", FilterCondition.GreaterThanOrEqual);
			var f2 = new Filter("OrderTypeId", 1, FilterCondition.GreaterThanOrEqual);
			var f3 = new Filter("CreatedDateUtc", _date, FilterCondition.LessThanOrEqual);
			Assert.AreEqual("?$filter=Name ge \'iPhone 5 Order\' and OrderTypeId ge 1 and CreatedDateUtc le datetime\'0001-01-01T00:00:00\'", _formatter.HypermediaFilterUrl(string.Empty, f1, f2, f3));
		}
	}
}
