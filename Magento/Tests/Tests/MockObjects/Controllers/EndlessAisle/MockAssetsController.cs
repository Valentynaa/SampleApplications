using MagentoSync.Controllers.EndlessAisle;
using MagentoSync.Models.EndlessAisle.ProductLibrary;
using MagentoSync.Models.EndlessAisle.ProductLibrary.Projections;
using System;

namespace Tests.MockObjects.Controllers.EndlessAisle
{
	public class MockAssetsController : BaseMockController, IAssetsController
	{
		public AssetResource CreateAsset(string path)
		{
			return new AssetResource()
			{
				Id = new Guid(),
				IsHidden = false,
				MimeType = "image/jpeg",
				Name = "TestImage.jpg"
			};
		}

		public AssetResponse SetHeroShot(string slug, AssetResponse heroShotAsset)
		{
			return new AssetResponse()
			{
				assetId = heroShotAsset.assetId
			};
		}

		public AssetResource GetAsset(string assetId)
		{
			return new AssetResource()
			{
				Id = new Guid(assetId),
				IsHidden = false,
				MimeType = "image/jpeg",
				Name = "TestImage.jpg"
			};
		}
	}
}
