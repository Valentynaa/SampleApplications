namespace WebhookUpdater.Utilities
{
	public class UrlFormatter
	{
		public UrlFormatter(string storeName)
		{
			_store = storeName;
		}
		private readonly string _store;

		public string CreateWebhookUrl()
		{
			return string.Format("https://{0}.myshopify.com/admin/webhooks.json", _store);
		}

		public string GetWebhooksUrl()
		{
			return string.Format("https://{0}.myshopify.com/admin/webhooks.json", _store);
		}
		
		public string UpdateWebhookUrl(int webhookId)
		{
			return string.Format("https://{0}.myshopify.com/admin/webhooks/{1}.json", _store, webhookId);
		}

		public string DeleteWebhookUrl(int webhookId)
		{
			return string.Format("https://{0}.myshopify.com/admin/webhooks/{1}.json", _store, webhookId);
		}
	}
}