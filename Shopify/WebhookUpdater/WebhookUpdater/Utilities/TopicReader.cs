using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace WebhookUpdater.Utilities
{
	public class TopicReader
	{
		/// <summary>
		/// Reads the topics from a file selected in a dialog
		/// </summary>
		/// <returns>List of topics</returns>
		public static IEnumerable<string> ReadTopics()
		{
			var path = GetPath();
			
			// This text is added only once to the file.
			if (File.Exists(path))
			{
				// Open the file to read from.
				return File.ReadAllLines(path);
			}
			return new List<string>();
		}

		/// <summary>
		/// Gets the path for a file selected from dialog
		/// </summary>
		/// <returns>File path seletced</returns>
		private static string GetPath()
		{
			OpenFileDialog openFileDialog = new OpenFileDialog();
			openFileDialog.Filter = "Text Files (*.txt)|*.txt|PDF Files (*.pdf)|*.pdf";

			while (true)
			{
				Console.WriteLine("Select a file for topics to subscribe to.");
				if (openFileDialog.ShowDialog() == DialogResult.OK)
				{
					Console.WriteLine(openFileDialog.FileName);
					return openFileDialog.FileName;
				}
			}
		}
	}
}
