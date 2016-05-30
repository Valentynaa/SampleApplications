using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//// ReSharper disable InconsistentNaming
namespace MagentoConnect.Models.Magento.Cart
{
	[Serializable]
	public class CurrencyResource
	{
		public string global_currency_code { get; set; }// Global currency code ,
		public string base_currency_code { get; set; }// Base currency code ,
		public string store_currency_code { get; set; }// Store currency code ,
		public string quote_currency_code { get; set; }// Quote currency code ,
		public decimal? store_to_base_rate { get; set; }// Store currency to base currency rate,
		public decimal? store_to_quote_rate { get; set; }// Store currency to quote currency rate,
		public decimal? base_to_global_rate { get; set; }// Base currency to global currency rate,
		public decimal? base_to_quote_rate { get; set; }// Base currency to quote currency rate,
		public object extension_attributes = null;
	}
}
