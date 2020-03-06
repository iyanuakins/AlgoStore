using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Text;
using FunctionalityLibrary;

namespace ConsoleUI
{
    class Program
    {
        static void Main(string[] args)
        {
            Helper.InitializeDatabase();
            Console.WriteLine("Welcome to Algo Store\n");
            bool isAppOn = false;

            do
            {
                int firstSelectedOption;
                bool isValid;
                do
                {
                    Console.WriteLine("1)  =>  Register\n" +
                                      "2)  =>  Login\n" +
                                      "3)  =>  Exit store");
                    Console.Write("Select option: ");
                    string userInput = Console.ReadLine();
                    isValid = int.TryParse(userInput, out firstSelectedOption);
                    if (!(isValid && (1 <= firstSelectedOption && firstSelectedOption <= 3)))
                    {
                        isValid = false;
                        Console.WriteLine("Invalid option, Please select valid option\n");
                    }
                } while (!isValid);

                if (firstSelectedOption == 1)
                {
                    string[] inputs = Helper.GetUserInputForRegistration();
                    if (inputs != null)
                    {
                        Helper.Register(inputs[0], inputs[1], inputs[2], inputs[3]);
                    }
                    else
                    {
                        Console.WriteLine("Something went wrong");
                    }
                }
                else if (firstSelectedOption == 2)
                {
                    string[] inputs = Helper.GetUserInputForLogin();
                    User authenticatedUser = Helper.Login(inputs[0], inputs[1]);
                    if (authenticatedUser != null)
                    {
                        if (authenticatedUser.GetType() == typeof(Customer))
                        {
                            bool isCustomerSessionOn = true;
                            do
                            {
                                Customer loggedInCustomer = (Customer)authenticatedUser;
                                bool isValidOption;
                                int secondSelectedOption;
                                do
                                {
                                    Console.WriteLine("========================================");
                                    Console.WriteLine("           Available Operations         ");
                                    Console.WriteLine("=========================================");
                                    Console.WriteLine("1) => Shop\n" +
                                                      "2) => View profile\n" +
                                                      "3) => Edit profile\n" +
                                                      "4) => View Cart\n" +
                                                      "5) => View shopping history\n" +
                                                      "6) => Log out\n");
                                    Console.WriteLine();
                                    Console.Write("Select operation: ");
                                    string userInputForOperation = Console.ReadLine();
                                    isValidOption = int.TryParse(userInputForOperation, out secondSelectedOption);
                                    if (!(isValidOption && (1 <= secondSelectedOption && secondSelectedOption <= 6)))
                                    {
                                        isValidOption = false;
                                        Console.WriteLine("Invalid option, Please select valid option\n");
                                    }
                                } while (!isValidOption);

                                switch (secondSelectedOption)
                                {
                                    case 1:
                                        List<Product> allProducts = Helper.ViewAllProducts();
                                        StringBuilder productStringBuilder = new StringBuilder();
                                        productStringBuilder.AppendLine(
                                            " S/N |   Name    |   Price  | Units Available");
                                        int i = 0;
                                        foreach (Product product in allProducts)
                                        {
                                            productStringBuilder.AppendLine(
                                                $"  {++i}  |   {product.Name}   |  NGN{product.Price}   |       {product.Unit}");
                                        }

                                        Console.WriteLine(productStringBuilder.ToString());
                                        Console.WriteLine("\n\n");
                                        Console.WriteLine("Enter product S/N to add to cart\n" +
                                                          $"Enter {allProducts.Count + 1} to view cart\n" +
                                                          $"Enter {allProducts.Count + 2} to checkout\n" +
                                                          $"Enter {allProducts.Count + 3} to go back to main menu\n");
                                        bool customerDoneShopping = false;
                                        do
                                        {
                                            bool isValidOption2;
                                            int promptSelectedOption;
                                            do
                                            {
                                                string userInputForOption = Console.ReadLine();
                                                isValidOption2 = int.TryParse(userInputForOption,
                                                    out promptSelectedOption);
                                                if (!(isValidOption2 &&
                                                      (1 <= promptSelectedOption &&
                                                       promptSelectedOption <= allProducts.Count + 3)))
                                                {
                                                    isValidOption2 = false;
                                                    Console.WriteLine("Invalid option, Please select valid option\n");
                                                }

                                            } while (!isValidOption2);

                                            if (1 <= promptSelectedOption && promptSelectedOption <= allProducts.Count)
                                            {
                                                    loggedInCustomer.AddToCart(allProducts[promptSelectedOption - 1]);
                                            }
                                            else if (promptSelectedOption == allProducts.Count + 1)
                                            {
                                                goto case 4;
                                            }
                                            else if (promptSelectedOption == allProducts.Count + 2)
                                            {
                                                goto case 4;
                                            }
                                            else
                                            {
                                                customerDoneShopping = true;
                                            }
                                        } while (!customerDoneShopping);

                                        break;
                                    case 2:
                                        Console.WriteLine(
                                            "\n=========================================================\n" +
                                            $"Name: {loggedInCustomer.Name}\n" +
                                            $"Email: {loggedInCustomer.Email}\n" +
                                            $"Home address: {loggedInCustomer.Address}\n" +
                                            $"Total number of order: {loggedInCustomer.TotalOrders}\n" +
                                            $"Date registered: {loggedInCustomer.DateRegistered}\n" +
                                            "=========================================================\n\n");
                                        break;
                                    case 3:
                                        bool isValidManagementOption;
                                        int promptOption;
                                        do
                                        {
                                            Console.WriteLine("\n1)  =>  Change your name\n" +
                                                              "2)  =>  Change your home address\n" +
                                                              "3)  =>  Change password");
                                            string userInputForOption = Console.ReadLine();
                                            isValidManagementOption = int.TryParse(userInputForOption, out promptOption);
                                            if (!(isValidManagementOption && (1 <= promptOption && promptOption <= 3)))
                                            {
                                                isValidManagementOption = false;
                                                Console.WriteLine("Invalid option, Please select valid option\n");
                                            }
                                        } while (!isValidManagementOption);

                                        if (promptOption == 1)
                                        {
                                            string newName;
                                            bool isValidNewName;
                                            do
                                            {
                                                Console.Write("Enter new name: ");
                                                newName = Console.ReadLine();
                                                isValidNewName = !string.IsNullOrEmpty(newName);
                                                if (!isValidNewName)
                                                {
                                                    Console.WriteLine("\nPlease enter a valid name\n");
                                                }

                                            } while (!isValidNewName);

                                            loggedInCustomer.EditProfile(newName);
                                        }
                                        else if (promptOption == 2)
                                        {
                                            string newAddress;
                                            bool isValidNewAddress;
                                            do
                                            {
                                                Console.Write("Enter new home address: ");
                                                newAddress = Console.ReadLine();
                                                isValidNewAddress = !string.IsNullOrEmpty(newAddress);
                                                if (!isValidNewAddress)
                                                {
                                                    Console.WriteLine("\nPlease enter a valid home address\n");
                                                }

                                            } while (!isValidNewAddress);

                                            loggedInCustomer.EditProfile(address: newAddress);
                                        }
                                        else
                                        {
                                            bool isValidPassword = false;
                                            string password;
                                            do
                                            {
                                                Console.WriteLine("----------------------------------------------------------------");
                                                Console.WriteLine("Note:\n" +
                                                                  "1. Password should be minimum of 6 and maximum of 32 characters\n" +
                                                                  "2. Password Can contain alphabets, number and symbols");
                                                Console.WriteLine("----------------------------------------------------------------");
                                                Console.Write("Enter your new password: ");
                                                
                                                password = Helper.GetPassword();
                                                
                                                bool passwordValid = 6 <= password.Length && password.Length <= 36;
                                                if (passwordValid)
                                                {
                                                    Console.Write("Confirm Password: ");
                                                    string confirmPassword = Helper.GetPassword();

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

                                            if (Helper.VerifyPassword(loggedInCustomer.Id))
                                            {
                                                loggedInCustomer.EditProfile(password: password);
                                            }
                                            else
                                            {
                                                Console.WriteLine("\n Failed: Incorrect current password entered\n");
                                            }
                                        }
                                        break;
                                    case 4:
                                        if (loggedInCustomer.Cart.ProductsInCart.Count > 0)
                                        {
                                            bool isCartOps = true;
                                            do
                                            {
                                                List<Product> allProductsInCart = loggedInCustomer.Cart.ProductsInCart;
                                                StringBuilder productsStringBuilder = new StringBuilder();
                                                productsStringBuilder.AppendLine(" S/N |   Name    |   Price  |");
                                                int j = 0;
                                                foreach (Product product in allProductsInCart)
                                                {
                                                    productsStringBuilder.AppendLine(
                                                        $"  {++j}  |   {product.Name}   |  NGN{product.Price}   |");
                                                }

                                                productsStringBuilder.AppendLine(
                                                    $"Total product price: NGN{loggedInCustomer.Cart.TotalPrice}");
                                                Console.WriteLine(productsStringBuilder.ToString());


                                                Console.WriteLine("Enter product S/N to remove from cart\n" +
                                                                  $"Enter {allProductsInCart.Count + 1} to checkout\n" +
                                                                  $"Enter {allProductsInCart.Count + 2} to shop\n");

                                                bool isCartValidOption;
                                                do
                                                {
                                                    Console.Write("Select operation: ");
                                                    string userInputForOperation = Console.ReadLine();
                                                    isCartValidOption = int.TryParse(userInputForOperation,
                                                        out secondSelectedOption);
                                                    if (!(isCartValidOption &&
                                                          (1 <= secondSelectedOption && secondSelectedOption <=
                                                           allProductsInCart.Count + 2)))
                                                    {
                                                        isCartValidOption = false;
                                                        Console.WriteLine(
                                                            "Invalid option, Please select valid option\n");
                                                    }
                                                } while (!isCartValidOption);

                                                if (1 <= secondSelectedOption &&
                                                    secondSelectedOption <= allProductsInCart.Count)
                                                {
                                                    loggedInCustomer.RemoveFromCart(secondSelectedOption - 1);
                                                    Console.WriteLine("Product has been removed from cart");
                                                }
                                                else if (secondSelectedOption == allProductsInCart.Count + 1)
                                                {
                                                    loggedInCustomer.Checkout();
                                                    isCartOps = false;
                                                }
                                                else
                                                {
                                                    goto case 1;
                                                }
                                            } while (isCartOps);
                                        }
                                        else
                                        {
                                            Console.WriteLine(
                                                "\nYour cart is empty please shop to add product to cart\n");
                                        }

                                        Console.WriteLine("\n\n");
                                        break;
                                    case 5:
                                        //View shopping history
                                        List<Order> orderHistory = loggedInCustomer.ViewOrdersHistory();
                                        if (orderHistory != null)
                                        {
                                            StringBuilder historyBuilder = new StringBuilder();
                                            i = 0;
                                            foreach (Order order in orderHistory)
                                            {
                                                StringBuilder productBuilder = new StringBuilder();
                                                if (order.ProductsInOrder.Count > 1)
                                                {
                                                    int j = 0;
                                                    foreach (Product product in order.ProductsInOrder)
                                                    {
                                                        string productString =
                                                            $"\n{++j}) => Product name: {product.Name} | Product price: NGN{product.Price}";
                                                        productBuilder.AppendLine(productString);
                                                    }
                                                }
                                                else
                                                {
                                                    string productString =
                                                        $"1) => Product name: {order.ProductsInOrder[0].Name} | Product price: NGN{order.ProductsInOrder[0].Price}";
                                                    productBuilder.AppendLine(productString);
                                                }

                                                string history =
                                                    $"==================================================================\n" +
                                                    $"Total products in order: {order.TotalProductInOrder}\n" +
                                                    $"Total cost: {order.TotalPrice}" +
                                                    $"{productBuilder.ToString()}\n" +
                                                    $"==================================================================\n";
                                                historyBuilder.AppendLine(history);
                                            }

                                            Console.WriteLine(historyBuilder.ToString());
                                        }
                                        else
                                        {
                                            Console.WriteLine("\nNo order history available\n");
                                        }
                                        
                                        break;
                                    default:
                                        Console.WriteLine(
                                            $"\n\n{loggedInCustomer.Name}, Thanks for shopping with us\n\n");
                                        isCustomerSessionOn = false;
                                        break;
                                }
                            } while (isCustomerSessionOn);
                        }
                        else
                        {
                            bool isAdminSessionOn = true;
                            do
                            {
                                Admin loggedInAdmin = (Admin)authenticatedUser;
                                bool isValidOption;
                                int secondSelectedOption;
                                Console.WriteLine("========================================");
                                Console.WriteLine("           Available Operations         ");
                                Console.WriteLine("=========================================");
                                Console.WriteLine("1) => View customers\n" +
                                                  "2) => View all orders\n" +
                                                  "3) => Add products\n" +
                                                  "4) => Update products\n" +
                                                  "5) => Log out\n");
                                Console.WriteLine();
                                do
                                {
                                    Console.Write("Select operation: ");
                                    string userInputForOperation = Console.ReadLine();
                                    isValidOption = int.TryParse(userInputForOperation, out secondSelectedOption);

                                    if (!(isValidOption && (1 <= secondSelectedOption && secondSelectedOption <= 5)))
                                    {
                                        isValidOption = false;
                                        Console.WriteLine("Invalid option, Please select valid option\n");
                                    }
                                } while (!isValidOption);

                                switch (secondSelectedOption)
                                {
                                    case 1:
                                        List<Customer> allCustomers = loggedInAdmin.ViewAllCustomers();
                                        if (allCustomers == null)
                                        {
                                            Console.WriteLine("No customer to display");
                                        }
                                        else
                                        {
                                            StringBuilder allCustomerStringBuilder = new StringBuilder();
                                            allCustomerStringBuilder.AppendLine(
                                                "=========================================================");
                                            int y = 0;
                                            foreach (Customer customer in allCustomers)
                                            {
                                                string customerString = $"\n{++y}\n" +
                                                                        $"ID: {customer.Id}\n" +
                                                                        $"Name: {customer.Name}\n" +
                                                                        $"Email: {customer.Email}\n" +
                                                                        $"Home address: {customer.Address}\n" +
                                                                        $"Total number of order: {customer.TotalOrders}\n" +
                                                                        $"Date registered: {customer.DateRegistered}\n" +
                                                                        "=========================================================\n";

                                                allCustomerStringBuilder.AppendLine(customerString);
                                            }

                                            Console.WriteLine(allCustomerStringBuilder.ToString());
                                        }
                                        break;
                                    case 2:
                                        List<Order> allOrders = loggedInAdmin.ViewAllOrders();
                                        StringBuilder orderStringBuilder = new StringBuilder();
                                        int i = 0;
                                        foreach (Order order in allOrders)
                                        {
                                            StringBuilder productBuilder = new StringBuilder();
                                            if (order.ProductsInOrder.Count > 1)
                                            {
                                                int j = 0;
                                                foreach (Product product in order.ProductsInOrder)
                                                {
                                                    string productString =
                                                        $"{++j}) => Product name: {product.Name} | Product price: NGN{product.Price}";
                                                    productBuilder.AppendLine(productString);
                                                }
                                            }
                                            else
                                            {
                                                string productString =
                                                    $"1) => Product name: {order.ProductsInOrder[0].Name} | Product price: NGN{order.ProductsInOrder[0].Price}";
                                                productBuilder.AppendLine(productString);
                                            }

                                            string history =
                                                $"\n==================================================================\n" +
                                                $"Total products in order: {order.TotalProductInOrder}\n" +
                                                $"Total cost: {order.TotalPrice}\n" +
                                                $"{productBuilder.ToString()}\n" +
                                                $"==================================================================\n";
                                            orderStringBuilder.AppendLine(history);
                                        }

                                        Console.WriteLine(orderStringBuilder.ToString());
                                        break;
                                    case 3:
                                        string name;
                                        string description;
                                        double price;
                                        int unit;
                                        bool isValidProductName;
                                        bool isValidProductDesc;
                                        bool isValidProductPrice;
                                        bool isValidProductUnit;
                                        do
                                        {
                                            Console.Write("Enter product name: ");
                                            name = Console.ReadLine();
                                            isValidProductName = !string.IsNullOrEmpty(name);
                                            if (!isValidProductName)
                                            {
                                                Console.WriteLine("\nPlease enter a valid product name\n");
                                            }

                                        } while (!isValidProductName);

                                        do
                                        {
                                            Console.Write("Enter product description: ");
                                            description = Console.ReadLine();
                                            isValidProductDesc = !string.IsNullOrEmpty(description);
                                            if (!isValidProductDesc)
                                            {
                                                Console.WriteLine("\nPlease enter a valid product description\n");
                                            }

                                        } while (!isValidProductDesc);

                                        do
                                        {
                                            Console.Write("Enter product price: ");
                                            string input = Console.ReadLine();
                                            isValidProductPrice = double.TryParse(input, out price);
                                            if (!isValidProductPrice)
                                            {
                                                Console.WriteLine("\nPlease enter a valid product price\n");
                                            }

                                        } while (!isValidProductPrice);

                                        do
                                        {
                                            Console.Write("Enter product units: ");
                                            string input = Console.ReadLine();
                                            isValidProductUnit = int.TryParse(input, out unit);
                                            if (!isValidProductUnit)
                                            {
                                                Console.WriteLine("\nPlease enter a valid Product unit\n");
                                            }

                                        } while (!isValidProductUnit);

                                        bool success = loggedInAdmin.AddProduct(name, description, price, unit);
                                        if (success)
                                        {
                                            Console.WriteLine("\nProduct successfully added\n");
                                        }
                                        else
                                        {
                                            Console.WriteLine("\nSomething went wrong, Could not add product.\n");
                                        }

                                        break;
                                    case 4:
                                        List<Product> allProducts = Helper.ViewAllProducts();
                                        StringBuilder productStringBuilder = new StringBuilder();
                                        productStringBuilder.AppendLine(
                                            " S/N |   Name    |   Price  | Units Available");
                                        i = 0;
                                        foreach (Product product in allProducts)
                                        {
                                            productStringBuilder.AppendLine(
                                                $"  {++i}  |   {product.Name}   |  NGN{product.Price}   |       {product.Unit}");
                                        }

                                        Console.WriteLine(productStringBuilder.ToString());
                                        Console.WriteLine("\n\n");
                                        bool isProductSelection;
                                        secondSelectedOption = 0;
                                        do
                                        {

                                            Console.Write("Enter product S/N to selected product: ");
                                            string userInputForOperation = Console.ReadLine();
                                            isProductSelection = int.TryParse(userInputForOperation,
                                                out secondSelectedOption);
                                            if (!(isProductSelection &&
                                                  (1 <= secondSelectedOption && secondSelectedOption <=
                                                   allProducts.Count) && secondSelectedOption > 0))
                                            {
                                                isProductSelection = false;
                                                Console.WriteLine(
                                                    "\nInvalid option, Please select valid product selection\n");
                                            }
                                        } while (!isProductSelection);

                                        bool isValidManagementOption;
                                        int promptSelectedOption;
                                        do
                                        {
                                            Console.WriteLine("\n1)  =>  Update product name and description\n" +
                                                              "2)  =>  Update price\n" +
                                                              "3)  =>  Add more units\n");
                                            string userInputForOption = Console.ReadLine();
                                            isValidManagementOption = int.TryParse(userInputForOption, out promptSelectedOption);
                                            if (!(isValidManagementOption && (1 <= promptSelectedOption && promptSelectedOption <= 3)))
                                            {
                                                isValidManagementOption = false;
                                                Console.WriteLine("Invalid option, Please select valid option\n");
                                            }
                                        } while (!isValidManagementOption);

                                        if (promptSelectedOption == 1)
                                        {
                                            name = "";
                                            description = "";
                                            
                                            do
                                            {
                                                Console.Write("Enter your address product name: ");
                                                name = Console.ReadLine();
                                                isValidProductName = !string.IsNullOrEmpty(name);
                                                if (!isValidProductName)
                                                {
                                                    Console.WriteLine("\nPlease enter a valid product name\n");
                                                }

                                            } while (!isValidProductName);


                                            do
                                            {
                                                Console.Write("Enter new product description: ");
                                                description = Console.ReadLine();
                                                isValidProductDesc = !string.IsNullOrEmpty(description);
                                                if (!isValidProductDesc)
                                                {
                                                    Console.WriteLine("\nPlease enter a valid product description\n");
                                                }

                                            } while (!isValidProductDesc);
                                                
                                            loggedInAdmin.UpdateProduct(allProducts[secondSelectedOption - 1].Id, name,
                                                description);
                                        }
                                        else if (promptSelectedOption == 2)
                                        {
                                            isValidProductPrice = false;
                                            price = 0.0;
                                            do
                                            {
                                                Console.Write("Enter new product price: ");
                                                string input = Console.ReadLine();
                                                isValidProductPrice = double.TryParse(input, out price);
                                                if (!isValidProductPrice && price > 0.0)
                                                {
                                                    Console.WriteLine("\nPlease enter a valid product price\n");
                                                }

                                            } while (!isValidProductPrice);

                                            loggedInAdmin.UpdateProduct(allProducts[secondSelectedOption - 1].Id,
                                                price: price);
                                        }
                                        else
                                        {
                                            isValidProductUnit = false;
                                            unit = 0;
                                            do
                                            {
                                                Console.Write("Enter units to add: ");
                                                string input = Console.ReadLine();
                                                isValidProductUnit = int.TryParse(input, out unit);
                                                if (!isValidProductUnit && unit > 0)
                                                {
                                                    Console.WriteLine("\nPlease enter a valid Product unit\n");
                                                }

                                            } while (!isValidProductUnit);

                                            loggedInAdmin.UpdateProduct(allProducts[secondSelectedOption - 1].Id,
                                                units: unit);
                                        }
                                        break;
                                    default:
                                        isAdminSessionOn = false;
                                        Console.WriteLine($"{loggedInAdmin.Name} you are now logged out");
                                        break;
                                }

                            } while (isAdminSessionOn);
                        }
                    }
                }
                else
                {
                    isAppOn = true;
                }

            } while (!isAppOn);
        }
    }
}
