using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagentoConnect.Models.EndlessAisle.Entities
{
	[Serializable]
	public class AssetResource
	{
		public string Id { get; set; }
		public string Href { get; set; }
		public int Height { get; set; }
		public int Width { get; set; }
		public string Md5Checksum { get; set; }
		public string Name { get; set; }
		public string MimeType { get; set; }
	}
}
