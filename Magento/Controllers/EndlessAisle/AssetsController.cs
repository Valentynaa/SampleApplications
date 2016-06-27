using MagentoSync.Models.EndlessAisle.ProductLibrary;
using Newtonsoft.Json;
using RestSharp;
using System.IO;
using MagentoSync.Controllers.EndlessAisle.Interfaces;

namespace MagentoSync.Controllers.EndlessAisle
{
	public class AssetsController : BaseController, IAssetsController
	{
		public string AuthToken { get; }

		public AssetsController(string eaAuthToken)
		{
			AuthToken = eaAuthToken;
		}

		/**
		 * Creates an Asset by uploading an image
		 * 
		 * @param   string          Location of image
		 *
		 * @return  AssetResource   Created asset
		 */
		public AssetResource CreateAsset(string path)
		{
			var endpoint = UrlFormatter.EndlessAisleCreateAssetUrl();

			var client = new RestClient(endpoint);
			var request = new RestRequest(Method.POST);

			request.AddHeader("Authorization", string.Format("Bearer {0}", AuthToken));
			request.AddHeader("Accept", "application/json");
			request.AddHeader("Content-Type", "multipart/form-data");

			request.AddFile("Filename", File.ReadAllBytes(path), Path.GetFileName(path));

			var response = client.Execute(request);

			//Ensure we get the right code
			CheckStatusCode(response.StatusCode, System.Net.HttpStatusCode.Created);

			return JsonConvert.DeserializeObject<AssetResource>(response.Content);
		}

		/// <summary>
		/// Gets an EA asset for an asset identifier
		/// </summary>
		/// <param name="assetId">Asset identifier</param>
		/// <returns>AssetResource with specified identifier</returns>
		public AssetResource GetAsset(string assetId)
		{
			var endpoint = UrlFormatter.EndlessAisleGetAssetUrl(assetId);

			var client = new RestClient(endpoint);
			var request = new RestRequest(Method.GET);

			request.AddHeader("Authorization", string.Format("Bearer {0}", AuthToken));
			request.AddHeader("Accept", "application/json");
			request.AddHeader("Content-Type", "application/json");
			
			var response = client.Execute(request);

			//Ensure we get the right code
			CheckStatusCode(response.StatusCode); 

			return JsonConvert.DeserializeObject<AssetResource>(response.Content);
		}
	}
}
