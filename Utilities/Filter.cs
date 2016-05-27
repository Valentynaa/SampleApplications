using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Newtonsoft.Json;

namespace MagentoConnect.Utilities
{
	public enum FilterCondition
	{
		[Description("gt")]
		GreaterThan,
		[Description("ge")]
		GreaterThanOrEqual,
		[Description("lt")]
		LessThan,
		[Description("le")]
		LessThanOrEqual
	}
	public class Filter
	{
		private const string JsonDateTimeString = "datetime";

		public string Property { get; set; }
		public object Value { get; set; }
		public FilterCondition Condition { get; set; }

		public Filter(string property, object value, FilterCondition condition)
		{
			Property = property;
			Value = value;
			Condition = condition;
		}
		
		/// <summary>
		/// Filter string only works if the value type is DateTime, int or string.
		/// If the Value is not one of these types, then the string behaviour is used.
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			string valueString;
			if (Value is int)
			{
				valueString = Value.ToString();
			}
			else if (Value is DateTime)
			{
				//Trim the " at the start and end of the serialized object
				valueString = string.Format("{0}\'{1}\'", JsonDateTimeString, JsonConvert.SerializeObject(Value).Substring(1, JsonConvert.SerializeObject(Value).Length - 2));
			}
			else
			{
				valueString = string.Format("\'{0}\'", Value);
			}

			return string.Format("{0} {1} {2}", Property, Enums.GetDescription(Condition), valueString);
		}
	}
}
