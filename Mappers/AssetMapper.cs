﻿using System;
using System.Linq;
using MagentoConnect.Controllers.EndlessAisle;
using MagentoConnect.Models.EndlessAisle.ProductLibrary;
using System.Collections.Generic;
using MagentoConnect.Models.EndlessAisle.ProductLibrary.Projections;
using MagentoConnect.Models.Magento.Products;
using MagentoConnect.Utilities;

namespace MagentoConnect.Mappers
{
	public class AssetMapper : BaseMapper
	{
		private readonly AssetsController _eaAssetsController;
		private readonly ProductLibraryController _eaProductController;

		public AssetMapper(string magentoAuthToken, string eaAuthToken) : base(magentoAuthToken, eaAuthToken)
		{
			_eaAssetsController = new AssetsController(eaAuthToken);
			_eaProductController = new ProductLibraryController(eaAuthToken);
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
		public List<AssetResource> ParseAssetsFromProduct(ProductResource magentoProduct)
		{
			var assets = new List<AssetResource>();

			//Get existing magento assets
			var magentoAssets = magentoProduct.media_gallery_entries;

			//Path to magento assets
			var magentoPath = new UrlFormatter().MagentoCatalogAssetPath(ConfigReader.MagentoServerPath);

			if (ProductHasMapping(magentoProduct))
			{
				//Get master product Id from the mapping slug
				var mappingSlug =
					GetAttributeByCode(magentoProduct.custom_attributes, ConfigReader.MappingCode).ToString();

				var eaAssets = _eaProductController.GetProductBySlug(mappingSlug).Assets;

				//Loop through magento product assets. This update can only ADD assets, not remove or change
				foreach (var magentoAsset in magentoAssets)
				{
					var hasChanged = true;

					//Is there a matching asset in the EA product? Only compare name
					foreach (var eaAsset in eaAssets)
					{
						if (eaAsset.Name == magentoAsset.file.Substring(magentoAsset.file.LastIndexOf('/') + 1))
						{
							//Add asset, no further processing
							assets.Add(new AssetResource
							{
								Id = eaAsset.Id,
								Name = eaAsset.Name,
								IsHidden = eaAsset.IsHidden,
								MimeType = eaAsset.Type
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
		* This function changes the Hero Shot, or main image, of a product to a different asset
		* The new asset must ALREADY be assigned to the product
		* 
		* @param   slug     Identifier of a product in EA
		* @param   assetId  Identifier of new hero shot asset
		*
		* @return  Guid     Identifier of new hero shot asset
		*/
		public Guid SetHeroShot(string slug, Guid assetId)
		{
			var assetRequest = new AssetResponse { assetId = assetId };

			return _eaAssetsController.SetHeroShot(slug, assetRequest).assetId;
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