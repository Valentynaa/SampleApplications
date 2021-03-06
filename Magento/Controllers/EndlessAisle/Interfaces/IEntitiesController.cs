using System.Collections.Generic;
using MagentoSync.Models.EndlessAisle.Entities;

namespace MagentoSync.Controllers.EndlessAisle.Interfaces
{
	public interface IEntitiesController : IController
	{
		/// <summary>
		/// Gets information about all supported manufacturers in EA
		/// </summary>
		/// <returns>List of supported Manufacturers in EA</returns>
		List<ManufacturerResource> GetAllManufacturers();

		/// <summary>
		/// Gets the EA location the device is at (from App.config)
		/// </summary>
		/// <returns>EA Location device is at</returns>
		LocationResource GetLocation();
	}
}