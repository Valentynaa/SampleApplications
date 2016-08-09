using Newtonsoft.Json;
using RestSharp;
using System.Collections.Generic;
using WebhookUpdater.Resources;
using WebhookUpdater.Utilities;

namespace WebhookUpdater.Controllers
{
	public class WebhookController : BaseController
	{
		public WebhookController(string storeName)
		{
			_urlFormatter = new UrlFormatter(storeName);
		}
		private readonly UrlFormatter _urlFormatter;

		/// <summary>
		/// Creates a webhook for Shopify
		/// </summary>
		/// <param name="webhook">Webhook to create</param>
		/// <returns>Webhook created</returns>
		public Webhook CreateWebhook(Webhook webhook)
		{
			var response = Request(_urlFormatter.CreateWebhookUrl(), Method.POST, new WebhookRequest(webhook));
			var content = JsonConvert.DeserializeObject<WebhookResponse>(response.Content);
			return content.webhook;
		}

		/// <summary>
		/// Gets all webhooks for the Shopify Store
		/// </summary>
		/// <returns>List of all Webhooks for store</returns>
		public IEnumerable<Webhook> GetWebhooks()
		{
			var response = Request(_urlFormatter.GetWebhooksUrl(), Method.GET);
			var content = JsonConvert.DeserializeObject<WebhookListResponse>(response.Content);
			return content.webhooks;
		}

		/// <summary>
		/// Updates a webhook
		/// </summary>
		/// <param name="webhook">Webhook with update</param>
		/// <returns>Webhook updated</returns>
		public Webhook UpdateWebhook(Webhook webhook)
		{
			var response = Request(_urlFormatter.UpdateWebhookUrl(webhook.id), Method.PUT, new WebhookRequest(webhook));
			var content = JsonConvert.DeserializeObject<WebhookResponse>(response.Content);
			return content.webhook;
		}

		/// <summary>
		/// Deletes a webhook
		/// </summary>
		/// <param name="webhook">Webhook deleted</param>
		public void DeleteWebhook(Webhook webhook)
		{
			Request(_urlFormatter.DeleteWebhookUrl(webhook.id), Method.DELETE);
		}
	}
}
