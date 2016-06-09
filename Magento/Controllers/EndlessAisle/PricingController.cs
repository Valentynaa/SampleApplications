using System;
using MagentoSync.Models.EndlessAisle.Pricing;
using MagentoSync.Utilities;
using Newtonsoft.Json;
using RestSharp;

namespace MagentoSync.Controllers.EndlessAisle
{
	public class PricingController : BaseController, IPricingController
	{
		public string AuthToken { get; }

		public PricingController(string eaAuthToken)
		{
			AuthToken = eaAuthToken;
		}

		/// <summary>
		/// Creates a pricing resource for EA.
		/// </summary>
		/// <param name="regularPrice">Price for the item to be set to.</param>
		/// <param name="catalogItemId">Catalog item to set price for.</param>
		/// <param name="pricing">Object representing Pricing to be created.</param>
		/// <param name="discountable">OPTIONAL: Sets the product as discountable or not.</param>
		/// <returns>Pricing resource created.</returns>
		public PricingResource CreatePricingResourceItem(PricingResource pricing, string catalogItemId, decimal regularPrice, bool discountable = false)
		{
			//Required fields for the request
			pricing.RegularPrice = regularPrice;
			pricing.CatalogItemId = new Guid(catalogItemId);
			pricing.CompanyId = ConfigReader.EaCompanyId;
			pricing.EntityId = ConfigReader.EaCompanyId;
			pricing.IsDiscountable = discountable;

			var endpoint = UrlFormatter.EndlessAisleCreatePricingUrl();

			var client = new RestClient(endpoint);
			var request = new RestRequest(Method.POST);

			request.AddHeader("Authorization", string.Format("Bearer {0}", AuthToken));
			request.AddHeader("Accept", "application/json");
			request.AddHeader("Content-Type", "application/json");

			request.AddJsonBody(pricing);

			var response = client.Execute(request);

			//Ensure we get the right code
			CheckStatusCode(response.StatusCode, System.Net.HttpStatusCode.Created);

			return JsonConvert.DeserializeObject<PricingResource>(response.Content);
		}
	}
}
