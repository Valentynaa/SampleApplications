using System.Collections.Generic;
using MagentoConnect.Models.EndlessAisle.FieldDefinitions;

namespace MagentoConnect.Controllers.EndlessAisle
{
	public interface IFieldDefinitionController : IController
	{
		FieldDefinitionResource GetAFieldDefinition(int fieldDefinitionId);
		List<FieldDefinitionResource> GetAllFieldDefinitions();
	}
}