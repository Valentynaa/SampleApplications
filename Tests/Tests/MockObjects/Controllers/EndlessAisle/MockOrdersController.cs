using MagentoConnect.Controllers.EndlessAisle;
using MagentoConnect.Models.EndlessAisle.Orders;
using MagentoConnect.Utilities;
using System;
using System.Collections.Generic;

namespace Tests.MockObjects.Controllers.EndlessAisle
{
	public class MockOrdersController : BaseMockController, IOrdersController
	{
		/// <summary>
		/// Number of items in the GetOrderItems function stub
		/// </summary>
		public static int OrderItemCount
		{
			get
			{
				return 2;
			}
		}

		public IEnumerable<OrderResource> GetOrders(Filter filter = null)
		{
			return new List<OrderResource>()
			{
				new OrderResource()
				{
					Id = new Guid(),
					OrderTypeId = 1,
					OrderType = "Sales", 
					State = OrderState.Completed,
					PrintableId = "7400009",
					Name = "iPhone 5 Order",
					TenderId = "TT101IN18",
					EntityId = 14223,
					ShippingEntityId = 0,
					CustomerId = new Guid(),
					BillingCustomerId = new Guid(),
					ShippingCustomerId = new Guid(),
					ShippingAddressId = new Guid(),
					BillingAddressId = new Guid(),
					EmployeeId = 166151,
					CreatedDateUtc = DateTime.Now,
					OrderExpiryHours = 72
				},
				new OrderResource()
				{
					Id = new Guid(),
					OrderTypeId = 1,
					OrderType = "Sales",
					State = OrderState.Completed,
					PrintableId = "2042068",
					Name = "Samsung Battery Order",
					TenderId = "TT101IN18",
					EntityId = 14223,
					ShippingEntityId = 0,
					CustomerId = new Guid(),
					BillingCustomerId = new Guid(),
					ShippingCustomerId = new Guid(),
					ShippingAddressId = new Guid(),
					BillingAddressId = new Guid(),
					EmployeeId = 166151,
					CreatedDateUtc = DateTime.Now,
					OrderExpiryHours = 72
				}
			};
		}

		public IEnumerable<OrderItemResource> GetOrderItems(string orderId)
		{
			return new List<OrderItemResource>()
			{
				new OrderItemResource()
				{
					Id = new Guid(),
					OrderId = new Guid(orderId),
					ItemTypeId = 1,
					ItemType = "DropShip",
					ItemStatusId = 3,
					ItemStatus = "Processed",
					ProductId = "b85cb879-bb5f-4847-a856-8287de0a92d5",
					SupplierEntityId = 14107,
					Quantity = 1,
					Cost = new decimal(20.49),
					ListPrice = new decimal(39.99),
					SellingPrice = new decimal(39.99),
					Index = 1,
					Description = "Samsung Galaxy S4 Standard Battery",
					SKU = "B00LAOKN45",
					SerialNumbers = new List<string>(),
					TrackingInformation = new List<TrackingInformationResource>(),
					ShippingOptionId = "1716"
				},
				new OrderItemResource()
				{
					Id = new Guid(),
					OrderId = new Guid(orderId),
					ItemTypeId = 4,
					ItemType = "Shipping",
					ItemStatusId = 15,
					ItemStatus = "New",
					ProductId = null,
					SupplierEntityId = 14107,
					Quantity = 1,
					Cost = new decimal(0),
					ListPrice = new decimal(12.16),
					SellingPrice = new decimal(12.16),
					Index = 2,
					Description = "Expected Parcel",
					SKU = null,
					SerialNumbers = new List<string>(),
					TrackingInformation = new List<TrackingInformationResource>(),
					ShippingOptionId = "1716"
				}
			};
		}
	}
}
