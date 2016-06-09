using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagentoSync.Models.EndlessAisle.Entities
{
	[Serializable]
	public class TimeResource
	{
		public int Hour { get; set; }

		public int Minute { get; set; }
	}
}
