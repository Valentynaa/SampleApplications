using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using WebhookUpdater.Controllers;
using WebhookUpdater.Resources;
using WebhookUpdater.Utilities;

namespace WebhookUpdater
{
	class Program
	{
		[STAThread]
		static void Main(string[] args)
		{
			//Display configuration
			try
			{
				var appSettings = ConfigurationManager.AppSettings;

				if (appSettings.Count == 0)
				{
					Console.WriteLine("AppSettings is empty.");
				}
				else
				{
					foreach (var key in appSettings.AllKeys)
					{
						Console.WriteLine("Key: {0} Value: {1}", key, appSettings[key]);
					}
				}
			}
			catch (ConfigurationErrorsException)
			{
				Console.WriteLine("Error reading app settings");
			}


			string storeName = ConfigurationManager.AppSettings["StoreName"];
			
			//Initialize controller and get existing webhooks
			var webhookController = new WebhookController(storeName);
			var webhooks = webhookController.GetWebhooks();

			CreateNewWebhooks(webhookController);
			DeleteWebhooks(webhookController, webhooks);

			Console.ReadKey();
		}

		/// <summary>
		/// Performs a dialog with user to create webhooks at a user specified endpoint
		/// </summary>
		/// <param name="webhookController">Controller to use for creating webhooks</param>
		private static void CreateNewWebhooks(WebhookController webhookController)
		{
			bool confirmed = false;
			string newUrl = string.Empty;
			while (!confirmed)
			{
				Console.WriteLine("Enter the new URL to set for webhooks.");
				newUrl = Console.ReadLine();
				Console.WriteLine("\"{0}\", confirm?(y/n)", newUrl);
				confirmed = (Console.ReadLine() == "y");
			}
			
			var topics = TopicReader.ReadTopics();
			List<Webhook> createdWebhooks = new List<Webhook>();
			
			//Create webhooks from selected file's topics
			foreach (string topic in topics)
			{
				try
				{
					createdWebhooks.Add(webhookController.CreateWebhook(new Webhook()
					{
						address = newUrl,
						topic = topic
					}));
				}
				catch (Exception ex)
				{
					Console.WriteLine("Error creating webhook: {0}", ex.Message);
				}
			}

			foreach (var createdWebhook in createdWebhooks)
			{
				Console.WriteLine("Webhook for {0} created with ID {1}", createdWebhook.topic, createdWebhook.id);
			}

		}

		/// <summary>
		/// Performs a dialog with user to delete webhooks for a user specified endpoint
		/// </summary>
		/// <param name="webhookController">Controller to use for deleting webhooks</param>
		/// <param name="existingWebhooks">Current webhooks</param>
		private static void DeleteWebhooks(WebhookController webhookController, IEnumerable<Webhook> existingWebhooks)
		{
			bool delete = true;
			bool confirmed = false;
			string deleteUrl = String.Empty;

			while (delete && !confirmed)
			{
				Console.WriteLine("Delete webhooks for a url? Enter URL to delete or \'n\'");
				deleteUrl = Console.ReadLine();
				delete = deleteUrl != "n";

				if (delete)
				{
					Console.WriteLine("\"{0}\", confirm?(y/n)", deleteUrl);
					confirmed = (Console.ReadLine() == "y");
				}
			}

			//Delete any webhook that matches user input
			foreach (var existingWebhook in existingWebhooks.Where(x => x.address == deleteUrl))
			{
				Console.WriteLine("Deleting webhook for {0} with topic {1}", existingWebhook.address, existingWebhook.topic);
				webhookController.DeleteWebhook(existingWebhook);
			}
			Console.WriteLine("Done deleting webhooks");
		}
	}
}
