using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MagentoSync;
using MagentoSync.Controllers.EndlessAisle;
using MagentoSync.Models.EndlessAisle.ProductLibrary.Projections;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Controllers.EndlessAisle
{
	/// <summary>
	/// This suite ensures the AssetsController is working correctly
	/// </summary>
	[TestClass]
	public class AssetsControllerTests
	{
		//IMPORTANT: Before you can run these tests, ensure the values below are replaced with ones from Endless Aisle
		private AssetsController _assetsController;
		private const string AssetPath = "C:\\RQ\\MagentoSync\\Tests\\Tests\\Resources\\TestImage.jpg";
		private const string AssetId = "f1fca075-464d-45c7-bc73-77aa570ffecc";
		private const string Slug = "M2039";

		[TestInitialize]
		public void SetUp()
		{
			var eaAuthToken = App.GetEaAuthToken();
			_assetsController = new AssetsController(eaAuthToken);
		}

		/// <summary>
		/// If this test fails, an issue occured while creating the asset.
		/// Uses path to create asset.
		/// </summary>
		[TestMethod]
		public void AssetController_CreateAsset()
		{
			Assert.IsNotNull(_assetsController.CreateAsset(AssetPath));
		}

		/// <summary>
		/// If this test fails, the Asset returned did not match the asset desired
		/// </summary>
		[TestMethod]
		public void AssetController_GetAsset()
		{
			Assert.AreEqual(AssetId, _assetsController.GetAsset(AssetId).Id.ToString());
		}

		/// <summary>
		/// If this test fails, there is an issue when setting the hero shot to the desired Asset
		/// </summary>
		[TestMethod]
		public void AssetController_SetHeroShot()
		{
			Assert.AreEqual(AssetId, _assetsController.SetHeroShot(Slug, new AssetResponse() {assetId = new Guid(AssetId)}).assetId.ToString());
		}
	}
}
