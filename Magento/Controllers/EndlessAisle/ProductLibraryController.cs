using System.Net;
using MagentoSync.Controllers.EndlessAisle.Interfaces;
using MagentoSync.Models.EndlessAisle.ProductLibrary;
using Newtonsoft.Json;
using RestSharp;
using MagentoSync.Models.EndlessAisle.ProductLibrary.Projections;

namespace MagentoSync.Controllers.EndlessAisle
{
	public class ProductLibraryController : BaseController, IProductLibraryController
	{
		public string AuthToken { get; }

		public ProductLibraryController(string eaAuthToken)
		{
			AuthToken = eaAuthToken;
		}

		/**
		 * Get a product using an endpoint that takes product details and combines it into a grouped view
		 * 
		 * @param   slug                Identifier for a Product in EA
		 *
		 * @returns ProductResource     Product 
		 */
		public ProductDetailsResource GetProductBySlug(string slug)
		{
			var endpoint = UrlFormatter.EndlessAisleGetProductBySlugUrl(slug);

			var client = new RestClient(endpoint);
			var request = new RestRequest(Method.GET);

			request.AddHeader("Authorization", string.Format("Bearer {0}", AuthToken));
			request.AddHeader("Accept", "application/json");
		   
			var response = client.Execute(request);

			//Ensure we get the right code
			CheckStatusCode(response.StatusCode);

			return JsonConvert.DeserializeObject<ProductDetailsResource>(response.Content);
		}

		/**
		 * Get color definitions for a product
		 * 
		 * @param       productDocumentId           Identifier for a Product in EA
		 *
		 * @returns     ColorDefinitionsResource    Color definitions for product 
		 */
		public ColorDefinitionsResource GetColorDefinitions(int productDocumentId)
		{
			var endpoint = UrlFormatter.EndlessAisleCreateColorDefinition(productDocumentId);

			var client = new RestClient(endpoint);
			var request = new RestRequest(Method.GET);

			request.AddHeader("Authorization", string.Format("Bearer {0}", AuthToken));
			request.AddHeader("Accept", "application/json");

			var response = client.Execute(request);

			//Ensure we get the right code
			CheckStatusCode(response.StatusCode);

			return JsonConvert.DeserializeObject<ColorDefinitionsResource>(response.Content);
		}

		/**
		* Gets list of color tags in EA
		* 
		* @return  ColorTagsResource    Color Tags
		*/
		public ColorTagsResource GetColorTags()
		{
			var endpoint = UrlFormatter.EndlessAisleColorTagsUrl();

			var client = new RestClient(endpoint);
			var request = new RestRequest(Method.GET);

			request.AddHeader("Authorization", string.Format("Bearer {0}", AuthToken));
			request.AddHeader("Accept", "application/json");

			var response = client.Execute(request);

			//Ensure we get the right code
			CheckStatusCode(response.StatusCode);

			return JsonConvert.DeserializeObject<ColorTagsResource>(response.Content);
		}
	
		/**
		 * Gets a product document, which contains the Master Product and all child variations/revisions
		 *
		 * @param   productDocumentId           Identifier of a Product 
		 * 
		 * @return  ProductDocumentResource     Retrieved product document
		 */
		public ProductDocumentResource GetProductHierarchy(int productDocumentId)
		{
			var endpoint = UrlFormatter.EndlessAisleGetProductUrl(productDocumentId);

			var client = new RestClient(endpoint);
			var request = new RestRequest(Method.GET);

			request.AddHeader("Authorization", string.Format("Bearer {0}", AuthToken));
			request.AddHeader("Accept", "application/json");
			request.AddHeader("Content-Type", "application/json");

			var response = client.Execute(request);

			//Ensure we get the right code
			CheckStatusCode(response.StatusCode);

			return JsonConvert.DeserializeObject<ProductDocumentResource>(response.Content);
		}

