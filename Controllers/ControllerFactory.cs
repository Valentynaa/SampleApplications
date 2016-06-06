using System;
using System.Collections.Generic;
using System.Linq;
using MagentoConnect.Controllers.EndlessAisle;
using MagentoConnect.Controllers.Magento;
using MagentoConnect.Exceptions;

namespace MagentoConnect.Controllers
{
	public enum ControllerType
	{
		Assets,
		Availability,
		Catalogs,
		ClassificationTree,
		Entities,
		FieldDefinition,
		Orders,
		Pricing,
		ProductLibrary,
		Cart,
		Category,
		CustomAttributes,
		Customer,
		Product,
		Region
	}

	public class ControllerFactory
	{
		private readonly string _magentoAuthToken;
		private readonly string _eaAuthToken;

		public ControllerFactory(string magentoAuthToken, string eaAuthToken)
		{
			_magentoAuthToken = magentoAuthToken;
			_eaAuthToken = eaAuthToken;
		}

		public List<IController> MakeAllControllers()
		{
			return new List<IController>
			{
				new AssetsController(_eaAuthToken),
				new AvailabilityController(_eaAuthToken),
				new CatalogsController(_eaAuthToken),
				new ClassificationTreeController(_eaAuthToken),
				new EntitiesController(_eaAuthToken),
				new FieldDefinitionController(_eaAuthToken),
				new OrdersController(_eaAuthToken),
				new PricingController(_eaAuthToken),
				new ProductLibraryController(_eaAuthToken),
				new CartController(_magentoAuthToken),
				new CategoryController(_magentoAuthToken),
				new CustomAttributesController(_magentoAuthToken),
				new CustomerController(_magentoAuthToken),
				new ProductController(_magentoAuthToken),
				new RegionController(_magentoAuthToken)
			};
		}

		public IController CreateController(ControllerType controller)
		{
			switch (controller)
			{
				case ControllerType.Assets:
					return new AssetsController(_eaAuthToken);
				case ControllerType.Availability:
					return new AvailabilityController(_eaAuthToken);
				case ControllerType.Catalogs:
					return new CatalogsController(_eaAuthToken);
				case ControllerType.ClassificationTree:
					return new ClassificationTreeController(_eaAuthToken);
				case ControllerType.Entities:
					return new EntitiesController(_eaAuthToken);
				case ControllerType.FieldDefinition:
					return new FieldDefinitionController(_eaAuthToken);
				case ControllerType.Orders:
					return new OrdersController(_eaAuthToken);
				case ControllerType.Pricing:
					return new PricingController(_eaAuthToken);
				case ControllerType.ProductLibrary:
					return new ProductLibraryController(_eaAuthToken);
				case ControllerType.Cart:
					return new CartController(_magentoAuthToken);
				case ControllerType.Category:
					return new CategoryController(_magentoAuthToken);
				case ControllerType.CustomAttributes:
					return new CustomAttributesController(_magentoAuthToken);
				case ControllerType.Customer:
					return new CustomerController(_magentoAuthToken);
				case ControllerType.Product:
					return new ProductController(_magentoAuthToken);
				case ControllerType.Region:
					return new RegionController(_magentoAuthToken);
				default:
					throw new ArgumentOutOfRangeException(nameof(controller), controller, null);
			}
		}
	}
}
