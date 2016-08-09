using System;

namespace WebhookUpdater.Resources
{
	[Serializable]
	public class WebhookRequest
	{
		public WebhookRequest(Webhook webhookIn)
		{
			webhook = webhookIn;
		}

		public Webhook webhook { get; set; }
	}
}
