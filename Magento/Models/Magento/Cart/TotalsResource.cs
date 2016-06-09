using System;
using System.Collections.Generic;

// ReSharper disable InconsistentNaming
namespace MagentoSync.Models.Magento.Cart
{
	[Serializable]
	public class TotalsResource
	{
		public decimal? grand_total { get; set; }// Grand total in quote currency,
		public decimal? base_grand_total  { get; set; }// Grand total in base currency,
		public decimal? subtotal { get; set; }// Subtotal in quote currency,
		public decimal? base_subtotal { get; set; }// Subtotal in base currency,
		public decimal? discount_amount { get; set; }// Discount amount in quote currency,
		public decimal? base_discount_amount { get; set; }// Discount amount in base currency,
		public decimal? subtotal_with_discount { get; set; }// Subtotal in quote currency with applied discount,
		public decimal? base_subtotal_with_discount { get; set; }// Subtotal in base currency with applied discount,
		public decimal? shipping_amount { get; set; }// Shipping amount in quote currency,
		public decimal? base_shipping_amount { get; set; }// Shipping amount in base currency,
		public decimal? shipping_discount_amount { get; set; }// Shipping discount amount in quote currency,
		public decimal? base_shipping_discount_amount { get; set; }// Shipping discount amount in base currency,
		public decimal? tax_amount { get; set; }// Tax amount in quote currency,
		public decimal? base_tax_amount { get; set; }// Tax amount in base currency,
		public decimal? weee_tax_applied_amount { get; set; }// Item weee tax applied amount in quote currency. ,
		public decimal? shipping_tax_amount { get; set; }// Shipping tax amount in quote currency,
		public decimal? base_shipping_tax_amount { get; set; }// Shipping tax amount in base currency,
		public decimal? subtotal_incl_tax { get; set; }// Subtotal including tax in quote currency,
		public decimal? base_subtotal_incl_tax { get; set; }// Subtotal including tax in base currency,
		public decimal? shipping_incl_tax { get; set; }// Shipping including tax in quote currency,
		public decimal? base_shipping_incl_tax { get; set; }// Shipping including tax in base currency,
		public string base_currency_code { get; set; }// Base currency code,
		public string quote_currency_code { get; set; }// Quote currency code,
		public string coupon_code { get; set; }// Applied coupon code,
		public int? items_qty { get; set; }// Items qty,
		public IEnumerable<TotalsItemResource> items { get; set; }// Totals by items ,
		public IEnumerable<TotalsSegmentResource> total_segments { get; set; }// Dynamically calculated totals ,
		public object extension_attributes = null;
	}
}