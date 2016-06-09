using System;

namespace MagentoSync.Models.EndlessAisle.Availability
{
    public class AvailabilityResource
    {
        public AvailabilityResource()
        {
            IsSerialized = false;
            IsDropShipable = false;
        }

        public Guid Id { get; set; }
        public int EntityId { get; set; }
        public int Quantity { get; set; }
        public bool IsSerialized { get; set; }
        public bool IsDropShipable { get; set; }
    }
}