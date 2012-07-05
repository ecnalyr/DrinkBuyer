namespace UnitTests
{
    #region

    using System.Linq;

    using DrinkBuyer.Domain.Entities;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    #endregion

    ///<summary>
    ///  This is a test class for CartTest and is intended to contain all CartTest Unit Tests
    ///</summary>
    [TestClass]
    public class CartTest
    {
        #region Public Properties

        ///<summary>
        ///  Gets or sets the test context which provides information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext { get; set; }

        #endregion

        #region Public Methods and Operators

        ///<summary>
        ///  Tests ability to calculate the total cost of the items in the cart.
        ///</summary>
        [TestMethod]
        public void Calculate_Cart_Total()
        {
            // Arrange - create some test products
            var p1 = new Product { ProductID = 1, Name = "P1", Price = 100M };
            var p2 = new Product { ProductID = 2, Name = "P2", Price = 50M };

            // Arrange - create a new cart
            var target = new Cart();

            // Act
            target.AddItem(p1, 1);
            target.AddItem(p2, 1);
            target.AddItem(p1, 3);
            decimal result = target.ComputeTotalValue();

            // Assert
            Assert.AreEqual(result, 450M);
        }

        ///<summary>
        ///  If this is the first time that a given Product has been added to the cart, we want a new CartLine to be added
        ///</summary>
        [TestMethod]
        public void Can_Add_New_Lines()
        {
            // Arrange - create some test products
            var p1 = new Product { ProductID = 1, Name = "P1" };
            var p2 = new Product { ProductID = 2, Name = "P2" };

            // Arrange - create a new cart
            var target = new Cart();

            // Act
            target.AddItem(p1, 1);
            target.AddItem(p2, 1);
            CartLine[] results = target.Lines.ToArray();

            // Assert
            Assert.AreEqual(results.Length, 2);
            Assert.AreEqual(results[0].Product, p1);
            Assert.AreEqual(results[1].Product, p2);
        }

        ///<summary>
        ///  If the customer has already added a Product to the cart, we want to increment the quantity of the corresponding CartLine and not create a new CartLine.
        ///</summary>
        [TestMethod]
        public void Can_Add_Quantity_For_Existing_Lines()
        {
            // Arrange - create some test products
            var p1 = new Product { ProductID = 1, Name = "P1" };
            var p2 = new Product { ProductID = 2, Name = "P2" };

            // Arrange - create a new cart
            var target = new Cart();

            // Act
            target.AddItem(p1, 1);
            target.AddItem(p2, 1);
            target.AddItem(p1, 10);
            CartLine[] results = target.Lines.OrderBy(c => c.Product.ProductID).ToArray();

            // Assert
            Assert.AreEqual(results.Length, 2);
            Assert.AreEqual(results[0].Quantity, 11);
            Assert.AreEqual(results[1].Quantity, 1);
        }

        ///<summary>
        ///  Tests the contents of the cart are properly removed when we reset the entire cart.
        ///</summary>
        [TestMethod]
        public void Can_Clear_Contents()
        {
            // Arrange - create some test products
            var p1 = new Product { ProductID = 1, Name = "P1", Price = 100M };
            var p2 = new Product { ProductID = 2, Name = "P2", Price = 50M };

            // Arrange - create a new cart
            var target = new Cart();

            // Arrange - add some items
            target.AddItem(p1, 1);
            target.AddItem(p2, 1);

            // Act - reset the cart
            target.Clear();

            // Assert
            Assert.AreEqual(target.Lines.Count(), 0);
        }

        ///<summary>
        ///  Tests if a customer can remove products from the cart.
        ///</summary>
        [TestMethod]
        public void Can_Remove_Line()
        {
            // Arrange - create some test products
            var p1 = new Product { ProductID = 1, Name = "P1" };
            var p2 = new Product { ProductID = 2, Name = "P2" };
            var p3 = new Product { ProductID = 3, Name = "P3" };

            // Arrange - create a new cart
            var target = new Cart();

            // Arrange - add some products to the cart
            target.AddItem(p1, 1);
            target.AddItem(p2, 3);
            target.AddItem(p3, 5);
            target.AddItem(p2, 1);

            // Act
            target.RemoveLine(p2);

            // Assert
            Assert.AreEqual(target.Lines.Count(c => c.Product == p2), 0);
            Assert.AreEqual(target.Lines.Count(), 2);
        }

        #endregion
    }
}