using System;
using System.Collections.Generic;
using System.Configuration;

namespace WebhookUpdater.Resources
{
	[Serializable]
	public class Webhook
	{
		public int id { get; set; }
		public string address { get; set; }
		public string topic { get; set; }
		public string format { get; set; }
		public IEnumerable<string> metafield_namespaces { get; set; }

		public Webhook()
		{
			format = "json";
			metafield_namespaces = new List<string>()
			{
				ConfigurationManager.AppSettings["MetafieldNamespace"]
			};
		}
	}
}
