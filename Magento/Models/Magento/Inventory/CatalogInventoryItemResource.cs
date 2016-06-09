using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//// ReSharper disable InconsistentNaming

namespace MagentoSync.Models.Magento.Inventory
{
	public class CatalogInventoryItemResource
	{
		public int? item_id { get; set; }
		public int? product_id { get; set; }
		public int? stock_id { get; set; }//Stock identifier
		public decimal? qty { get; set; }//CAN return as null
		public bool is_in_stock { get; set; }//Stock Availability
		public bool is_qty_decimal { get; set; }
		public bool show_default_notification_message { get; set; }
		public bool use_config_min_qty { get; set; }
		public decimal min_qty { get; set; }//Minimal quantity available for item status in stock
		public int use_config_min_sale_qty { get; set; }
		public decimal min_sale_qty { get; set; }//Minimum Qty Allowed in Shopping Cart or NULL when there is no limitation
		public bool use_config_max_sale_qty { get; set; }
		public decimal max_sale_qty { get; set; } //Maximum Qty Allowed in Shopping Cart data wrapper
		public bool use_config_backorders { get; set; }
		public int backorders { get; set; }//Backorders status
		public bool use_config_notify_stock_qty { get; set; }
		public decimal notify_stock_qty { get; set; }//Notify for Quantity Below data wrapper
		public bool use_config_qty_increments { get; set; }
		public decimal qty_increments { get; set; }//Quantity Increments data wrapper
		public bool use_config_enable_qty_inc { get; set; }
		public bool enable_qty_increments { get; set; } //Whether Quantity Increments is enabled
		public bool use_config_manage_stock { get; set; }
		public bool manage_stock { get; set; }//Can Manage Stock
		public string low_stock_date { get; set; }
		public bool is_decimal_divided { get; set; }
		public int stock_status_changed_auto { get; set; }
		public object extension_attributes = null;
	}
}
