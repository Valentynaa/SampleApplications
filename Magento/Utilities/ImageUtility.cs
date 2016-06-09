using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MagentoSync.Utilities
{
	public static class ImageUtility
	{
		/// <summary>
		/// Creates an image from the data located at a URI
		/// 
		/// NOTE: 
		///		This method does not dispose the memory stream used to create the Image as that would no longer allow you to use the Image properly
		///		http://stackoverflow.com/questions/336387/image-save-throws-a-gdi-exception-because-the-memory-stream-is-closed
		/// </summary>
		/// <param name="uri">String representing URI to get image from </param>
		/// <returns>Image created from data at URI</returns>
		public static Image ImageFromUri(string uri)
		{
			return ImageFromUri(new Uri(uri));
		}

		/// <summary>
		/// Creates an image from the data located at a URI
		/// 
		/// NOTE: 
		///		This method does not dispose the memory stream used to create the Image as that would no longer allow you to use the Image properly
		///		http://stackoverflow.com/questions/336387/image-save-throws-a-gdi-exception-because-the-memory-stream-is-closed
		/// </summary>
		/// <param name="uri">URI to get image from </param>
		/// <returns>Image created from data at URI</returns>
		public static Image ImageFromUri(Uri uri)
		{
			using (WebClient client = new WebClient())
			{
				byte[] imageBytes = client.DownloadData(uri);
				return ImageFromBytes(imageBytes);
			}
		}

		/// <summary>
		/// Creates an image from bytes provided
		/// 
		/// NOTE: 
		///		This method does not dispose the memory stream used to create the Image as that would no longer allow you to use the Image properly
		///		http://stackoverflow.com/questions/336387/image-save-throws-a-gdi-exception-because-the-memory-stream-is-closed
		/// </summary>
		/// <param name="bytes">Bytes to create image from </param>
		/// <returns>Image created from bytes</returns>
		public static Image ImageFromBytes(byte[] bytes)
		{
			//NOTE: Do not dispose this memory stream as that no longer allows you to use the Image properly
			MemoryStream stream = new MemoryStream(bytes);
			return Image.FromStream(stream);
		}

		/// <summary>
		/// Determines if two images are equal.
		/// 
		/// Images have their bytes converted into strings and are compared that way rather
		/// than comparing pixel by pixel to save time.
		/// </summary>
		/// <param name="image1">Image to compare</param>
		/// <param name="image2">Image to compare</param>
		/// <returns>If the image bytes are equal</returns>
		public static bool AreEqual(Image image1, Image image2)
		{
			if (image1 == null || image2 == null)
			{
				return false;
			}

			string image1String;
			string image2String;

			using (MemoryStream stream = new MemoryStream())
			{
				image1.Save(stream, image1.RawFormat);
				image1String = Convert.ToBase64String(stream.ToArray());
			}

			using (MemoryStream stream = new MemoryStream())
			{
				image2.Save(stream, image2.RawFormat);
				image2String = Convert.ToBase64String(stream.ToArray());
			}

			return image1String.Equals(image2String);
		}
	}
}
