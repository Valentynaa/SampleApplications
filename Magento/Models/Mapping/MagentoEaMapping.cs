using System;

// ReSharper disable InconsistentNaming

namespace MagentoSync.Models.Mapping
{
    [Serializable]
    public class MagentoEaMapping
    {
        public MagentoEaMapping(int magentoId, int eaId)
        {
            this.magentoId = magentoId;
            this.eaId = eaId;
        }

        public int magentoId { get; set; }
        public int eaId { get; set; }
    }
}