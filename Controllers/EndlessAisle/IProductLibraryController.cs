using MagentoConnect.Models.EndlessAisle.ProductLibrary;
using MagentoConnect.Models.EndlessAisle.ProductLibrary.Projections;

namespace MagentoConnect.Controllers.EndlessAisle
{
	public interface IProductLibraryController : IController
	{
		ProductDetailsResource GetProductBySlug(string slug);
		ColorDefinitionsResource GetColorDefinitions(int productDocumentId);
		ColorTagsResource GetColorTags();
		ProductDocumentResource GetProductHierarchy(int productDocumentId);
		int UpdateVariation(int productDocumentId, int variationId, RevisionEditResource product);
		ColorDefinitionsResource CreateColorDefinition(int productDocumentId, ColorDefinitionsResource colorDefs);
		ProductDocumentResource CreateMasterProduct(ProductDocumentResource product);
		int UpdateMasterProduct(int productDocumentId, RevisionEditResource product);
		CreatedVariationResource CreateVariation(int productDocumentId, VariationResource variation);
	}
}