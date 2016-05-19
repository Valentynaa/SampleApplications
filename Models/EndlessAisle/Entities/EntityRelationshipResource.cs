using System;

namespace MagentoConnect.Models.EndlessAisle.Entities
{
	[Serializable]
	public class EntityRelationshipResource
	{
		public int Id { get; set; }

		public int Definition { get; set; }
		public int Source { get; set; }
		public int Destination { get; set; }
		public DateTime? CreatedUtc { get; set; }

		public int Version { get; set; }
	}
}