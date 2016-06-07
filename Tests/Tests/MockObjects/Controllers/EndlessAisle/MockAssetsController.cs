using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MagentoConnect.Controllers.EndlessAisle;
using MagentoConnect.Models.EndlessAisle.ProductLibrary;
using MagentoConnect.Models.EndlessAisle.ProductLibrary.Projections;

namespace Tests.MockObjects.Controllers.EndlessAisle
{
	public class MockAssetsController : IAssetsController
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

		public string AuthToken
		{
			get
			{
				return "WHdvc0h9b3NYQW9zWHZvc1h2JSYBAyc_PhRWBSAYLUFsIBkSMEA9MRYVWAcsPCQQaD8NJiBPLSUeFVgXGjEBJA4bX0EaBBtAOR8I";
			}
		}
	}
}
