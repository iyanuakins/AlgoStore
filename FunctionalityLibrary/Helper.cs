using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SQLite;
using System.Text.RegularExpressions;

namespace FunctionalityLibrary
{
    public class Helper
    {
        public static SQLiteConnection ConnectToDb()
        {
            //string connectionString = ConfigurationManager.ConnectionStrings["SqliteConnectionString"].ConnectionString;
            string connectionString = "Data Source=Dbase.db;Version=3";
            return new SQLiteConnection(connectionString);
        }

        public static Product GetProduct(int productId)
        {
            string query = "SELECT * FROM products where productid = @id";
            using (SQLiteConnection connection = Helper.ConnectToDb())
            using (SQLiteCommand command = new SQLiteCommand(query, connection))
            using (SQLiteDataAdapter adapter = new SQLiteDataAdapter())
            {
                try
                {

                    command.Parameters.AddWithValue("@id", productId);
                    connection.Open();
                    adapter.SelectCommand = command;
                    SQLiteDataReader reader = adapter.SelectCommand.ExecuteReader();
                    if (reader.Read())
                    {
                        return new Product(
                            productId,
                            reader.GetString(1),
                            reader.GetString(2),
                            Convert.ToDouble(reader.GetFloat(3)),
                            reader.GetInt32(4),
                            Convert.ToDateTime(reader.GetString(5))
                        );
                    }
                    else
                    {
                        return null;
                    }

                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    return null;
                }
            }
        }

