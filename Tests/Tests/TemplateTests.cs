using System.Linq;
using MagentoConnect;
using MagentoConnect.Controllers.Magento;
using MagentoConnect.Models.Magento.Products;
using MagentoConnect.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;

//Adjust Namespace Accordingly
namespace Tests
{
	[TestClass]
	//Classname: "Class Tested" + Tests
	public class TemplateTests
	{
		//Private variables go here
		private bool _test = true;

		[TestInitialize]
		public void SetUp()
		{

		}

		//Tests go here

		/// <summary>
		/// Test Description
		/// ex. If this test fails, your product does not have an image
		/// </summary>
		[TestMethod]
		public void TestName()
		{
			Assert.IsTrue(_test);
		}
	}
}
