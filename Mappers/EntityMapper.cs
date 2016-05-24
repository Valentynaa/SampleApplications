using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MagentoConnect.Controllers.EndlessAisle;
using MagentoConnect.Controllers.Magento;
using MagentoConnect.Models.EndlessAisle.Entities;
using MagentoConnect.Models.Magento.Country;

namespace MagentoConnect.Mappers
{
	public class EntityMapper : BaseMapper
	{
		private readonly EntitiesController _eaEntitiesController;
		private readonly RegionController _magentoRegionController;

		public EntityMapper(string magentoAuthToken, string eaAuthToken) : base(magentoAuthToken, eaAuthToken)
		{
			_eaEntitiesController = new EntitiesController(eaAuthToken);
			_magentoRegionController = new RegionController(magentoAuthToken);
		}

		//Magento Region from the 
		public RegionResource MagentoRegion
		{
			get
			{
				if (_magentoRegion == null)
				{
					var countries = _magentoRegionController.GetCountries();

					var country = countries.First(x => x.id == EaLocation.Address.CountryCode);
					if (country.available_regions != null)
					{
						_magentoRegion = country.available_regions.First(x => x.code == EaLocation.Address.StateCode || x.code == EaLocation.Address.StateName);
					}
				}
				return _magentoRegion;
			}
		}
		private RegionResource _magentoRegion;

		//Endless Aisle location from App.config
		public LocationResource EaLocation
		{
			get
			{
				if (_eaLocation == null)
				{
					_eaLocation = _eaEntitiesController.GetLocation();
				}
				return _eaLocation;
			}
		}
		private LocationResource _eaLocation;
	}
}
