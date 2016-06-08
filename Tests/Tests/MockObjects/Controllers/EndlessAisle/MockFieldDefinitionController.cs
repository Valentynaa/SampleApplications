using MagentoConnect.Controllers.EndlessAisle;
using MagentoConnect.Models.EndlessAisle.FieldDefinitions;
using System.Collections.Generic;

namespace Tests.MockObjects.Controllers.EndlessAisle
{
	public class MockFieldDefinitionController : BaseMockController, IFieldDefinitionController
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
	}
}
