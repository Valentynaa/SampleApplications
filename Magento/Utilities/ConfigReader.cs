using MagentoSync.Models.Mapping;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using MagentoSync.Database;

namespace MagentoSync.Utilities
{
	public static class ConfigReader
	{
		//Mapping dictionaries
		public static readonly List<MagentoEaMapping> ManufacturerMapping;
		public static readonly List<MagentoEaMapping> CategoryMapping;
		public static readonly Dictionary<string, int> FieldMapping;
		public static readonly List<MagentoEaMapping> ColorMapping;

		//Magento values
		public static string MappingCode { get; set; }
		public static string MagentoManufacturerCode { get; set; }
		public static string MagentoCategoryCode { get; set; }
		public static string MagentoDescriptionCode { get; set; }
		public static string MagentoCreatedAtProperty { get; set; }
		public static string MagentoUpdatedAtProperty { get; set; }
		public static string MagentoColorCode { get; set; }
		public static string MagentoMaterialCode { get; set; }
		public static string MagentoImageCode { get; set; }

		public static string MagentoGreaterThanCondition { get; set; }
		public static string MagentoEqualsCondition { get; set; }
		public static string MagentoSearchDateString { get; set; }
		public static string MagentoConfigurableTypeId { get; set; }
		public static string MagentoAttrCodeName { get; set; }
		public static string MagentoNameCode { get; set; }
		public static string MagentoUserName { get; set; }
		public static string MagentoPassword { get; set; }
		public static string MagentoUrl { get; set; }
		public static string MagentoServerPath { get; set; }
		public static int MagentoCustomerId { get; set; }
		public static string MagentoShippingCode { get; set; }
		public static string MagentoPaymentMethod { get; set; }
		public static MediaStorageConfiguration MagentoStorageConfiguration { get; set; }
		public static string MagentoDatabaseServer { get; set; }
		public static string MagentoDatabaseName { get; set; }
		public static string MagentoDatabaseUserId { get; set; }
		public static string MagentoDatabasePassword { get; set; }
		public static int MagentoDatabasePort { get; set; }

		//EA values
		public static int EaClassificationTreeId { get; set; }
		public static string EaClientSecret { get; set; }
		public static string EaClientId  { get; set; }
		public static string EaUsername { get; set; }
		public static string EaPassword { get; set; }
		public static string EaEnviornment { get; set; }
		public static string EaGrantType { get; set; }
		public static int EaCompanyId { get; set; }
		public static int EaLocationId { get; set; }

		static ConfigReader()
		{
			MagentoUrl = ReadFromConfig("Magento_Url");
			MagentoServerPath = ReadFromConfig("Magento_ServerPath");
			MagentoUserName = ReadFromConfig("Magento_Username");
			MagentoPassword = ReadFromConfig("Magento_Password");
			MagentoCustomerId = int.Parse(ReadFromConfig("Magento_CustomerId"));

			EaClientId = ReadFromConfig("EA_ClientId");
			EaClientSecret = ReadFromConfig("EA_ClientSecret");
			EaUsername = ReadFromConfig("EA_Username");
			EaPassword = ReadFromConfig("EA_Password");
			EaEnviornment = ReadFromConfig("EA_Environment");
			EaCompanyId = int.Parse(ReadFromConfig("EA_CompanyId"));
			EaLocationId = int.Parse(ReadFromConfig("EA_LocationId"));

			EaGrantType = ReadFromConfig("EA_GrantType");

			FieldMapping = new Dictionary<string, int>();
			CategoryMapping = new List<MagentoEaMapping>();
			ManufacturerMapping = new List<MagentoEaMapping>();
			ColorMapping = new List<MagentoEaMapping>();

			MagentoStorageConfiguration = bool.Parse(ReadFromConfig("Magento_DatabaseStorageConfiguration"))
				? MediaStorageConfiguration.Database
				: MediaStorageConfiguration.FileSystem;

			if (MagentoStorageConfiguration == MediaStorageConfiguration.Database)
			{
				MagentoDatabaseServer = ReadFromConfig("Magento_DatabaseServer");
				MagentoDatabaseName = ReadFromConfig("Magento_DatabaseName");
				MagentoDatabaseUserId = ReadFromConfig("Magento_DatabaseUserId");
				MagentoDatabasePassword = ReadFromConfig("Magento_DatabasePassword");
				MagentoDatabasePort = int.Parse(ReadFromConfig("Magento_DatabasePort"));
				if (DatabaseConnection.Instance.GetMediaStorageConfiguration() != MediaStorageConfiguration.Database)
				{
					MagentoStorageConfiguration = MediaStorageConfiguration.FileSystem;
					Console.WriteLine("Database storage configuration is set to \"true\" in App.config " +
					                  "\n however the database is not set to this configuration type." +
					                  "\n File System media storage configuration will be used instead.");
				}
			}


			MappingCode = ReadFromConfig("MappingCode");
			MagentoManufacturerCode = ReadFromConfig("Magento_ManufacturerCode");
			MagentoCategoryCode = ReadFromConfig("Magento_CategoryCode");
			MagentoDescriptionCode = ReadFromConfig("Magento_DescriptionCode");
			MagentoCreatedAtProperty = ReadFromConfig("Magento_CreatedAtProperty");
			MagentoUpdatedAtProperty = ReadFromConfig("Magento_UpdatedAtProperty");
			MagentoGreaterThanCondition = ReadFromConfig("Magento_GreaterThanCondition");
			MagentoEqualsCondition = ReadFromConfig("Magento_EqualsCondition");
			MagentoSearchDateString = ReadFromConfig("Magento_SearchDateString");
			MagentoConfigurableTypeId = ReadFromConfig("Magento_ConfigurableTypeId");
			MagentoAttrCodeName = ReadFromConfig("Magento_AttrCodeName");
			MagentoNameCode = ReadFromConfig("Magento_NameCode");
			MagentoColorCode = ReadFromConfig("Magento_ColorCode");
			MagentoMaterialCode = ReadFromConfig("Magento_MaterialCode");
			MagentoImageCode = ReadFromConfig("Magento_ImageCode");
			MagentoShippingCode = ReadFromConfig("Magento_ShippingCode");
			MagentoPaymentMethod = ReadFromConfig("Magento_PaymentMethod");

			EaClassificationTreeId = int.Parse(ReadFromConfig("EA_ClassificationTreeId"));

			var fields = ConfigurationManager.GetSection("FieldMapping") as NameValueCollection;

			if (fields != null)
			{
				foreach (var key in fields.Keys)
				{
					FieldMapping.Add(key.ToString(), int.Parse(fields[key.ToString()]));
				}
			}

			var manufacturers = ConfigurationManager.GetSection("ManufacturerMapping") as NameValueCollection;

			if (manufacturers != null)
			{
				foreach (var key in manufacturers.Keys)
				{
					ManufacturerMapping.Add(new MagentoEaMapping(int.Parse(key.ToString()), int.Parse(manufacturers[key.ToString()])));
				}
			}

			var categories = ConfigurationManager.GetSection("CategoryMapping") as NameValueCollection;

			if (categories != null)
			{
				foreach (var key in categories.Keys)
				{
					CategoryMapping.Add(new MagentoEaMapping(int.Parse(key.ToString()), int.Parse(categories[key.ToString()])));
				}
			}

			var colors = ConfigurationManager.GetSection("ColorMapping") as NameValueCollection;

		    if (colors == null) return;
		    {
		        foreach (var key in colors.Keys)
		        {
		            ColorMapping.Add(new MagentoEaMapping(int.Parse(key.ToString()), int.Parse(colors[key.ToString()])));
		        }
		    }
		}

