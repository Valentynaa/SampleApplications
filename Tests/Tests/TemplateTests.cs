using Microsoft.VisualStudio.TestTools.UnitTesting;

//Adjust Namespace Accordingly
namespace Tests
{
    /// <summary>
    /// Purpose of this test suite
    /// </summary>
	[TestClass]
	//Classname: "Class Tested" + Tests
	public class TemplateTests
	{
	    private const bool Test = true;

	    [TestInitialize]
		public void SetUp()
		{

		}

		/// <summary>
		/// Test Description. This test should describe the situation you are attempting to detect or resolve
		/// ex. If this test fails, your product does not have an image
		/// </summary>
		[TestMethod]
		public void TestName()
		{
			Assert.IsTrue(Test);
		}
	}
}
