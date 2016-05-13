using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MagentoConnect.Utilities
{
	class RegexPatterns
	{
		public const string SlugPattern = "^[M]\\d+(-[V]\\d+)?(-[E]\\d+)?(-[R]([A-Z]{2}|[A-Z]{4}))?$";
	}
}
