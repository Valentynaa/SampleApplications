using System;

namespace WebhookUpdater.Resources
{
	[Serializable]
	public class WebhookResponse
	{
		public Webhook webhook { get; set; }
	}
}
