using System;
using System.Linq;
using MagentoConnect.Models.EndlessAisle.ClassificationTree;
using Newtonsoft.Json;
using RestSharp;

namespace MagentoConnect.Controllers.EndlessAisle
{
	public class ClassificationTreeController : BaseController, IClassificationTreeController
	{
		public string AuthToken { get; }

		public ClassificationTreeController(string eaAuthToken)
		{
			AuthToken = eaAuthToken;
		}
	   

		/**
		 * Gets a classification tree
		 *
		 * @param   classificationTreeId          Identifier for a Classification tree to fetch
		 * 
		 * @return  ClassificationTreeResource     Returns a classification tree
		 */
		public ClassificationTreeResource GetClassificationTree(int classificationTreeId)
		{
			var endpoint = UrlFormatter.EndlessAisleClassificationTreeUrl(classificationTreeId);

			var client = new RestClient(endpoint);
			var request = new RestRequest(Method.GET);

			request.AddHeader("Authorization", $"Bearer {AuthToken}");
			request.AddHeader("Accept", "application/json");

			var response = client.Execute(request);

			//Ensure we get the right code
			CheckStatusCode(response.StatusCode);

			return JsonConvert.DeserializeObject<ClassificationTreeResource>(response.Content);
		}

		/**
		 * Searches for a similar Classification in the Tree
		 *
		 * @param   classificationTreeId    Identifier of a Classification Tree
		 * @param   classificationId        Identifier of a Classification 
		 * 
		 * @return  int                     Id of a Classification or Category
		 */
		public int GetClassificationById(int classificationTreeId, int classificationId)
		{
			var tree = GetClassificationTree(classificationTreeId);

			return SearchTree(tree, classificationId).Id;
		}

		/**
		 * Searches the base tree
		 *
		 * @param   node                    Name of a node to search for
		 * @param   classificationId        Identifier of a Classification 
		 * 
		 * @return  ClassificationTreeNode  Matching classification
		 */
		private static ClassificationTreeNode SearchTree(ClassificationTreeResource node, int classificationId)
		{
			object foundObj = null;

			//search root classifications 
			foreach (var classification in node.Classifications.Where(classification => classification.Id == classificationId))
			{
				return classification;
			}

			//search child categories
			foreach (var category in node.Categories.Where(category => foundObj == null))
			{
				foundObj = SearchCategories(category, classificationId);
			}

			return (ClassificationTreeNode)foundObj;
		}

		/**
		 * Searches child categories
		 *
		 * @param   node                    Name of a node to search
		 * @param   classificationId        Identifier of a Classification 
		 * 
		 * @return  object                  Matching classification
		 */
		private static object SearchCategories(ClassificationCategoryResource node, int classificationId)
		{
			object foundObj = null;

			if (node.Id == classificationId)
			{
				return node;
			}

			//search root classifications 
			foreach (var classification in node.Classifications.Where(classification => classification.Id == classificationId))
			{
				return classification;
			}

			//search child categories
			foreach (var category in node.Categories.Where(category => foundObj == null))
			{
				foundObj = SearchCategories(category, classificationId);
			}

			return foundObj;
		}
	}
}