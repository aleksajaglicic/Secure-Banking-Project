using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Common.InMemoryDatabase
{
    public class Database
    {
        public static Dictionary<string, User> Users = new Dictionary<string, User>();

        public Database()
        {
            Users = new Dictionary<string, User>();
        }
        public static bool AddUser(string username, string pin)
        {
            if (Users.ContainsKey(username))
            {
                return false;
            }
            Users.Add(username, new User(username, 0, HashPin(pin)));
            return true;
        }
        public static User GetUser(string username)
        {
            if (!Users.ContainsKey(username))
            {
                return null;
            }
            return Users[username];
        }
        public static bool RemoveUser(string username, string pin)
        {
            if (Users.ContainsKey(username))
            {
                Users.Remove(username);
                return true;
            }
            return false;

        }
        public static bool CheckPin(string username, string pin)
        {
            if (GetUser(username) == null)
                return false;
            if (!Users[username].HashedPin.Equals(HashPin(pin)))
                return false;

            return true;
        }
        public static bool UpdateUsersPin(string username, string oldPin, string newPin)
        {
            if (GetUser(username) != null && Users[username].HashedPin == HashPin(oldPin) && newPin != "")
            {
                Users[username].HashedPin = HashPin(newPin);
                return true;
            }
            return false;
        }
        public static bool DecreaseUsersBalance(User user, int amount)
        {
            if (Users[user.Username].Balance - amount < 0)
            {
                return false;
            }

            Users[user.Username].Balance -= amount;
            return true;
        }
        public static bool IncreaseUsersBalance(User user, int amount)
        {
            Users[user.Username].Balance += amount;
            return true;
        }
        public static string HashPin(string pinCode)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(pinCode));

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    builder.Append(hashBytes[i].ToString("x2"));
                }

                return builder.ToString();
            }
        }
    }
}
