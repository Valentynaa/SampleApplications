using System.Net;
using MagentoSync.Models.Magento.CustomAttributes;
using Newtonsoft.Json;
using RestSharp;

namespace MagentoSync.Controllers.Magento
{
	public class CustomAttributesController : BaseController, ICustomAttributesController
	{
		public string AuthToken { get; }

		public CustomAttributesController(string magentoAuthToken)
		{
			AuthToken = magentoAuthToken;
		}

		/**
		 * Returns information about a custom attribute, if found
		 *
		 * @param   attributeCode           attribute_code of the attribute, see Product response
		 *
		 * @return  CustomAttributeResource Custom Attribute if found, otherwise null
		 */
		public CustomAttributeResource GetCustomAttributeIfExists(string attributeCode)
		{
			var endpoint = UrlFormatter.MagentoCustomAttributeUrl(attributeCode);

			var client = new RestClient(endpoint);
			var request = new RestRequest(Method.GET);

			request.AddHeader("Authorization", string.Format("Bearer {0}", AuthToken));

			var response = client.Execute(request);

			return response.StatusCode == HttpStatusCode.NotFound ? null : JsonConvert.DeserializeObject<CustomAttributeResource>(response.Content);
		}

		/**
		 * Creates a custom attribute in the system to be used for mapping or other purposes
		 *
		 * @param   attributeCode           attribute_code value for the attribute, see Product response
		 * @param   inputType               Type of html element, see Magento docs for acceptable values (text, select, etc)
		 * @param   isRequired              A flag to indicate if this property will be required by products
		 * @param   label                   Descriptive value that will appear for attribute in admin console
		 *
		 * @return  CustomAttributeResource Custom Attribute created
		 */
		public CustomAttributeResource CreateCustomAttribute(string attributeCode, string inputType, bool isRequired, string label)
		{
			var endpoint = UrlFormatter.MagentoCreateCustomAttributeUrl();

			var client = new RestClient(endpoint);
			var request = new RestRequest(Method.POST);

			request.AddHeader("Authorization", string.Format("Bearer {0}", AuthToken));
			request.AddHeader("Content-Type", "application/json");

			var customAttribute = new CreateCustomAttributeResource
			{
				attribute =  new CustomAttributeResource
				{
					attribute_code = attributeCode,
					frontend_input = inputType,
					is_required = isRequired,
					default_frontend_label = label
				}
			 };

			request.AddJsonBody(customAttribute);

			var response = client.Execute(request);

			//Ensure we get the right code
			CheckStatusCode(response.StatusCode);

			return JsonConvert.DeserializeObject<CustomAttributeResource>(response.Content);

		}
	}
}
