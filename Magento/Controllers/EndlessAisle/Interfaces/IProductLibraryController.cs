using MagentoSync.Models.EndlessAisle.ProductLibrary;
using MagentoSync.Models.EndlessAisle.ProductLibrary.Projections;

namespace MagentoSync.Controllers.EndlessAisle.Interfaces
{
	public interface IProductLibraryController : IController
	{
        ColorDefinitionsResource GetColorDefinitions(int productDocumentId);
		ColorTagsResource GetColorTags();
		ProductDocumentResource GetProductHierarchy(int productDocumentId);
		int UpdateVariation(int productDocumentId, int variationId, RevisionEditResource product);
		ColorDefinitionsResource CreateColorDefinition(int productDocumentId, ColorDefinitionsResource colorDefs);
		ProductDocumentResource CreateMasterProduct(ProductDocumentResource product);
		int UpdateMasterProduct(int productDocumentId, RevisionEditResource product);
		CreatedVariationResource CreateVariation(int productDocumentId, VariationResource variation);
        ProductDetailsResource GetProductBySlug(string slug);
    }
}