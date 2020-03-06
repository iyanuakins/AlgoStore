using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FunctionalityLibrary;
using System.Data.SQLite;


namespace StoreTests
{
    [TestClass]
    public class CustomerTests
    {
        public CustomerTests()
        {
            Helper.InitializeDatabase();
        }

        [TestMethod]
        public void AddToCartTest()
        {
            //Arrange
            Product firstProduct = Helper.GetProduct(1);
            Product secondProduct = Helper.GetProduct(2); 
            string email = "ayo@test.com";
            string password = "111000";
            //Customer customer = (Customer)Helper.Login(email, password);
            Customer customer = new Customer(1, "", email, 0, DateTime.Now, "");
            int actualCartCount = 1;
            int secondActualCartCount = 2;

            //Act
            int cartCount = customer.AddToCart(firstProduct);

            //Assert
            Assert.AreEqual(cartCount, actualCartCount, "Should return 1 as cart count after adding one product");
            
            //Act
            //cartCount = customer.AddToCart(secondProduct);

            //Assert
            //Assert.AreEqual(cartCount, secondActualCartCount, "Should return 2 as cart count after adding another product");
        }
        
        
        [TestMethod]
        public void RemoveFromCartTest()
        {
            //Arrange
            Product firstProduct = Helper.GetProduct(1);
            Product secondProduct = Helper.GetProduct(2);
            string email = "ayo@test.com";
            string password = "111000";
            //            Customer customer = (Customer)Helper.Login(email, password);
            Customer customer = new Customer(1, "", email, 0, DateTime.Now, "");
            int actualCartCount = 1;

            //Act
            customer.AddToCart(firstProduct);
            customer.AddToCart(secondProduct);
            int cartCount = customer.RemoveFromCart(0);

            //Assert
            Assert.AreEqual(cartCount, actualCartCount, "Should return 1 as cart count after adding 2 products and remove one product");
        }
        
        [TestMethod]
        public void CheckoutTest()
        {
            //Arrange
            Product firstProduct = Helper.GetProduct(1);
            Product secondProduct = Helper.GetProduct(2);

            string email = "ayo@test.com";
            string password = "111000";
//            Customer customer = (Customer)Helper.Login(email, password);
            Customer customer = new Customer(1, "", email, 0, DateTime.Now, "");

            //Act
            customer.AddToCart(firstProduct);
            customer.AddToCart(secondProduct);
            int cartCountBeforeCheckout = customer.Cart.ProductsInCart.Count;
            customer.Checkout();
            int i = customer.Cart.ProductsInCart.Count;
            int cartCountAfterCheckout = customer.Cart.ProductsInCart.Count;

            //Assert
            Assert.AreNotEqual(cartCountBeforeCheckout, cartCountAfterCheckout, "Should clear cart on successful checkout");
        }
    }
}
