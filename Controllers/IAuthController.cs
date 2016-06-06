using MagentoConnect.Models;

namespace MagentoConnect.Controllers
{
	public interface IAuthController : IController
	{
		string Authenticate(IAuthenticationCredentials credentials);
	}
}