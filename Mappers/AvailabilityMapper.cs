using MagentoConnect.Controllers.EndlessAisle;
using MagentoConnect.Models.EndlessAisle.Availability;
using MagentoConnect.Utilities;
using System;

namespace MagentoConnect.Mappers
{
    class AvailabilityMapper : BaseMapper
    {
        private static AvailabilityController _eaAvailabilityController;

        public AvailabilityMapper(string magentoAuthToken, string eaAuthToken) : base(magentoAuthToken, eaAuthToken)
        {
            _eaAvailabilityController = new AvailabilityController(eaAuthToken);
        }

        /**
         * This function exists only because as of the creation of this app, EA requires products to have availability
         * Before they can show up. This function creates an availability record of 1 for a product across the company
         * Allowing a product to appear in EA, passing the "do not display out of stock products" rule
         * 
         * @param   catalogItemId            Slug of a Product in EA
         */
        public void CreateAvailabilityForCatalogItem(string catalogItemId)
        {
            var availability = new AvailabilityResource
            {
                Id = new Guid(catalogItemId),
                EntityId = ConfigReader.EaLocationId,
                Quantity = 1
            };

            _eaAvailabilityController.CreateCatalogItem(availability);
        }


    }
}
