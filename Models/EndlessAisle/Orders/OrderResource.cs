using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MagentoConnect.Utilities;

namespace MagentoConnect.Models.EndlessAisle.Orders
{
	[Serializable]
	public class OrderResource
	{
		/// <summary>
		/// Unique Identifier for the order.
		/// </summary>
		public Guid Id { get; set; }

		/// <summary>
		/// The id of the OrderType. For example, sales, drop ship, etc.
		/// </summary>
		public int OrderTypeId { get; set; }

		/// <summary>
		/// A read-only description of the OrderType. For example, sales, drop ship, etc.
		/// </summary>
		public string OrderType { get; set; }

		/// <summary>
		/// The state the order is currently in.
		/// </summary>
		public OrderState State { get; set; }

		/// <summary>
		/// A unique identifier that can be printed on an invoice or searched in the system.
		/// </summary>
		public string PrintableId { get; set; }

		/// <summary>
		/// The Name of the Order. Used for identifying/differentiating one order from another.
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// The Tender Id from from the tendering system.
		/// </summary>
		public string TenderId { get; set; }

		/// <summary>
		/// The Tender Origin from from the tendering system.
		/// </summary>
		public string TenderOrigin { get; set; }

		/// <summary>
		/// The Id from the source service creating the order.
		/// </summary>
		public string SourceId { get; set; }

		/// <summary>
		/// The name of the source service creating the order.
		/// </summary>
		public string SourceName { get; set; }

		/// <summary>
		/// Value from Entity Manager that indicates the physical location.
		/// </summary>
		public int EntityId { get; set; }

		/// <summary>
		/// An identifier for the physical location if the order is to be shipped to a store.
		/// </summary>
		public int ShippingEntityId { get; set; }

		/// <summary>
		/// Id of billing and shipping customer when only one CustomerId is submitted.
		/// </summary>
		public Guid CustomerId { get; set; }

		/// <summary>
		/// CustomerId of customer to be billed for the order
		/// </summary>
		public Guid BillingCustomerId { get; set; }

		/// <summary>
		/// CusotmerId of the customer to receive the order
		/// </summary>
		public Guid ShippingCustomerId { get; set; }

		/// <summary>
		/// ShippingAddressId of the Customer who created the order.
		/// </summary>
		public Guid ShippingAddressId { get; set; }

		/// <summary>
		/// BillingAddressId of the customer who created the order.
		/// </summary>
		public Guid BillingAddressId { get; set; }

		/// <summary>
		/// Id of the employee associated with the order.
		/// </summary>
		public int EmployeeId { get; set; }

		/// <summary>
		/// The discount code for a discount applied to the order.
		/// </summary>
		public string DiscountCode { get; set; }

		/// <summary>
		/// A description of the discount.
		/// </summary>
		public string DiscountDescription { get; set; }

		/// <summary>
		/// The value of the discount to be applied at the order level.
		/// </summary>
		public decimal DiscountAmount { get; set; }

		/// <summary>
		/// UTC creation date of the Order.
		/// </summary>
		public DateTime CreatedDateUtc { get; set; }

		/// <summary>
		/// Number of hours before the order expires. If nothing is provided use the system default.
		/// </summary>
		public int OrderExpiryHours { get; set; }

		/// <summary>
		/// UTC date the Order is set to Expire.
		/// </summary>
		public DateTime OrderExpiryDate { get; set; }
	}
}
