
using admin_panel2.Data;
using user_panel2.Models;
using System.Linq;
using System.Threading;
using System.Globalization;

using Microsoft.EntityFrameworkCore;

namespace user_panel
{
    internal class Program
    {
        static List<Category> categories = new List<Category>();
        static List<Product> Basket = new List<Product>();

        public static void CreateAccountPage()
        {
            while (true)
            {
                Console.WriteLine("Create account");
                Console.Write("First name: ");
                var firstName = Console.ReadLine();
                Console.Write("Last name: ");
                var lastName = Console.ReadLine();
                Console.Write("Date of birth (dd.MM.yyyy): ");
                var birth = Console.ReadLine();
                Console.Write("Email: ");
                var email = Console.ReadLine();
                Console.Write("Password: ");
                var password = Console.ReadLine();

                try
                {
                    user_panel2.Helpers.UserManager.CreateAccount(firstName!, lastName!, birth!, email!.ToLower().Trim(), password!);
                    break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        public static bool LoginPage()
        {
            while (true)
            {
                Console.WriteLine("Login");
                Console.Write("Email: ");
                var loginEmail = Console.ReadLine();
                Console.Write("Password: ");
                var loginPassword = Console.ReadLine();

                if (user_panel2.Helpers.UserManager.Login(loginEmail!, loginPassword!))
                {
                    Console.WriteLine("Login successful!");

                    if (user_panel2.Helpers.UserManager.User is not null)
                    {
                        Console.WriteLine($"Welcome {user_panel2.Helpers.UserManager.User.Name}!");
                        Thread.Sleep(2000);
                        Console.Clear();
                    }
                    return true;
                    
                }
                else
                {
                    Console.WriteLine("Invalid email or password");
                }
            }
        }

        public static void LoginMenu()
        {
            while (true)
            {
                Console.WriteLine("1. Profile");
                Console.WriteLine("2. Categories");
                Console.WriteLine("3. Basket");
                Console.WriteLine("4. Logout");

                Console.Write("Make choice: ");
                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        Console.Clear();
                        ShowProfile();
                        break;
                    case "2":
                        Console.Clear();
                        ShowCategories();
                        break;
                    case "3":
                        Console.Clear();
                        ShowBasket();
                        break;
                    case "4":
                        Thread.Sleep(1000);  
                        Environment.Exit(0); 
                        break;
                    default:
                        Console.WriteLine("Invalid choice");
                        break;
                }
            }
        }

        public static void MainPage()
        {
            Console.WriteLine("1. Create account");
            Console.WriteLine("2. Login");
        }

        public static void UpdateProfile()
        {
            Console.WriteLine("Update profile");
            Console.WriteLine("1. Update first name");
            Console.WriteLine("2. Update last name");
            Console.WriteLine("3. Update Date of Birth");
            Console.WriteLine("4. Update email");
            Console.WriteLine("5. Return");
            Console.Write("Make choice: ");
            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    Console.Clear();
                    Console.Write("New first name: ");
                    var newName = Console.ReadLine();
                    if (newName == user_panel2.Helpers.UserManager.User.Name)
                    {
                        Console.WriteLine("New first name is same with previous one");
                    }
                    else
                    {
                        user_panel2.Helpers.UserManager.User.Name = newName;
                        Console.WriteLine("First name updated");
                    }
                    break;

                case "2":
                    Console.Clear();
                    Console.Write("New last name: ");
                    var newSurname = Console.ReadLine();
                    if (newSurname == user_panel2.Helpers.UserManager.User.Surname)
                    {
                        Console.WriteLine("New last name is same with previous one");
                    }
                    else
                    {
                        user_panel2.Helpers.UserManager.User.Surname = newSurname;
                        Console.WriteLine("Last name updated");
                    }
                    break;

                case "3":
                    Console.Clear();
                    Console.Write("New Date of Birth (dd.MM.yyyy): ");
                    if (DateOnly.TryParseExact(Console.ReadLine(), "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateOnly dateOfBirth))
                    {
                        if (dateOfBirth == user_panel2.Helpers.UserManager.User.DateOfBirth)
                        {
                            Console.WriteLine("New Date of Birth is same with previous one");
                        }
                        else
                        {
                            user_panel2.Helpers.UserManager.User.DateOfBirth = dateOfBirth;
                            Console.WriteLine("Date of Birth updated");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid input");
                    }
                    break;

                case "4":
                    Console.Clear();
                    Console.Write("New email: ");
                    var newEmail = Console.ReadLine();
                    if (newEmail.ToLower().Trim() == user_panel2.Helpers.UserManager.User.Email)
                    {
                        Console.WriteLine("New email is the same with previous one");
                    }
                    else
                    {
                        user_panel2.Helpers.UserManager.User.Email = newEmail.ToLower().Trim();
                        Console.WriteLine("Email updated");
                    }
                    break;

                case "5":
                    Console.Clear();
                    return;

                default:
                    Console.WriteLine("Invalid choice");
                    break;
            }

            SaveUpdateProfile(); 
        }

        public static void SaveUpdateProfile()
        {
            try
            {
                using (var context = new MarketContext())
                {
                    var currentUser = user_panel2.Helpers.UserManager.User;
                    var updateUser = context.Users.FirstOrDefault(u => u.Id == currentUser.Id);

                    if (updateUser != null)
                    {
                        updateUser.Name = currentUser.Name;
                        updateUser.Surname = currentUser.Surname;
                        updateUser.Email = currentUser.Email;
                        updateUser.DateOfBirth = currentUser.DateOfBirth;

                        if (!string.IsNullOrEmpty(currentUser.Password))
                        {
                            updateUser.Password = currentUser.Password; 
                        }

                        context.Users.Update(updateUser); 
                        context.SaveChanges(); 
                        Console.WriteLine("Profile updated");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public static void ChangePassword()
        {
            Console.Write("Current password: ");
            var currentPassword = Console.ReadLine();

            if (currentPassword != user_panel2.Helpers.UserManager.User.Password)
            {
                Console.WriteLine("Current password is wrong");
                return;
            }

            Console.Write("New password: ");
            var newPassword = Console.ReadLine();

            if (currentPassword == newPassword)
            {
                Console.WriteLine("New password is same with current password");
                Thread.Sleep(2000);
                Console.Clear();
                return;
            }

            Console.Write("Confirm new password: ");
            var confirmPassword = Console.ReadLine();

            if (newPassword != confirmPassword)
            {
                Console.WriteLine("Passwords do not match");
                return;
            }

            user_panel2.Helpers.UserManager.User.Password = newPassword;
            SaveUpdateProfile();
            Console.WriteLine("Password changed");
            Thread.Sleep(2000);
            Console.Clear();
        }

        public static void ShowProfile()
        {
            Console.WriteLine($"Name: {user_panel2.Helpers.UserManager.User.Name}");
            Console.WriteLine($"Surname: {user_panel2.Helpers.UserManager.User.Surname}");
            Console.WriteLine($"Date of Birth: {user_panel2.Helpers.UserManager.User.DateOfBirth:dd.MM.yyyy}");
            Console.WriteLine($"Email: {user_panel2.Helpers.UserManager.User.Email}");
            Console.WriteLine("1. Update profile");
            Console.WriteLine("2. Change password");
            Console.WriteLine("3. Return");
            Console.Write("Make choice: ");
            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    Console.Clear();
                    UpdateProfile();
                    break;
                case "2":
                    Console.Clear();
                    ChangePassword();
                    break;
                case "3":
                    Console.Clear();
                    return;
                default:
                    Console.WriteLine("Invalid choice");
                    break;
            }
        }
        public static void ShowBasket()
        {
            if (Basket.Count == 0)
            {
                Console.WriteLine("Basket is empty");
                return;
            }

            double totalAmount = 0;
            Console.WriteLine("Your basket:");
            for (int i = 0; i < Basket.Count; i++)
            {
                var product = Basket[i];
                Console.WriteLine($" {i + 1}. {product.Name} Price: {product.Price} Quantity: {product.Quantity} Desc: {product.Description}");
                totalAmount += product.Price * product.Quantity;
            }
            Console.WriteLine($"Total Amount: {totalAmount} azn");

            Console.WriteLine("1. Make payment");
            Console.WriteLine("2. Go to menu");
            Console.WriteLine("3. Remove From Basket");
            Console.Write("Make choice: ");
            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    Console.Clear();
                    CalculateTotal(totalAmount);
                    Thread.Sleep(2000);
                    Console.Clear();
                    break;
                case "2":
                    Console.Clear();
                    break;
                case "3":
                    RemoveFromBasket();
                    break;
                default:
                    Console.WriteLine("Invalid choice");
                    break;
            }
        }

     
        public static void CalculateTotal(double totalAmount)
        {
            Console.WriteLine($"Total amount: {totalAmount} azn");
            while (true)
            {
                Console.WriteLine("Enter payment:");
                var pay = Console.ReadLine();
                if (double.TryParse(pay, out double payment))
                {
                    if (payment >= totalAmount)
                    {
                        Console.WriteLine(payment > totalAmount ? $"Take your change: {payment - totalAmount} azn" : "Thanks for the payment");
                        UpdateStockQuantities();

                        Basket.Clear();
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Payment not enough");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid payment amount");
                }
            }
        }
        public static void UpdateStockQuantities()
        {
            try
            {
                using (var context = new MarketContext())
                {
                    foreach (var item in Basket)
                    {
                        var product = context.Products.FirstOrDefault(p => p.Id == item.Id);
                        if (product != null)
                        {
                            product.Quantity -= item.Quantity;
                            context.Products.Update(product);
                        }
                    }
                    context.SaveChanges();
                }
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public static void RemoveFromBasket()
        {
            if (Basket.Count == 0)
            {
                Console.WriteLine("Basket is empty");
                return;
            }

            Console.WriteLine("Select item to remove:");
            for (int i = 0; i < Basket.Count; i++)
            {
                var product = Basket[i];
                Console.WriteLine($" {i + 1}. {product.Name} Price: {product.Price} Quantity: {product.Quantity} Desc: {product.Description}");
            }

            Console.Write("Enter row number of product to remove: ");
            if (int.TryParse(Console.ReadLine(), out int prod) && prod > 0 && prod <= Basket.Count)
            {
                var removeProd = Basket[prod - 1];
                Basket.RemoveAt(prod - 1);
                Console.WriteLine($"{removeProd.Name} removed from basket");

            }
            else
            {
                Console.WriteLine("Invalid input");
            }
            Thread.Sleep(2000);
            Console.Clear();
        }

        public static void ShowCategories()
        {
            try
            {
                using (var context = new MarketContext())
                {
                    var categories = context.Categories.ToList();

                    if (categories.Count > 0)
                    {
                        Console.WriteLine("Select a category:");

                        for (int i = 0; i < categories.Count; i++)
                        {
                            Console.WriteLine($"{i + 1}. {categories[i].Name}");
                        }

                        Console.Write("Enter row number of category: ");
                        if (int.TryParse(Console.ReadLine(), out int selectedCategoryNum) && selectedCategoryNum > 0 && selectedCategoryNum <= categories.Count)
                        {
                            var selectedCategory = categories[selectedCategoryNum - 1];
                            ShowProducts(selectedCategory);
                        }
                        else
                        {
                            Console.WriteLine("Invalid input");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public static void ShowProducts(Category selectedCategory)
        {
            try
            {
                using (var context = new MarketContext())
                {
                    var products = context.Products.Where(p => p.CategoryId == selectedCategory.Id && p.Quantity > 0).ToList();

                    if (products.Count > 0)
                    {
                        Console.WriteLine(selectedCategory.Name);

                        for (int i = 0; i < products.Count; i++)
                        {
                            Console.WriteLine($"{i + 1}. {products[i].Name} Price: {products[i].Price} Quantity: {products[i].Quantity}");
                        }

                        Console.Write("Enter row number of product to add to basket else 0 go back: ");
                        if (int.TryParse(Console.ReadLine(), out int selectedProductNum) && selectedProductNum > 0 && selectedProductNum <= products.Count)
                        {
                            var selectedProduct = products[selectedProductNum - 1];
                            Console.Write("Enter quantity: ");
                            if (int.TryParse(Console.ReadLine(), out int quantity) && quantity > 0 && quantity <= selectedProduct.Quantity)
                            {
                                var productToAdd = new Product
                                {
                                    Id = selectedProduct.Id,
                                    Name = selectedProduct.Name,
                                    Price = selectedProduct.Price,
                                    Quantity = quantity,
                                    Description = selectedProduct.Description
                                };
                                AddToBasket(productToAdd);
                            }
                            else
                            {
                                Console.WriteLine("Invalid quantity");
                            }
                        }
                        else if (selectedProductNum == 0)
                        {
                            Console.Clear();
                            ShowCategories();
                        }
                        else
                        {
                            Console.WriteLine("Invalid quantity");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public static void AddToBasket(Product product)
        {
            var existProduct = Basket.FirstOrDefault(name => name.Name.ToLower() == product.Name.ToLower());
            if (existProduct != null)
            {
                existProduct.Quantity += product.Quantity;
            }
            else
            {
                Basket.Add(product);
            }

            Console.WriteLine($"{product.Name} added to basket");
            Thread.Sleep(1000);
            Console.Clear();
        }

        static void Main(string[] args)
        {
            bool loggedin = false;
            while (true)
            {
                if (!loggedin)
                {
                    MainPage();
                    Console.Write("Make choice: ");
                    var choice = Console.ReadLine();

                    switch (choice)
                    {
                        case "1":
                            Console.Clear();
                            CreateAccountPage();
                            loggedin = LoginPage();
                            break;
                        case "2":
                            Console.Clear();
                            loggedin = LoginPage();
                            break;
                        default:
                            Console.WriteLine("Invalid choice");
                            break;
                    }
                }
                else
                {
                    LoginMenu();
                    loggedin = false;
                }
            }
        }
    }
}