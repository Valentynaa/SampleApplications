
namespace MagentoSync.Utilities
{
	public class RegexPatterns
	{
		/// <summary>
		/// Valid slug formats in Endless Aisle
		/// </summary>
		public const string SlugPattern = "[M]\\d+(-[V]\\d+)?(-[E]\\d+)?(-[R]([A-Z]{2}|[A-Z]{4}))?";

		/// <summary>
		/// DateTime.ToString() format for "en-US" culture
		/// </summary>
		public const string TimeStampPattern = "(\\d){1,2}\\/(\\d){1,2}\\/(\\d){4}\\s(\\d){1,2}:(\\d){1,2}:(\\d){1,2}\\s[APap][mM]";
	}
}
