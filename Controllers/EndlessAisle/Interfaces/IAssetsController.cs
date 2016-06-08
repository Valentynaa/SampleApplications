using MagentoConnect.Models.EndlessAisle.ProductLibrary;
using MagentoConnect.Models.EndlessAisle.ProductLibrary.Projections;

namespace MagentoConnect.Controllers.EndlessAisle
{
	public interface IAssetsController : IController
	{
		AssetResource CreateAsset(string path);
		AssetResponse SetHeroShot(string slug, AssetResponse heroShotAsset);

		/// <summary>
		/// Gets an EA asset for an asset identifier
		/// </summary>
		/// <param name="assetId">Asset identifier</param>
		/// <returns>AssetResource with specified identifier</returns>
		AssetResource GetAsset(string assetId);
	}
}