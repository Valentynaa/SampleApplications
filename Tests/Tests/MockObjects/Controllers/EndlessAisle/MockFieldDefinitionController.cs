using System;
using System.Collections.Generic;
using MagentoConnect.Controllers.EndlessAisle;
using MagentoConnect.Models.EndlessAisle.FieldDefinitions;

namespace Tests.MockObjects.Controllers.EndlessAisle
{
	public class MockFieldDefinitionController : IFieldDefinitionController
	{
		public FieldDefinitionResource GetAFieldDefinition(int fieldDefinitionId)
		{
			return new FieldDefinitionResource()
			{
				Id = fieldDefinitionId,
				StringId = "Release Date",
				InputType = "Date",
				IsRequired = false, 
				LanguageInvariantUnit = "",
				DisplayName = "ReleaseDate",
				Unit = "",
				Options = new List<OptionValueResource>()
			};
		}

		public List<FieldDefinitionResource> GetAllFieldDefinitions()
		{
			return new List<FieldDefinitionResource>()
			{
				new FieldDefinitionResource()
				{
					Id = 1,
					StringId = "Product Name",
					InputType = "TextSingleLine",
					IsRequired = true,
					LanguageInvariantUnit = "",
					DisplayName = "Product Name",
					Unit = "",
					Options = new List<OptionValueResource>()
				},
				new FieldDefinitionResource()
				{
					Id = 140,
					StringId = "Release Date",
					InputType = "Date",
					IsRequired = false,
					LanguageInvariantUnit = "",
					DisplayName = "ReleaseDate",
					Unit = "",
					Options = new List<OptionValueResource>()
				}
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
