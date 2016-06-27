using MagentoSync.Models.EndlessAisle.ProductLibrary;

namespace MagentoSync.Controllers.EndlessAisle.Interfaces
{
	public interface IAssetsController : IController
	{
		AssetResource CreateAsset(string path);

		/// <summary>
		/// Gets an EA asset for an asset identifier
		/// </summary>
		/// <param name="assetId">Asset identifier</param>
		/// <returns>AssetResource with specified identifier</returns>
		AssetResource GetAsset(string assetId);
	}
}