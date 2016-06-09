using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagentoSync.Models.EndlessAisle.Entities
{
	[Serializable]
	public class MeasurementResource
	{
		public decimal Value { get; set; }
		public string Unit { get; set; }
	}
}
