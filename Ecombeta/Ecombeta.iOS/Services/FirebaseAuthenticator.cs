using System.Threading.Tasks;
using Firebase.Auth;
using Ecombeta.Services;

namespace XFFirebaseAuthExample.iOS.Services
{
    public class FirebaseAuthenticator : IFirebaseAuthenticator
    {
        public async Task<string> LoginWithEmailPassword(string email, string password)
        {
            AuthDataResult user = await Auth.DefaultInstance.SignInWithPasswordAsync(email, password);
            return await user.User.GetIdTokenAsync(false);
        }
    }
}