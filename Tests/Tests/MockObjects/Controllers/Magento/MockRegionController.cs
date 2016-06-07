using System;
using System.Collections.Generic;
using MagentoConnect.Controllers.Magento;
using MagentoConnect.Models.Magento.Country;

namespace Tests.MockObjects.Controllers.Magento
{
	public class MockRegionController : IRegionController
	{
		public IEnumerable<CountryResource> GetCountries()
		{
			return new List<CountryResource>()
			{
				new CountryResource()
				{
					id = "AU",
					two_letter_abbreviation = "AU",
					three_letter_abbreviation = "AUS",
					full_name_locale = "Australia",
					full_name_english = "Australia"
				},
				new CountryResource()
				{
					id = "CA",
					two_letter_abbreviation = "CA",
					three_letter_abbreviation = "CAN",
					full_name_locale = "Canada",
					full_name_english = "Canada",
					available_regions = new List<RegionResource>()
					{
						new RegionResource()
						{
							id = "77",
							code = "SK",
							name = "Saskatchewan"
						}
					}
				},
				new CountryResource()
				{
					id = "US",
					two_letter_abbreviation = "US",
					three_letter_abbreviation = "USA",
					full_name_locale = "United States",
					full_name_english = "United States",
					available_regions = new List<RegionResource>()
					{
						new RegionResource()
						{
							id = "33",
							code = "MI",
							name = "Michigan"
						}
					}
				}
			};
		}

		public string AuthToken
		{
			get
			{
				return "WHdvc0h9b3NYQW9zWHZvc1h2JSYBAyc_PhRWBSAYLUFsIBkSMEA9MRYVWAcsPCQQaD8NJiBPLSUeFVgXGjEBJA4bX0EaBBtAOR8I";
			}
		}
	}
}
