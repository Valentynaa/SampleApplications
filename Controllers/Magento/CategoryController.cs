using MagentoConnect.Models.Magento.Category;
using Newtonsoft.Json;
using RestSharp;

namespace MagentoConnect.Controllers.Magento
{
    public class CategoryController : BaseController
    {
        public static string MagentoAuthToken;

        public CategoryController(string magentoAuthToken)
        {
            MagentoAuthToken = magentoAuthToken;
        }

        /**
         * Returns information about a category
         *
         * @param   categoryId          attribute_code of the attribute, see Product response
         *
         * @return  CategoryResource    Category requested
         */
        public CategoryResource GetCategory(int categoryId)
        {
            var endpoint = UrlFormatter.MagentoCategoryUrl(categoryId);

            var client = new RestClient(endpoint);
            var request = new RestRequest(Method.GET);

            request.AddHeader("Authorization", string.Format("Bearer {0}", MagentoAuthToken));

            var response = client.Execute(request);

            //Ensure we get the right code
            CheckStatusCode(response.StatusCode);

            return JsonConvert.DeserializeObject<CategoryResource>(response.Content);
        }
    }
}
