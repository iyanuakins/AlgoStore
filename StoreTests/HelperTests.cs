using System;
using System.Collections.Generic;
using FunctionalityLibrary;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace StoreTests
{
    [TestClass]
    public class HelperTests
    {
        public HelperTests()
        {
            Helper.InitializeDatabase();
        }

        [TestMethod]
        public void RegistrationTests()
        {
            //Arrange
            string name = "Ayo ola";
            string email = "ayo@gmail.com";
            string password = "111111";
            string address = "Lekki,lagos";
            int actual = 1;

            //Act
            int row = Helper.Register(name, email, password, address);

            //Assert
            Assert.AreEqual(row, actual, "Registration should return 1 row affected by inserting a new user to DB");
        }

        [TestMethod]
        public void CheckMailTests()
        {
            //Arrange
            string email = "ayo@test.com";

            //Act
            bool response = Helper.CheckEmail(email);

            //Assert
            Assert.IsTrue(response, "Email check should return true when email is in use");
        }
        
        [TestMethod]
        public void SuccessLoginTests()
        {
            //Arrange
            string email = "ayo@test.com";
            string password = "111111";

            //Act
            Customer user = (Customer)Helper.Login(email, password);

            //Assert
            Assert.IsInstanceOfType(user, typeof(Customer), "Login should return an instance of customer on successful login");
        }
        
        [TestMethod]
        public void FailedLoginTests()
        {
            //Arrange
            string email = "ayo@test.com";
            string password = "111000";

            //Act
            Customer user = (Customer)Helper.Login(email, password);

            //Assert
            Assert.IsNull(user, "Login should return an null on failed login");
        }
        
        [TestMethod]
        public void GetProductTests()
        {
            //Arrange and Act
            Product product = Helper.GetProduct(1);

            //Assert
            Assert.IsInstanceOfType(product, typeof(Product), "Get product should return an instance of product");
        }
        
        [TestMethod]
        public void ViewAllProductTests()
        {
            //Arrange and Act
            List<Product> products = Helper.ViewAllProducts();

            //Assert
            Assert.IsTrue(products.Count > 0, "View all product should return list of product");
        }
    }
}