using System;
using System.Collections.Generic;
using MagentoConnect.Controllers.EndlessAisle;
using MagentoConnect.Models.EndlessAisle.ProductLibrary;
using MagentoConnect.Models.EndlessAisle.ProductLibrary.Projections;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.MockObjects.Controllers.EndlessAisle
{
	public class MockProductLibraryController : IProductLibraryController
	{
		public ProductDetailsResource GetProductBySlug(string slug)
		{
			return new ProductDetailsResource()
			{
				Id = slug,
				Name = "Configurable Product",
				MasterProductId = 2039,
				Owner = new EntityRefResource()
				{
					Id = 14146,
					Name = "KENTEL Corp"
				},
				CanonicalClassification = new ClassificationBreadcrumbResource()
				{
					TreeId = 395,
					Id = 399,
					Name = "Jackets",
					ParentCategories = new List<ClassificationBreadcrumbItem>()
				},
				ShortDescription = "",
				LongDescription = "",
				Assets = new List<ProductDetailAssetResource>()
				{
					new ProductDetailAssetResource()
					{
						Id = new Guid("5908ae37-2a45-4942-a6e0-ce32e1c2c2c7"),
						Name = "TestImage.jpg",
						Uri = "https://amsdemo.iqmetrix.net/images/5908ae37-2a45-4942-a6e0-ce32e1c2c2c7",
						Type = "Image",
						IsHidden = false
					}
				},
				HeroShotUri = "https://amsdemo.iqmetrix.net/images/5908ae37-2a45-4942-a6e0-ce32e1c2c2c7",
				HeroShotId = new Guid("5908ae37-2a45-4942-a6e0-ce32e1c2c2c7"),
				ManufacturerSkus = new List<IdentifierResource>(),
				VendorSkus = new List<IdentifierResource>(),
				UpcCodes = new List<IdentifierResource>(),
				IsSaleable = true,
				Version = 96
			};
		}

		public ColorDefinitionsResource GetColorDefinitions(int productDocumentId)
		{
			return new ColorDefinitionsResource()
			{
				ColorDefinitions = new List<ColorDefinitionResource>()
				{
					new ColorDefinitionResource()
					{
						ColorTagIds = new List<int>()
						{
							1
						},
						ColorTags = new List<ColorTagResource>()
						{
							new ColorTagResource()
							{
								Id = 1,
								Name = "Black",
								ColorCode = "#303232"
							}
						},
						Id = new Guid("5c6e2779-79d1-4fbd-b6a8-36b81e851b15"),
						Name = "Black Sapphire",
						Swatch = new SwatchResource()
						{
							Type = "ColorCode",
							ColorCode = "#C0C8D0"
						}
					}
				}
			};
		}

		public ColorTagsResource GetColorTags()
		{
			return new ColorTagsResource()
			{
				ColorTags = new List<ColorTagResource>()
				{
					new ColorTagResource()
					{
						Id = 1,
						Name = "Black",
						ColorCode = "#303232"
					},
					new ColorTagResource()
					{
						Id = 2,
						Name = "Blue",
						ColorCode = "#3180BA"
					}
				}
			};
		}

		public ProductDocumentResource GetProductHierarchy(int productDocumentId)
		{
			return new ProductDocumentResource()
			{
				Id = productDocumentId,
				Classification = new ProductClassificationRefResource()
				{
					Id = 4,
					TreeId = 1,
					Name = "Smartphones"
				},
				ColorDefinitions = new List<ColorDefinitionResource>()
				{
					new ColorDefinitionResource()
					{
						ColorTagIds = new List<int>()
						{
							1
						},
						ColorTags = new List<ColorTagResource>()
						{
							new ColorTagResource()
							{
								Id = 1,
								Name = "Black",
								ColorCode = "#303232"
							}
						},
						Id = new Guid("5c6e2779-79d1-4fbd-b6a8-36b81e851b15"),
						Name = "Black Sapphire",
						Swatch = new SwatchResource()
						{
							Type = "ColorCode",
							ColorCode = "#C0C8D0"
						}
					}
				},
				CreatedUtc = DateTime.Now, 
				Manufacturer = new EntityRefResource()
				{
					Id = 13149,
					Name = "OtterBox"
				},
				Owner = new EntityRefResource()
				{
					Id = 14146,
					Name = "Kentel Corp"
				},
				RevisionGroups = new List<RevisionGroupResource>(),
				RootRevision = new RootRevisionResource()
				{
					Variations = new List<VariationResource>()
					{
						new VariationResource()
						{
							FieldValues = new List<FieldResource>()
							{
								new FieldEditResource()
								{
									FieldDefinitionId = 1,
									LanguageInvariantValue = "Daisy Lou Pump - Youth Blue-XS"
								}
							}
						}
					},
					IsArchived = true,
					FieldValues = new List<FieldResource>()
					{
						new FieldEditResource()
								{
									FieldDefinitionId = 1,
									LanguageInvariantValue = "Daisy Lou Pump - Youth Blue-XS"
								}
					},
					IdentifierGroups = new List<ProductIdentifierGroupResource>()
				},
				Version = 1
			};
		}

		public int UpdateVariation(int productDocumentId, int variationId, RevisionEditResource product)
		{
			return variationId;
		}

		public ColorDefinitionsResource CreateColorDefinition(int productDocumentId, ColorDefinitionsResource colorDefs)
		{
			return colorDefs;
		}

		public ProductDocumentResource CreateMasterProduct(ProductDocumentResource product)
		{
			return product;
		}

		public int UpdateMasterProduct(int productDocumentId, RevisionEditResource product)
		{
			return productDocumentId;
		}

		public CreatedVariationResource CreateVariation(int productDocumentId, VariationResource variation)
		{
			return new CreatedVariationResource()
			{
				VariationId = productDocumentId
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
