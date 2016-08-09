using System;
using System.Collections.Generic;

namespace WebhookUpdater.Resources
{
	[Serializable]
	public class WebhookListResponse
	{
		public IEnumerable<Webhook> webhooks { get; set; }
	}
}
