using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagentoConnect.Models.EndlessAisle.Orders
{
	[Serializable]
	public class TrackingInformationResource
	{
		/// <summary>
		/// The number of items associated with the tracking number.
		/// </summary>
		public int Quantity { get; set; }

		/// <summary>
		/// The tracking number associated with the item shipment.
		/// </summary>
		public string TrackingNumber { get; set; }
	}
}
