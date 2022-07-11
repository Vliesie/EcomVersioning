using System.Threading.Tasks;
using Ecombeta.Services;
using Firebase.Auth;

namespace XFFirebaseAuthExample.iOS.Services
{
    public class FirebaseAuthenticator : IFirebaseAuthenticator
    {
        public async Task<string> LoginWithEmailPassword(string email, string password)
        {
            var user = await Auth.DefaultInstance.SignInWithPasswordAsync(email, password);
            return await user.User.GetIdTokenAsync(false);
        }
    }
}