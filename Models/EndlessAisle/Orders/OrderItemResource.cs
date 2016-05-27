using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagentoConnect.Models.EndlessAisle.Orders
{
	[Serializable]
	public class OrderItemResource
	{
		/// <summary>
		/// Unique Identifier for the item.
		/// </summary>
		public Guid Id { get; set; }

		/// <summary>
		/// Unique Identifier for the order associated with this item.
		/// </summary>
		public Guid OrderId { get; set; }

		/// <summary>
		/// The id of the ItemType.
		/// </summary>
		public int ItemTypeId { get; set; }

		/// <summary>
		/// A read-only description of the ItemType. For example, sales, drop ship, etc.
		/// </summary>
		public string ItemType { get; set; }

		/// <summary>
		/// The id of the ItemStatus.
		/// </summary>
		public int ItemStatusId { get; set; }

		/// <summary>
		/// A read-only description of the ItemStatus. For example, new, processed, ordered, etc.
		/// </summary>
		public string ItemStatus { get; set; }

		/// <summary>
		/// The retailer catalog product identifier for this item.
		/// </summary>
		public string ProductId { get; set; }

		/// <summary>
		/// EntityId for the vendor from the entity store.
		/// </summary>
		public int SupplierEntityId { get; set; }

		/// <summary>
		/// Quantity of items added to the Order.
		/// </summary>
		public int Quantity { get; set; }

		/// <summary>
		/// The vendor cost for a single item.
		/// </summary>
		public decimal Cost { get; set; }

		/// <summary>
		/// The original list price for a single item.
		/// </summary>
		public decimal ListPrice { get; set; }

		/// <summary>
		/// The selling price for a single item.
		/// </summary>
		public decimal SellingPrice { get; set; }

		/// <summary>
		/// Sort order for the items in the Order.
		/// </summary>
		public int Index { get; set; }

		/// <summary>
		/// A physical description of the item.
		/// </summary>
		public string Description { get; set; }

		/// <summary>
		/// The product SKU for the item.
		/// </summary>
		public string SKU { get; set; }

		/// <summary>
		/// A generic notes field.
		/// </summary>
		public string Notes { get; set; }

		/// <summary>
		/// SerialNumber of the item.
		/// </summary>
		public List<string> SerialNumbers { get; set; }

		/// <summary>
		/// A unique identifier provided by the supplier.
		/// </summary>
		public string SupplierReference { get; set; }

		/// <summary>
		/// A list of Tracking information for the item.
		/// </summary>
		public List<TrackingInformationResource> TrackingInformation { get; set; }

		/// <summary>
		/// A Unique identifier for the shipping option
		/// </summary>
		public String ShippingOptionId { get; set; }
	}
}
