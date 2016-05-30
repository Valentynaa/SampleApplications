using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MagentoConnect.Models.EndlessAisle.Entities
{
	[Serializable]
	public class EntityResource
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }

		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public IEnumerable<EntityRoleResource> Roles { get; set; }
		public string Role { get; set; }

		// NOTE: can we do that on a client?
		public string SortName { get; set; }

		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public IDictionary<string, string> Attributes { get; set; }

		[JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
		public IEnumerable<EntityRelationshipResource> Relationships { get; set; }

		public int Version { get; set; }

		public DateTime CreatedUtc { get; set; }
		public DateTime? LastModifiedUtc { get; set; }

		public string CorrelationId { get; set; }

		public string ClientEntityId { get; set; }

		public int? TypeId { get; set; }

		public AssetResource Logo { get; set; }
	}
}