		/**
		 * Searches mapping dictionary for a magento attribute code to get the EA field definition Id
		 * 
		 * @param       attrCode    Identifier for an attribute in Magento
		 *
		 * @return      int         Id of a FieldDefinition in EA
		 */
		public static int GetValueForField(string attrCode)
		{
			var eaId = -1;

			foreach (var value in FieldMapping.Where(value => value.Key == attrCode))
			{
			    eaId = value.Value;
			}

			return eaId;
		}

		/**
		 * Searches mapping dictionary for a matching magento color
		 * 
		 * @param       magentoColorId      Identifier for color in Magento
		 *
		 * @return      int                 Id of a Color Tag in EA that matches the Magento color for the Product
		 */
		public static int GetMatchingEndlessAisleColor(int magentoColorId)
		{
			var eaId = -1;

			foreach (var value in ColorMapping.Where(value => value.magentoId == magentoColorId))
			{
			    eaId = value.eaId;
			}

			return eaId;
		}

		/**
		 * Searches mapping dictionary for a matching magento category
		 * 
		 * @param       magentoCategoryId   Identifier for category in Magento
		 *
		 * @return      int                 Id of a Classification or Category in EA that matches the Magento category for the Product
		 */
		public static int GetMatchingEndlessAisleCategory(int magentoCategoryId)
		{
			var eaId = -1;

			foreach (var value in CategoryMapping.Where(value => value.magentoId == magentoCategoryId))
			{
			    eaId = value.eaId;
			}

			return eaId;
		}

		/**
		 * Searches mapping dictionary for a matching magento manufacturer
		 * 
		 * @param       magentoManufacturerId   Identifier for manufacturer in Magento
		 *
		 * @return      int                     Id of a Manufacturer in EA that matches the Magento manufacturer for the Product
		 */
		public static int GetMatchingEndlessAisleManufacturer(int magentoManufacturerId)
		{
			var eaId = -1;

			foreach (var value in ManufacturerMapping.Where(value => value.magentoId == magentoManufacturerId))
			{
			    eaId = value.eaId;
			}

			return eaId;
		}

		/**
		 * Returns a string read from the App.config
		 * This utility is used as an exception wrapper
		 * 
		 * @param   configKey    Key value of an App.config entry to read
		 * @return  value        The value of the App.config entry read
		 */
		private static string ReadFromConfig(string configKey)
		{
			var value = ConfigurationManager.AppSettings[configKey];

			if (string.IsNullOrEmpty(value))
			{
				throw new Exception(string.Format("Unable to read value {0} from App.config", configKey));
			}

			return value;
		}
	}
}
