using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MagentoConnect.Controllers.EndlessAisle;
using MagentoConnect.Models.EndlessAisle.Availability;
using MagentoConnect.Utilities;

namespace Tests.MockObjects.Controllers.EndlessAisle
{
	public class MockAvailabilityController : IAvailabilityController
	{
		public AvailabilityResource CreateCatalogItem(AvailabilityResource availability)
		{
			return availability;
		}

		public string AuthToken
		{
			get
			{
				return "WHdvc0h9b3NYQW9zWHZvc1h2JSYBAyc_PhRWBSAYLUFsIBkSMEA9MRYVWAcsPCQQaD8NJiBPLSUeFVgXGjEBJA4bX0EaBBtAOR8I";
			}
		}
	}
}
