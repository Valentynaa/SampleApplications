using System.Collections.Generic;
using MagentoSync.Models.EndlessAisle.FieldDefinitions;

namespace MagentoSync.Controllers.EndlessAisle.Interfaces
{
	public interface IFieldDefinitionController : IController
	{
		FieldDefinitionResource GetAFieldDefinition(int fieldDefinitionId);
		List<FieldDefinitionResource> GetAllFieldDefinitions();
	}
}