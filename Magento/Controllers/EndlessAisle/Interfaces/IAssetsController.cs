using MagentoSync.Models.EndlessAisle.ProductLibrary;
using MagentoSync.Models.EndlessAisle.ProductLibrary.Projections;

namespace MagentoSync.Controllers.EndlessAisle
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