        public static void InitializeDatabase()
        {
            using (SQLiteConnection connection = Helper.ConnectToDb())
            {
                string query = "CREATE TABLE IF NOT EXISTS users(userid INTEGER PRIMARY KEY AUTOINCREMENT, name TEXT NOT NULL, email TEXT NOT NULL UNIQUE, type TEXT NOT NULL, password TEXT NOT NULL, dateregistered TEXT NOT NULL, totalorders INTEGER NOT NULL, address TEXT NOT NULL)";
                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    try
                    {
                        connection.Open();
                        command.ExecuteNonQuery();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
            }
            
            using (SQLiteConnection connection = Helper.ConnectToDb())
            {
                string query = "CREATE TABLE IF NOT EXISTS products(productid INTEGER PRIMARY KEY AUTOINCREMENT, productname TEXT NOT NULL, productdescription TEXT NOT NULL, price REAL NOT NULL, units INTEGER NOT NULL, dateadded TEXT NOT NULL)";
                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    try
                    {
                        connection.Open();
                        command.ExecuteNonQuery();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
            }
            
            using (SQLiteConnection connection = Helper.ConnectToDb())
            {
                string query = "CREATE TABLE IF NOT EXISTS orders (orderid INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, totalproduct INTEGER NOT NULL, user INTEGER NOT NULL, productsinorder TEXT NOT NULL, totalprice REAL NOT NULL, orderdate TEXT NOT NULL";
                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    try
                    {
                        connection.Open();
                        command.ExecuteNonQuery();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
            }
        }

        public static bool CheckEmail(string email)
        {
            try
            {
                string query = "SELECT email FROM users WHERE email = @email";
                using (SQLiteConnection connection = Helper.ConnectToDb())
                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                using (SQLiteDataAdapter adapter = new SQLiteDataAdapter())
                {
                    command.Parameters.AddWithValue("@email", email);
                    connection.Open();
                    adapter.SelectCommand = command;
                    SQLiteDataReader reader = adapter.SelectCommand.ExecuteReader();
                    //SQLiteDataReader reader = command.ExecuteReader();
                    return reader.Read();
                };

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public static int Register(string name, string email, string password, string address)
        {
            string query = "INSERT INTO users VALUES(@id, @name, @email, @type, @password, @date, @totalOrder, @address)";
            using (SQLiteConnection connection = Helper.ConnectToDb())
            using (SQLiteCommand command = new SQLiteCommand(query, connection))
            using (SQLiteDataAdapter adapter = new SQLiteDataAdapter())
            {
                command.Parameters.AddWithValue("@name", name);
                command.Parameters.AddWithValue("@id", null);
                command.Parameters.AddWithValue("@email", email);
                command.Parameters.AddWithValue("@type", "customer");
                command.Parameters.AddWithValue("@password", password);
                command.Parameters.AddWithValue("@date", DateTime.Now.ToString());
                command.Parameters.AddWithValue("@totalOrder", 0);
                command.Parameters.AddWithValue("@address", address);
                try
                {
                    connection.Open();
                    adapter.InsertCommand = command;
                    int row = adapter.InsertCommand.ExecuteNonQuery();
                    Console.WriteLine(row > 0
                        ? "Registration successful, proceed to login"
                        : "Registration failed, please retry");
                    return row;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    return 0;
                }
            }
        }

        public static User Login(string email, string password)
        {
            try
            {
                string query = "SELECT * FROM users WHERE email = @email";
                using (SQLiteConnection connection = Helper.ConnectToDb())
                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                using (SQLiteDataAdapter adapter = new SQLiteDataAdapter())
                {
                    command.Parameters.AddWithValue("@email", email);
                    connection.Open();
                    adapter.SelectCommand = command;
                    SQLiteDataReader reader = adapter.SelectCommand.ExecuteReader();

                    if (reader.Read())
                    {
                        if (reader.GetString(4).Equals(password))
                        {
                            if (reader.GetString(3).Equals("customer"))
                            {
                                return new Customer(
                                    reader.GetInt32(0),
                                    reader.GetString(1),
                                    reader.GetString(2),
                                    reader.GetInt32(6),
                                    Convert.ToDateTime(reader.GetString(5)),
                                    reader.GetString(7)
                                );
                            }
                            else
                            {
                                return new Admin(
                                    reader.GetInt32(0),
                                    reader.GetString(1),
                                    reader.GetString(2),
                                    Convert.ToDateTime(reader.GetString(5))
                                );
                            }
                        }
                        else
                        {
                            Console.WriteLine("Invalid email or password");
                            return null;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid email or password");
                        return null;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        public static List<Product> ViewAllProducts()
        {
            using (SQLiteConnection connection = Helper.ConnectToDb())
            {
                try
                {
                    string query = "SELECT * FROM products";
                    SQLiteCommand command = new SQLiteCommand(query, connection);
                    connection.Open();
                    SQLiteDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        List<Product> allProducts = new List<Product>
                        {
                            new Product(
                                reader.GetInt32(0),
                                reader.GetString(1),
                                reader.GetString(2),
                                Convert.ToDouble(reader.GetFloat(3)),
                                reader.GetInt32(4),
                                Convert.ToDateTime(reader.GetString(5))
                            )
                        };
                        while (reader.Read())
                        {
                            allProducts.Add(new Product(
                                    reader.GetInt32(0),
                                    reader.GetString(1),
                                    reader.GetString(2),
                                    Convert.ToDouble(reader.GetFloat(3)),
                                    reader.GetInt32(4),
                                    Convert.ToDateTime(reader.GetString(5))
                                )
                            );
                        }

                        return allProducts;
                    }
                    else
                    {
                        return null;
                    }

                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    return null;
                }
                finally { connection.Close(); }
            }
        }

        public static string[] GetUserInputForRegistration()
        {
            bool isValid;
            string[] inputs = new string[4];
            do
            {
                string name;
                string email;
                string address;
                string password;
                bool isValidName;

                do
                {
                    Console.WriteLine("================================================================");
                    Console.WriteLine("                     Fill Registration Form                     ");
                    Console.WriteLine("================================================================");

                    Console.Write("Enter First name and Last name: ");
                    name = Console.ReadLine();
                    isValidName = Regex.IsMatch(name, @"^[A-Za-z\s.\'\-]+$", RegexOptions.IgnoreCase);
                    if (!isValidName)
                    {
                        Console.WriteLine("\nPlease enter a valid name\n");
                    }

                } while (!isValidName);

                inputs[0] = name;

                bool isValidAddress;
                do
                {
                    Console.Write("Enter your address: ");
                    address = Console.ReadLine();
                    isValidAddress = !string.IsNullOrEmpty(address);
                    if (!isValidAddress)
                    {
                        Console.WriteLine("\nPlease enter a valid address\n");
                    }

                } while (!isValidAddress);

                inputs[3] = address;
                bool isValidEmail;
                do
                {
                    Console.Write("Enter email address: ");
                    email = Console.ReadLine();
                    isValidEmail = Regex.IsMatch(email, @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                                                        @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-0-9a-z]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$",
                                                        RegexOptions.IgnoreCase);
                    if (!isValidEmail)
                    {
                        Console.WriteLine("\nPlease enter a valid email address\n");
                    }
                    else if (CheckEmail(email))
                    {
                        isValidEmail = false;
                        Console.WriteLine("\nEmail already in use \n" +
                                          "Try another email or try to login\n");
                        int option = 0;
                        do
                        {
                            Console.WriteLine("\nEnter 1 to try another email\n" +
                                              "Enter 2 to exit registration process");
                            string userInput = Console.ReadLine();
                            bool isValidInput = int.TryParse(userInput, out int selectedOption);

                            if (isValidInput && (selectedOption == 1 || selectedOption == 2))
                            {
                                option = selectedOption;
                            }
                            else
                            {
                                Console.WriteLine("\nInvalid option, Please select valid option\n");
                            }
                        } while (option == 0);

                        if (option == 2)
                        {
                            return null;
                        }
                    }
                } while (!isValidEmail);

                inputs[1] = email;
                bool isValidPassword = false;
                do
                {
                    Console.WriteLine("----------------------------------------------------------------");
                    Console.WriteLine("Note:\n" +
                                      "1. Password should be minimum of 6 and maximum of 32 characters\n" +
                                      "2. Password Can contain alphabets, number and symbols");
                    Console.WriteLine("----------------------------------------------------------------");
                    Console.Write("Enter your desired password: ");

                    password = GetPassword();

                    bool passwordValid = 6 <= password.Length && password.Length <= 36;
                    if (passwordValid)
                    {
                        Console.Write("Confirm Password: ");
                        string confirmPassword = GetPassword();

                        isValidPassword = password == confirmPassword;
                        if (!isValidPassword)
                        {
                            Console.WriteLine("\nPassword does not match, try again\n");
                        }
                    }
                    else
                    {
                        Console.WriteLine("\nPlease enter a valid password\n");

                    }
                } while (!isValidPassword);

                inputs[2] = password;
                isValid = isValidEmail && isValidName && isValidPassword;

            } while (!isValid);

            return inputs;
        }

        public static bool VerifyPassword(int id)
        {
            bool passwordValid;
            string password;
            do
            {
                Console.Write("Enter your current password: ");
                password = GetPassword();

                passwordValid = 6 <= password.Length && password.Length <= 36;
                if (!passwordValid)
                {
                    Console.WriteLine("\nPlease enter a valid password\n");

                }
            } while (!passwordValid);

            try
            {
                string query = $"SELECT password FROM users WHERE userid = {id}";
                using (SQLiteConnection connection = Helper.ConnectToDb())
                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                using (SQLiteDataAdapter adapter = new SQLiteDataAdapter())
                {
                    connection.Open();
                    adapter.SelectCommand = command;
                    SQLiteDataReader reader = adapter.SelectCommand.ExecuteReader();
                    if (reader.Read())
                    {
                        Console.WriteLine($"old pass: {reader.GetString(0)}");
                         return reader.GetString(0).Equals(password);
                    }

                    return false;
                };

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }

        }

        public static string[] GetUserInputForLogin()
        {

            string[] inputs = new string[2];
            string email;
            string password;
            bool isValidEmail;
            do
            {
                Console.WriteLine("================================================================");
                Console.WriteLine("                     Fill Registration Form                     ");
                Console.WriteLine("================================================================");

                Console.Write("Enter email address: ");
                email = Console.ReadLine();
                isValidEmail = Regex.IsMatch(email,
                    @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                    @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-0-9a-z]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$",
                    RegexOptions.IgnoreCase);
                if (!isValidEmail)
                {
                    Console.WriteLine("\nPlease enter a valid email address\n");
                }
            } while (!isValidEmail);
            
            Console.Write("Enter your password: ");
            password = GetPassword();

            inputs[0] = email;
            inputs[1] = password;
            return inputs;
        }

        public static string GetPassword()
        {
            string password = "";
            do
            {
                ConsoleKeyInfo key = Console.ReadKey(true);
                // Backspace Should Not Work
                if (key.Key != ConsoleKey.Backspace && key.Key != ConsoleKey.Enter)
                {
                    password += key.KeyChar;
                    Random Rand = new Random();
                    int number = Rand.Next(1, 3);
                    string Asterisks = "".PadLeft(number, '*');
                    Console.Write($"{Asterisks}");
                }
                else
                {
                    if (key.Key == ConsoleKey.Backspace && password.Length > 0)
                    {
                        password = password.Substring(0, (password.Length - 1));
                        Console.Write("\b \b");
                    }
                    else if (key.Key == ConsoleKey.Enter)
                    {
                        Console.WriteLine();
                        return password;
                    }
                }
            } while (true);
        }
    }
}
