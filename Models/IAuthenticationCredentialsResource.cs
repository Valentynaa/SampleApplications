namespace MagentoConnect.Models
{
	public interface IAuthenticationCredentials
	{
		string grant_type { get; set; }
		string username { get; set; }
		string password { get; set; }
		string client_id { get; set; }
		string client_secret { get; set; }
	}
}