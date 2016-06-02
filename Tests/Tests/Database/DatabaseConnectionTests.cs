using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MagentoConnect;
using MagentoConnect.Database;
using MagentoConnect.Mappers;
using MagentoConnect.Models.Magento.Products;
using MagentoConnect.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tests.Utilities;

namespace Tests.Database
{
	[TestClass]
	public class DatabaseConnectionTests
	{
		//IMPORTANT: Before you can run these tests, ensure the values below are replaced with ones from your Magento system. 
		private DatabaseConnection _connection;
		private MediaGalleryEntryResource _media;

		//MediaPath needs to exist in your database and match TestImage in Resources file
		private const string MediaPath = "/b/r/brand_new_2.jpg";
		private const string MediaType = "image";
		private const string ControlImagePath = "C:\\RQ\\MagentoConnect\\Tests\\Tests\\Resources\\TestImage.jpg";

		[TestInitialize]
		public void SetUp()
		{
			_connection = DatabaseConnection.Instance;
			_media = new MediaGalleryEntryResource
			{
				media_type = MediaType,
				file = MediaPath
			};
		}

		/// <summary>
		/// This test ensures that the media returned from the database is a match to the test image used
		/// </summary>
		[TestMethod]
		public void DatabaseConnection_GetMediaGalleryEntryFile()
		{
			Image expected = Image.FromFile(ControlImagePath);
			Image result = ImageUtility.ImageFromBytes(_connection.GetMediaGalleryEntryFile(_media));
			Assert.IsTrue(ImageUtility.AreEqual(expected, result));
		}

		/// <summary>
		/// This test ensures an exception is thrown when null is passed into GetMediaGalleryEntryFile
		/// </summary>
		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void DatabaseConnection_GetMediaGalleryEntryFile_NullEntry()
		{
			_connection.GetMediaGalleryEntryFile(null);
		}

		/// <summary>
		/// If this test fails, the configuration returned from the database is not the same as the setting set in App.config
		/// </summary>
		[TestMethod]
		public void DatabaseConnection_GetMediaStorageConfiguration()
		{
			Assert.AreEqual(ConfigReader.MagentoStorageConfiguration, _connection.GetMediaStorageConfiguration());
		}
	}
}
