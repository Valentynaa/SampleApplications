using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Linq;
using MagentoSync;
using MagentoSync.Controllers.EndlessAisle;
using MagentoSync.Controllers.Magento;
using MagentoSync.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Configuration
{
    /// <summary>
    /// This test suite ensures the values in your App.config are configured properly
    /// </summary>
    [TestClass]
    public class Configuration
    {
        private FieldDefinitionController _eaFieldDefinitionController;
        private EntitiesController _eaEntitiesController;
        private ClassificationTreeController _eaClassificationController;
        private ProductLibraryController _eaProductLibraryController;
        private CustomAttributesController _magentoCustomAttributeController;
        private CategoryController _magentoCategoryController;

        [TestInitialize]
        public void SetUp()
        {
            var eaAuthToken = App.GetEaAuthToken();
            var magentoAuthToken = App.GetMagentoAuthToken();

            _eaFieldDefinitionController = new FieldDefinitionController(eaAuthToken);
            _eaEntitiesController = new EntitiesController(eaAuthToken);
            _eaClassificationController = new ClassificationTreeController(eaAuthToken);
            _eaProductLibraryController = new ProductLibraryController(eaAuthToken);

            _magentoCustomAttributeController = new CustomAttributesController(magentoAuthToken);
            _magentoCategoryController = new CategoryController(magentoAuthToken);
        }

        /// <summary>
        /// If this test fails, you have a value missing from App.config
        /// </summary>
        [TestMethod]
        public void RequiredValuesAreNotNull()
        {
            Assert.IsNotNull(ConfigurationManager.AppSettings["EA_Environment"]);
            Assert.IsNotNull(ConfigurationManager.AppSettings["EA_ClientSecret"]);
            Assert.IsNotNull(ConfigurationManager.AppSettings["EA_ClientId"]);
            Assert.IsNotNull(ConfigurationManager.AppSettings["EA_Username"]);
            Assert.IsNotNull(ConfigurationManager.AppSettings["EA_Password"]);
            Assert.IsNotNull(ConfigurationManager.AppSettings["EA_CompanyId"]);
            Assert.IsNotNull(ConfigurationManager.AppSettings["EA_LocationId"]);
            Assert.IsNotNull(ConfigurationManager.AppSettings["EA_ClassificationTreeId"]);
            Assert.IsNotNull(ConfigurationManager.AppSettings["EA_GrantType"]);
            Assert.IsNotNull(ConfigurationManager.AppSettings["MappingCode"]);
            Assert.IsNotNull(ConfigurationManager.AppSettings["Magento_Url"]);
            Assert.IsNotNull(ConfigurationManager.AppSettings["Magento_Username"]);
            Assert.IsNotNull(ConfigurationManager.AppSettings["Magento_Password"]);
            Assert.IsNotNull(ConfigurationManager.AppSettings["Magento_ManufacturerCode"]);
            Assert.IsNotNull(ConfigurationManager.AppSettings["Magento_CategoryCode"]);
            Assert.IsNotNull(ConfigurationManager.AppSettings["Magento_DescriptionCode"]);
            Assert.IsNotNull(ConfigurationManager.AppSettings["Magento_CreatedAtProperty"]);
            Assert.IsNotNull(ConfigurationManager.AppSettings["Magento_UpdatedAtProperty"]);
            Assert.IsNotNull(ConfigurationManager.AppSettings["Magento_GreaterThanCondition"]);
            Assert.IsNotNull(ConfigurationManager.AppSettings["Magento_SearchDateString"]);
            Assert.IsNotNull(ConfigurationManager.AppSettings["Magento_ConfigurableTypeId"]);
            Assert.IsNotNull(ConfigurationManager.AppSettings["Magento_AttrCodeName"]);
            Assert.IsNotNull(ConfigurationManager.AppSettings["Magento_NameCode"]);
            Assert.IsNotNull(ConfigurationManager.AppSettings["Magento_ColorCode"]);
            Assert.IsNotNull(ConfigurationManager.AppSettings["Magento_MaterialCode"]);
            Assert.IsNotNull(ConfigurationManager.AppSettings["Magento_ImageCode"]);

            Assert.IsNotNull(ConfigurationManager.GetSection("FieldMapping"));
            Assert.IsNotNull(ConfigurationManager.GetSection("ManufacturerMapping"));
            Assert.IsNotNull(ConfigurationManager.GetSection("CategoryMapping")); 
        }

        /// <summary>
        /// If this test fails, the mapping attribute code provided is invalid
        /// </summary>
        [TestMethod]
        public void MappingAttributeExists()
        {
            var mappingAttrCode = ConfigReader.MappingCode;

            Assert.IsNotNull(mappingAttrCode);
            Assert.IsNotNull(_magentoCustomAttributeController.GetCustomAttributeIfExists(mappingAttrCode));
        }

        /// <summary>
        /// If this test fails, the specified file path in App.config does not exist
        /// </summary>
        [TestMethod]
        public void FilePathIsValid()
        {
            Assert.IsTrue(Directory.Exists(new UrlFormatter().MagentoCatalogAssetPath(ConfigReader.MagentoServerPath)));
        }

        /// <summary>
        /// If this test fails, there is a problem with FieldMappings in App.config
        /// </summary>
        [TestMethod]
        public void FieldMappings_AreValid()
        {
            var fields = ConfigurationManager.GetSection("FieldMapping") as NameValueCollection;

            Assert.IsNotNull(fields);

            foreach (var key in fields.Keys)
            {
                Assert.IsNotNull(key);
                Assert.IsNotNull(fields[key.ToString()]);
                Assert.IsNotNull(int.Parse(fields[key.ToString()]));

                Assert.IsNotNull(_eaFieldDefinitionController.GetAFieldDefinition(int.Parse(fields[key.ToString()])));
                Assert.IsNotNull(_magentoCustomAttributeController.GetCustomAttributeIfExists(key.ToString()));
            }
        }

        /// <summary>
        /// If this test fails, there is a problem with ManufacturerMappings in App.config
        /// </summary>
        [TestMethod]
        public void ManufacturerMappings_AreValid()
        {
            var manufacturers = ConfigurationManager.GetSection("ManufacturerMapping") as NameValueCollection;
            var eaManufacturers = _eaEntitiesController.GetAllManufacturers();

            Assert.IsNotNull(manufacturers);
            Assert.IsNotNull(eaManufacturers);

            var magentoManufacturerAttr =
                _magentoCustomAttributeController.GetCustomAttributeIfExists(ConfigReader.MagentoManufacturerCode);

            foreach (var key in manufacturers.Keys)
            {
                Assert.IsNotNull(key);
                Assert.IsNotNull(manufacturers[key.ToString()]);
                Assert.IsNotNull(int.Parse(manufacturers[key.ToString()]));

                Assert.IsNotNull(magentoManufacturerAttr.options.Where(option => option.value == key.ToString()));
                Assert.IsNotNull(eaManufacturers.Where(manufacturer => manufacturer.Id == int.Parse(manufacturers[key.ToString()])));
            }
        }

        /// <summary>
        /// If this test fails, there is a problem with CategoryMappings in App.config
        /// </summary>
        [TestMethod]
        public void CategoryMappings_AreValid()
        {
            var categories = ConfigurationManager.GetSection("CategoryMapping") as NameValueCollection;

            Assert.IsNotNull(categories);

            foreach (var key in categories.Keys)
            {
                Assert.IsNotNull(key);
                Assert.IsNotNull(categories[key.ToString()]);
                Assert.IsNotNull(int.Parse(categories[key.ToString()]));

                Assert.IsNotNull(_eaClassificationController.GetClassificationById(ConfigReader.EaClassificationTreeId, int.Parse(categories[key.ToString()])));
                Assert.IsNotNull(_magentoCategoryController.GetCategory(int.Parse(key.ToString())));
            }      
        }

        /// <summary>
        /// If this test fails, there is a problem with ColorMappings in App.config
        /// </summary>
        [TestMethod]
        public void ColorMappings_AreValid()
        {
            var colors = ConfigurationManager.GetSection("ColorMapping") as NameValueCollection;

            var magentoColorAttr =
                _magentoCustomAttributeController.GetCustomAttributeIfExists(ConfigReader.MagentoColorCode);

            var eaColors = _eaProductLibraryController.GetColorTags().ColorTags;

            Assert.IsNotNull(colors);

            foreach (var key in colors.Keys)
            {
                Assert.IsNotNull(key);
                Assert.IsNotNull(colors[key.ToString()]);
                Assert.IsNotNull(int.Parse(colors[key.ToString()]));

                Assert.IsNotNull(magentoColorAttr.options.Where(option => option.value == key.ToString()));
                Assert.IsNotNull(eaColors.Where(eaColor => eaColor.Id == int.Parse(key.ToString())));
            }
        }
    }
}
