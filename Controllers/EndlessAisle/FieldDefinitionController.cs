using System.Collections.Generic;
using MagentoConnect.Models.EndlessAisle.FieldDefinitions;
using Newtonsoft.Json;
using RestSharp;

namespace MagentoConnect.Controllers.EndlessAisle
{
	public class FieldDefinitionController : BaseController, IFieldDefinitionController
	{
		public string AuthToken { get; }

		public FieldDefinitionController(string eaAuthToken)
		{
			AuthToken = eaAuthToken;
		}

		/**
		 * Gets a field definition
		 *
		 * @param   fieldDefinitionId           Identifier of a field definition
		 *
		 * @return  FieldDefinitionResource     Returns a field definition
		 */
		public FieldDefinitionResource GetAFieldDefinition(int fieldDefinitionId)
		{
			var endpoint = UrlFormatter.EndlessAisleFieldDefinitionUrl(fieldDefinitionId);

			var client = new RestClient(endpoint);
			var request = new RestRequest(Method.GET);

			request.AddHeader("Authorization", string.Format("Bearer {0}", AuthToken));
			request.AddHeader("Accept", "application/json");

			var response = client.Execute(request);

			//Ensure we get the right code
			CheckStatusCode(response.StatusCode);

			return JsonConvert.DeserializeObject<FieldDefinitionResource>(response.Content);
		}

		/**
		 * Gets information about all field definitions
		 *
		 * @return  List<FieldDefinitionResource>     Returns a list of field definitions
		 */
		public List<FieldDefinitionResource> GetAllFieldDefinitions()
		{
			var endpoint = UrlFormatter.EndlessAisleFieldDefinitionsUrl();

			var client = new RestClient(endpoint);
			var request = new RestRequest(Method.GET);

			request.AddHeader("Authorization", string.Format("Bearer {0}", AuthToken));
			request.AddHeader("Accept", "application/json");

			var response = client.Execute(request);

			//Ensure we get the right code
			CheckStatusCode(response.StatusCode);

			return JsonConvert.DeserializeObject<List<FieldDefinitionResource>>(response.Content);
		}       
	}
}
