using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MagentoConnect.Utilities
{
	/// <summary>
	/// Enum for different types of Logging
	/// </summary>
	public enum Log
	{
		[PathValue("\\syncLog.txt")]
		Sync,
		[PathValue("\\errorLog.txt")]
		Error
	}

	public enum OrderState
	{
		/// <summary>
		/// New Order created.
		/// links/actions:
		///     AddItems -> /Orders(123)/Items
		///     DeleteOrder -> /Orders(123)/Delete
		///     UpdateOrder -> /Orders(123)
		/// </summary>
		Created,

		/// <summary>
		/// links/actions:
		///     AddItems -> /Orders(123)/Items
		///     RemoveItems -> /Orders(123)/Items
		///     ProcessOrder -> /Orders(123)/Process
		///     CancelOrder -> /Orders(123)/Cancel
		///     DeleteOrder -> /Orders(123)/Delete
		///     UpdateOrder -> /Orders(123)
		/// </summary>
		Pending,

		/// <summary>
		/// Order processed, no action links.
		/// </summary>
		Processed,

		/// <summary>
		/// Order cancelled, no action links.
		/// </summary>
		Cancelled,

		/// <summary>
		/// Order completed, no action links.
		/// </summary>
		Completed
	}

	#region Enum Attributes
	//Add new enum attributes here

	/// <summary>
	/// PathValueAttribute for enums. This Attribute will append the value to the path to the executable.
	/// </summary>
	public class PathValueAttribute : Attribute
	{
		public PathValueAttribute(string value)
		{
			Value = string.Format("{0} {1}", Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), value); ;
		}
		public string Value { get; }
	}

	#endregion

	class Enums
	{
		/// <summary>
		/// Returns the Value associated with the PathValueAttribute of the specified Enum
		/// http://blog.spontaneouspublicity.com/associating-strings-with-enums-in-c
		/// </summary>
		/// <param name="value">Enum to get the Path from</param>
		/// <returns>The PathValue or value.ToString if the Attribute was not found</returns>
		public static string GetPath(Enum value)
		{
			FieldInfo fi = value.GetType().GetField(value.ToString());

			PathValueAttribute[] attributes =
				(PathValueAttribute[])fi.GetCustomAttributes(typeof(PathValueAttribute), false);

			if (attributes != null && attributes.Length > 0)
				return attributes[0].Value;
			else
				return value.ToString();
		}

		public static string GetDescription(Enum value)
		{
			FieldInfo fi = value.GetType().GetField(value.ToString());

			DescriptionAttribute[] attributes =
				(DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

			if (attributes != null && attributes.Length > 0)
				return attributes[0].Description;
			else
				return value.ToString();
		}
	}
}