		/**
		 * Updates a Variation
		 *
		 * @param   productDocumentId   Identifier for a product document
		 * @param   variationId         Identifier for a Variation
		 * @param   product             Object representing Master product to be updated
		 *
		 * @return  variationId         Identifier of updated Variation
		 */
		public int UpdateVariation(int productDocumentId, int variationId, RevisionEditResource product)
		{
			var endpoint = UrlFormatter.EndlessAisleUpdateVariationUrl(productDocumentId, variationId);

			var client = new RestClient(endpoint);
			var request = new RestRequest(Method.PUT);

			request.AddHeader("Authorization", string.Format("Bearer {0}", AuthToken));
			request.AddHeader("Accept", "application/json");
			request.AddHeader("Content-Type", "application/json");

			request.AddJsonBody(product);

			var response = client.Execute(request);

			//Ensure we get the right code
			CheckStatusCode(response.StatusCode, HttpStatusCode.NoContent);

			return variationId;
		}

		/**
		 * Creates a color definition
		 *
		 * @param   ColorDefinitionsResource       Object representing color def to be created
		 * @return  ColorDefinitionsResource       ColorDefinitionsResource that was created, if sucessful
		 */
		public ColorDefinitionsResource CreateColorDefinition(int productDocumentId, ColorDefinitionsResource colorDefs)
		{
			var endpoint = UrlFormatter.EndlessAisleCreateColorDefinition(productDocumentId);

			var client = new RestClient(endpoint);
			var request = new RestRequest(Method.POST);

			request.AddHeader("Authorization", string.Format("Bearer {0}", AuthToken));
			request.AddHeader("Accept", "application/json");
			request.AddHeader("Content-Type", "application/json");

			request.AddJsonBody(colorDefs);

			var response = client.Execute(request);

			//Ensure we get the right code
			CheckStatusCode(response.StatusCode);

			return JsonConvert.DeserializeObject<ColorDefinitionsResource>(response.Content);
		}

		/**
		 * Creates a Master Product
		 *
		 * @param   ProductDocumentResource       Object representing Master product to be created
		 * @return  ProductDocumentResource       ProductDocumentResource that was created, if sucessful
		 */
		public ProductDocumentResource CreateMasterProduct(ProductDocumentResource product)
		{
			var endpoint = UrlFormatter.EndlessAisleCreateProductUrl();

			var client = new RestClient(endpoint);
			var request = new RestRequest(Method.POST);

			request.AddHeader("Authorization", string.Format("Bearer {0}", AuthToken));
			request.AddHeader("Accept", "application/json");
			request.AddHeader("Content-Type", "application/json");

			request.AddJsonBody(product);

			var response = client.Execute(request);

			//Ensure we get the right code
			CheckStatusCode(response.StatusCode, HttpStatusCode.Created);

			return JsonConvert.DeserializeObject<ProductDocumentResource>(response.Content);
		}

		/**
		 * Updates a Master Product
		 *
		 * @param   productDocumentId   Identifier for a product document
		 * @param   product             Object representing Master product to be updated
		 *
		 * @return  int                 Identifier of product document that was updated
		 */
		public int UpdateMasterProduct(int productDocumentId, RevisionEditResource product)
		{
			var endpoint = UrlFormatter.EndlessAisleGetProductUrl(productDocumentId);

			var client = new RestClient(endpoint);
			var request = new RestRequest(Method.PUT);

			request.AddHeader("Authorization", string.Format("Bearer {0}", AuthToken));
			request.AddHeader("Accept", "application/json");
			request.AddHeader("Content-Type", "application/json");

			request.AddJsonBody(product);

			var response = client.Execute(request);

			//Ensure we get the right code
			CheckStatusCode(response.StatusCode, HttpStatusCode.NoContent);

			return productDocumentId;
		}

		/**
		 * Creates a Variation
		 *
		 * @param   productDocumentId       Identifier of a Product Document this Variation will be a child of
		 * @param   variation               Object representing Variation to be created
		 * 
		 * @return  int                     Identifier of created Variation
		 */
		public CreatedVariationResource CreateVariation(int productDocumentId, VariationResource variation)
		{
			var endpoint = UrlFormatter.EndlessAisleCreateVariationUrl(productDocumentId);

			var client = new RestClient(endpoint);
			var request = new RestRequest(Method.POST);

			request.AddHeader("Authorization", string.Format("Bearer {0}", AuthToken));
			request.AddHeader("Accept", "application/json");
			request.AddHeader("Content-Type", "application/json");

			request.AddJsonBody(variation);

			var response = client.Execute(request);

			//Ensure we get the right code
			CheckStatusCode(response.StatusCode, HttpStatusCode.Created);

			return JsonConvert.DeserializeObject<CreatedVariationResource>(response.Content);
		}
	}
}
