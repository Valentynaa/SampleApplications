using System;
using MagentoSync.Models.EndlessAisle.ProductLibrary;
using MagentoSync.Models.EndlessAisle.ProductLibrary.Projections;
using Newtonsoft.Json;
using RestSharp;
using System.IO;

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
		
		/**
		 * Sets the hero shot for a product
		 * 
		 * @param   slug            Identifier for a product in EA
		 * @param   heroShotAsset   Object representing asset to become to new hero shot for the product  
		 *
		 * @return  AssetResource   Created asset
		 */
		public AssetResponse SetHeroShot(string slug, AssetResponse heroShotAsset)
		{
			var endpoint = UrlFormatter.EndlessAisleSetHeroShotUrl(slug);

			var client = new RestClient(endpoint);
			var request = new RestRequest(Method.PUT);

			request.AddHeader("Authorization", string.Format("Bearer {0}", AuthToken));
			request.AddHeader("Accept", "application/json");
			request.AddHeader("Content-Type", "application/json");

			request.AddJsonBody(heroShotAsset);

			var response = client.Execute(request);

			//Ensure we get the right code
			//CheckStatusCode(response.StatusCode); <-- commented out for now because the API has a bug

			return JsonConvert.DeserializeObject<AssetResponse>(response.Content);
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
