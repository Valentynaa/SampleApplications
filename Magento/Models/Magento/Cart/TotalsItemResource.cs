using System;
// ReSharper disable InconsistentNaming
namespace MagentoSync.Models.Magento.Cart
{
	[Serializable]
	public class TotalsItemResource
	{
		public int item_id { get; set; } // Item id,
		public decimal price { get; set; } // Item price in quote currency. ,
		public decimal base_price { get; set; } // Item price in base currency. ,
		public decimal qty { get; set; } // Item quantity. ,
		public decimal row_total { get; set; } // Row total in quote currency. ,
		public decimal base_row_total { get; set; } // Row total in base currency. ,
		public decimal? row_total_with_discount { get; set; } // Row total with discount in quote currency.Otherwise, null. ,
		public decimal? tax_amount { get; set; } // Tax amount in quote currency.Otherwise, null. ,
		public decimal? base_tax_amount { get; set; } // Tax amount in base currency.Otherwise, null. ,
		public decimal? tax_percent { get; set; } // Tax percent.Otherwise, null. ,
		public decimal? discount_amount { get; set; } // Discount amount in quote currency.Otherwise, null. ,
		public decimal? base_discount_amount { get; set; } // Discount amount in base currency.Otherwise, null. ,
		public decimal? discount_percent { get; set; } // Discount percent.Otherwise, null. ,
		public decimal? price_incl_tax { get; set; } // Price including tax in quote currency.Otherwise, null. ,
		public decimal? base_price_incl_tax { get; set; } // Price including tax in base currency.Otherwise, null. ,
		public decimal? row_total_incl_tax { get; set; } // _row total including tax in quote currency.Otherwise, null. ,
		public decimal? base_row_total_incl_tax { get; set; } // Row total including tax in base currency.Otherwise, null. ,
		public string options { get; set; } // Item price in quote currency. ,
		public decimal? weee_tax_applied_amount { get; set; } // Item weee tax applied amount in quote currency. ,
		public string weee_tax_applied { get; set; } // Item weee tax applied in quote currency. ,
		public object extension_attributes = null;
		public string name { get; set; } // Product name.Otherwise, null.
	}
}