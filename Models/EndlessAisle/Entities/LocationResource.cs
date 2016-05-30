using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagentoConnect.Models.EndlessAisle.Entities
{
	[Serializable]
	public class LocationResource : EntityResource
	{
		public string LocationType { get; set; }
		public string LocationSubType { get; set; }
		public AddressResource Address { get; set; }
		public IEnumerable<ContactResource> Contacts { get; set; }
		public IEnumerable<PhoneNumberResource> StorePhoneNumbers { get; set; }
		public MeasurementResource Area { get; set; }
		public StoreHoursResource StoreHours { get; set; }
		public GeographyResource Geography { get; set; }
		public LocationTimeZoneResource TimeZone { get; set; }
	}
}
