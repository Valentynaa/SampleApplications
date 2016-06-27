using System.Linq;
using MagentoSync.Controllers.EndlessAisle.Interfaces;
using MagentoSync.Controllers.Magento.Interfaces;
using MagentoSync.Models.EndlessAisle.Entities;
using MagentoSync.Models.Magento.Country;

namespace MagentoSync.Mappers
{
	public class EntityMapper : BaseMapper
	{
		private readonly IEntitiesController _eaEntitiesController;
		private readonly IRegionController _magentoRegionController;

		public EntityMapper(IEntitiesController entitiesController, IRegionController regionController)
		{
			_eaEntitiesController = entitiesController;
			_magentoRegionController = regionController;
		}

		//
		/// <summary>
		/// Magento Region from the config file.
		/// 
		/// Region is determined by matching the EA location's country to the Magento  country then 
		/// matching the region code in the magento country to the EA StateCode or StateName
		/// </summary>
		public RegionResource MagentoRegion
		{
			get
			{
			    if (_magentoRegion != null) return _magentoRegion;
			    var countries = _magentoRegionController.GetCountries();

			    var country = countries.First(x => x.id == EaLocation.Address.CountryCode);
			    if (country.available_regions != null)
			    {
			        _magentoRegion = country.available_regions.First(x => x.code == EaLocation.Address.StateCode || x.code == EaLocation.Address.StateName);
			    }
			    return _magentoRegion;
			}
		}
		private RegionResource _magentoRegion;

		//Endless Aisle location from App.config
		public LocationResource EaLocation => _eaLocation ?? (_eaLocation = _eaEntitiesController.GetLocation());
	    private LocationResource _eaLocation;
	}
}
