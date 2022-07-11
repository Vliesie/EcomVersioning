using System.Threading.Tasks;

namespace Ecombeta.Services
{
    public interface IFirebaseAuthenticator
    {
        Task<string> LoginWithEmailPassword(string email, string password);
    }
}