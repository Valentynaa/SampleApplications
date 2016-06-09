using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//// ReSharper disable InconsistentNaming
namespace MagentoSync.Models.Magento.Country
{
	[Serializable]
	public class CountryResource
	{
		public string id { get; set; }// The country id for the store. ,
		public string two_letter_abbreviation { get; set; }// The country 2 letter abbreviation for the store. ,
		public string three_letter_abbreviation { get; set; }// The country 3 letter abbreviation for the store. ,
		public string full_name_locale { get; set; }// The country full name(in store locale) for the store. ,
		public string full_name_english { get; set; }// The country full name(in English) for the store. ,
		public List<RegionResource> available_regions { get; set; }// The available regions for the store. ,
		public object extension_attributes = null;
	}
}
