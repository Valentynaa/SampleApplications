using System;
using System.Drawing;
using System.Linq;
using MagentoSync.Models.EndlessAisle.ProductLibrary;
using System.Collections.Generic;
using MagentoSync.Controllers.EndlessAisle.Interfaces;
using MagentoSync.Database;
using MagentoSync.Models.Magento.Products;
using MagentoSync.Utilities;

namespace MagentoSync.Mappers
{
	public class AssetMapper : BaseMapper
	{
		private readonly IAssetsController _eaAssetsController;
		private readonly IProductLibraryController _eaProductController;
        private readonly ICatalogsController _eaCatalogsController;

		public AssetMapper(IAssetsController assetsController, IProductLibraryController productLibraryController, ICatalogsController catalogsController)
		{
			_eaAssetsController = assetsController;
			_eaProductController = productLibraryController;
		    _eaCatalogsController = catalogsController;
		}

		/**
		* This function extracts a list of assets from a magento product and returns them as a list of EA assets
		* If the magento product has a mapping, it will cross reference existing assets to prevent assets from being uploaded
		* multiple times
		* Note that this WILL NOT delete assets that were removed
		* 
		* @param   magentoProduct          Magento product to parse
		*
		* @return  List<AssetResource>     Assets
		*/
		public List<AssetResource> ParseAssetsFromProduct(ProductResource magentoProduct, MediaStorageConfiguration configuration)
		{
			var assets = new List<AssetResource>();

			//Get existing magento assets
			var magentoAssets = magentoProduct.media_gallery_entries;

			//Path to magento assets
			var magentoPath = new UrlFormatter().MagentoCatalogAssetPath(ConfigReader.MagentoServerPath);

			if (ProductHasMapping(magentoProduct))
			{
				//Get master product Id from the mapping slug
				var catalogItemId =
					GetAttributeByCode(magentoProduct.custom_attributes, ConfigReader.MappingCode).ToString();

			    var slug = _eaCatalogsController.GetCatalogItem(catalogItemId).Slug;

				var eaAssets = _eaProductController.GetProductBySlug(slug).Assets.ToList();

				//Loop through magento product assets. This update can only ADD assets, not remove or change
				foreach (var magentoAsset in magentoAssets)
				{
					bool hasChanged = true;
					Image magentoImage = null;

					switch (configuration)
					{
						case MediaStorageConfiguration.FileSystem:
							magentoImage = Image.FromFile(magentoPath + magentoAsset.file);
							break;
						case MediaStorageConfiguration.Database:
							magentoImage = ImageUtility.ImageFromBytes(DatabaseConnection.Instance.GetMediaGalleryEntryFile(magentoAsset));
							break;
					}
					
					//Is there a matching asset in the EA product? Only compare name
					foreach (var eaAsset in eaAssets)
					{
						if (eaAsset.Name == magentoAsset.file.Substring(magentoAsset.file.LastIndexOf('/') + 1) && ImageUtility.AreEqual(magentoImage, ImageUtility.ImageFromUri(eaAsset.Uri)))
						{
							//Add asset, no further processing
							assets.Add(new AssetResource
							{
								Id = eaAsset.Id,
								Name = eaAsset.Name,
								IsHidden = eaAsset.IsHidden,
								MimeType = _eaAssetsController.GetAsset(eaAsset.Id.ToString()).MimeType
							});

							hasChanged = false;
						}
					}

					//new asset! upload + add it to the product
					if (hasChanged)
					{
						assets.Add(_eaAssetsController.CreateAsset(magentoPath + magentoAsset.file));
					}
				}

			}
			else if(magentoAssets != null)
			{
				assets.AddRange(magentoAssets.Select(magentoAsset => _eaAssetsController.CreateAsset(magentoPath + magentoAsset.file)));
			}

			return assets;
		}

		/**
		* This function determines the hero shot, based on the Image attribute on the magento product
		* It then cross references this value with the media images and returns the appropriate one, or null 
		* 
		* @param   magentoProduct               Magento product to parse
		*
		* @return  MediaGalleryEntryResource    Matching magento image, or null
		*/
		public MediaGalleryEntryResource GetHeroShot(ProductResource magentoProduct)
		{
			if (magentoProduct == null)
			{
				throw new Exception("A Magento product must be provided.");
			}

			var imageAttr = GetAttributeByCode(magentoProduct.custom_attributes, ConfigReader.MagentoImageCode);

			return imageAttr == null ? null : magentoProduct.media_gallery_entries.FirstOrDefault(asset => asset.file == imageAttr.ToString());
		}
	}
}
