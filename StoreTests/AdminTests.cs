using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FunctionalityLibrary;

namespace StoreTests
{
    [TestClass]
    public class AdminTests
    {
        public AdminTests()
        {
            Helper.InitializeDatabase();
        }
        public int Id { get; set; } = 1;

        [TestMethod]
        public void AddProductTest()
        {
            //Arrange
            string productName = "Redmi Note 7";
            string productDesc = "Black, 4gb ram, 64gb";
            double productPrice = 72333.99;
            int productUnit = 6;
            Admin admin = new Admin(1, "admin", "admin@test.com", DateTime.Now);

            //Act
            bool success = admin.AddProduct(productName, productDesc, productPrice, productUnit);

            //Assert
            Assert.IsTrue(success);
        }

        [TestMethod]
        public void ViewAllCustomerTests()
        {
            //Arrange
            Admin admin = new Admin(1, "admin", "admin@test.com", DateTime.Now);
            List<Customer> customers = admin.ViewAllCustomers();

            //Assert
            Assert.IsTrue(customers.Count > 0, "View all customer should return list of customer");
        }
        
        [TestMethod]
        public void ViewAllOrderTests()
        {
            //Arrange
            Admin admin = new Admin(1, "admin", "admin@test.com", DateTime.Now);
            List<Order> orders = admin.ViewAllOrders();

            //Assert
            Assert.IsTrue(orders.Count > 0, "View all order should return list of order");
        }
    }
}