using MagentoSync.Models;

namespace MagentoSync.Controllers
{
	public interface IAuthController : IController
	{
		string Authenticate(IAuthenticationCredentials credentials);
	}
}