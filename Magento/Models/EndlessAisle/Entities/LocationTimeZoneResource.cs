using System;

namespace MagentoSync.Models.EndlessAisle.Entities
{
	[Serializable]
	public class LocationTimeZoneResource
	{
		public string Id { get; set; }
		public bool DaylightSavingTimeEnabled { get; set; }
	}
}