using System;
using System.Collections.Generic;
using System.Linq;
using user_panel2.Models;
using admin_panel2.Data;

namespace user_panel2.Helpers
{
    internal static class UserManager
    {
        public static User? User { get; set; }

        public static void CreateAccount(string firstName, string lastName, string birth, string email, string password)
        {
            using (var context = new MarketContext())
            {
                var user = context.Users.FirstOrDefault(u => u.Email == email.ToLower().Trim());

                if (user == null)
                {
                    user = new User
                    {
                        Name = firstName,
                        Surname = lastName,
                        DateOfBirth = DateOnly.ParseExact(birth, "dd.MM.yyyy"),
                        Email = email,
                        Password = password
                    };

                    context.Users.Add(user);
                    context.SaveChanges();
                    return;
                }
                throw new Exception("User already exist");
            }
        }

        public static bool Login(string email, string password)
        {
            using (var context = new MarketContext())
            {
                User = context.Users.FirstOrDefault(u => u.Email == email.ToLower().Trim() && u.Password == password);
                return User is not null;
            }
        }

        public static void Logout()
        {
            UserManager.User = null;
        }
    }
}
