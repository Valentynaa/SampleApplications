﻿using System;
using System.Collections.Generic;

//// ReSharper disable InconsistentNaming
namespace MagentoConnect.Models.Magento.Cart
{
	[Serializable]
	public class DownloadableOptionResource
	{
		public IEnumerable<int> downloadable_links { get; set; }//The list of downloadable links
	}
}