using System;
using System.Collections.Generic;
using MagentoConnect.Controllers.EndlessAisle;
using MagentoConnect.Models.EndlessAisle.Entities;

namespace Tests.MockObjects.Controllers.EndlessAisle
{
	public class MockEntitiesController : IEntitiesController
	{
		public List<ManufacturerResource> GetAllManufacturers()
		{
			return new List<ManufacturerResource>()
			{
				new ManufacturerResource()
				{
					Attributes = new Dictionary<string, string>(),
					ClientEntityId = null,
					CorrelationId = null,
					CreatedUtc = DateTime.Now,
					Description = "",
					Id = 13149,
					LastModifiedUtc = null,
					Logo = new AssetResource()
					{
						Height = 656,
						Href = "https://amsdemostorage.blob.core.windows.net/assets/be69f1de-975f-45e1-b69e-5a458957a02d.png",
						Id = "be69f1de-975f-45e1-b69e-5a458957a02d",
						Md5Checksum = "534b838c7996a7d8309607949daa59ff",
						MimeType = "image/png",
						Name = "otterbox-logo.png",
						Width = 1024
					},
					Name = "OtterBox",
					Relationships = new List<object>(),
					Role = "Manufacturer",
					Roles = new List<EntityRoleResource>()
					{
						new EntityRoleResource()
						{
							Name = "Manufacturer"
						}
					},
					SortName = "otterbox",
					TypeId = null
				}
			};
		}

		public LocationResource GetLocation()
		{
			return new LocationResource()
			{
				Address = new AddressResource()
				{
					AddressLine1 = "300 Murray St",
					City = "Perth",
					StateCode = "WA",
					StateName = "Western",
					CountryCode = "AU",
					CountryName = "Australia",
					Zip = "6000"
				},
				Id = 14192,
				Name = "Raine Square",
				Description = "",
				Roles = new List<EntityRoleResource>()
				{
					new EntityRoleResource()
					{
						Name = "Location"
					}
				},
				Role = "Location",
				SortName = "raine square",
				Attributes = new Dictionary<string, string>(),
				Relationships = new List<EntityRelationshipResource>()
				{
					new EntityRelationshipResource()
					{
						Id = 9727,
						Definition = 16,
						Source = 14192,
						Destination = 17566,
						CreatedUtc = DateTime.Now,
						Version = 1
					}
				},
				Version = 3,
				CreatedUtc = DateTime.Now,
				LastModifiedUtc = null,
				TypeId = 95
			};
		}

		public string AuthToken
		{
			get
			{
				return "WHdvc0h9b3NYQW9zWHZvc1h2JSYBAyc_PhRWBSAYLUFsIBkSMEA9MRYVWAcsPCQQaD8NJiBPLSUeFVgXGjEBJA4bX0EaBBtAOR8I";
			}
		}
	}
}
