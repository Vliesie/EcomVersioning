//using Firebase.Database;
//using Firebase.Database.Query;
//using System;
//using System.Collections.Generic;
//using System.Diagnostics;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Ecombeta.Models;

//namespace Ecombeta.ViewModel
//{
//    public class FirebaseHelper
//    {
//        public static FirebaseClient firebase = new FirebaseClient("https://mica-marketapp.firebaseio.com");

//        //Read All
//        public static  async Task<List<Users>> GetAllUser()
//        {
//            try
//            {
//                var userlist = (await firebase
//                .Child("Users")
//                .OnceAsync<Users>()).Select(item =>
//                new Users
//                {
//                    Usersname = item.Object.Usersname,
//                    Password = item.Object.Password
//                }).ToList();
//                return userlist;
//            }
//            catch (Exception e)
//            {
//                Debug.WriteLine($"Error:{e}");
//                return null;
//            }
//        }

//        //Read 
//        public static async Task<Users> GetUser(string email)
//        {
//            try
//            {
//                var allUsers = await GetAllUser();
//                await firebase
//                .Child("Users")
//                .OnceAsync<Users>();
//                return allUsers.Where(a => a.Usersname == email).FirstOrDefault();
//            }
//            catch (Exception e)
//            {
//                Debug.WriteLine($"Error:{e}");
//                return null;
//            }
//        }

//        //Inser a user
//        public static async Task<bool> AddUser(string email, string password)
//        {
//            try
//            {


//                await firebase
//                .Child("Users")
//                .PostAsync(new Users() { Usersname = email, Password = password });
//                return true;
//            }
//            catch (Exception e)
//            {
//                Debug.WriteLine($"Error:{e}");
//                return false;
//            }
//        }

//        //Update 
//        public static async Task<bool> UpdateUser(string email, string password)
//        {
//            try
//            {


//                var toUpdateUser = (await firebase
//                .Child("Users")
//                .OnceAsync<Users>()).Where(a => a.Object.Usersname == email).FirstOrDefault();
//                await firebase
//                .Child("Users")
//                .Child(toUpdateUser.Key)
//                .PutAsync(new Users() { Usersname = email, Password = password });
//                return true;
//            }
//            catch (Exception e)
//            {
//                Debug.WriteLine($"Error:{e}");
//                return false;
//            }
//        }

//        //Delete User
//        public static async Task<bool> DeleteUser(string email)
//        {
//            try
//            {


//                var toDeletePerson = (await firebase
//                .Child("Users")
//                .OnceAsync<Users>()).Where(a => a.Object.Usersname == email).FirstOrDefault();
//                await firebase.Child("Users").Child(toDeletePerson.Key).DeleteAsync();
//                return true;
//            }
//            catch (Exception e)
//            {
//                Debug.WriteLine($"Error:{e}");
//                return false;
//            }
//        }

//    }
//}

