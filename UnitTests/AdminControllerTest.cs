namespace UnitTests
{
    #region

    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    using DrinkBuyer.Domain.Abstract;
    using DrinkBuyer.Domain.Entities;
    using DrinkBuyer.WebUI.Controllers;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Moq;

    #endregion

    ///<summary>
    ///  This is a test class for AdminControllerTest and is intended to contain all AdminControllerTest Unit Tests
    ///</summary>
    [TestClass]
    public class AdminControllerTest
    {
        #region Public Properties

        ///<summary>
        ///  Gets or sets the test context which provides information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext { get; set; }

        #endregion

        #region Public Methods and Operators

        ///<summary>
        ///  Ensures that when a valid ProductID is passed as a parameter, the action method calls the DeleteProduct method of the repository and passes the correct Product object to be deleted
        ///</summary>
        [TestMethod]
        public void CanDeleteValidProducts()
        {
            // Arrange - create a Product
            var prod = new Product { ProductID = 2, Name = "Test" };

            // Arrange - create the mock repository
            var mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(
                new[] { new Product { ProductID = 1, Name = "P1" }, prod, new Product { ProductID = 3, Name = "P3" }, }.
                    AsQueryable());

            // Arrange - create the controller
            var target = new AdminController(mock.Object);

            // Act - delete the product
            target.Delete(prod.ProductID);

            // Assert - ensure that the repository delete method was
            // called with the correct Product
            mock.Verify(m => m.DeleteProduct(prod));
        }

        ///<summary>
        ///  Ensures that we get (and can edit) the product we ask for when we provide a valid ID value.
        ///</summary>
        [TestMethod]
        public void CanEditProduct()
        {
            // Arrange - create the mock repository
            var mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(
                new[]
                    {
                        new Product { ProductID = 1, Name = "P1" }, new Product { ProductID = 2, Name = "P2" }, 
                        new Product { ProductID = 3, Name = "P3" }, 
                    }.AsQueryable());

            // Arrange - create the controller
            var target = new AdminController(mock.Object);

            // Act
            var p1 = target.Edit(1).ViewData.Model as Product;
            var p2 = target.Edit(2).ViewData.Model as Product;
            var p3 = target.Edit(3).ViewData.Model as Product;

            // Assert
            Assert.AreEqual(1, p1.ProductID);
            Assert.AreEqual(2, p2.ProductID);
            Assert.AreEqual(3, p3.ProductID);
        }

        ///<summary>
        ///  Ensures that valid updates to the Product object that the model binder has created are passed to the product repository to be saved
        ///</summary>
        [TestMethod]
        public void CanSaveValidChanges()
        {
            // Arrange - create mock repository
            var mock = new Mock<IProductRepository>();

            // Arrange - create the controller
            var target = new AdminController(mock.Object);

            // Arrange - create a product
            var product = new Product { Name = "Test" };

            // Act - try to save the product
            ActionResult result = target.Edit(product, null);

            // Assert - check that the repository was called
            mock.Verify(m => m.SaveProduct(product));

            // Assert - check the method result type
            Assert.IsNotInstanceOfType(result, typeof(ViewResult));
        }

        ///<summary>
        ///  Ensures that if the parameter value passed to the Delete method does not correspond to a valid product in the repository, the repository DeleteProduct method is not called
        ///</summary>
        [TestMethod]
        public void CannotDeleteInvalidProducts()
        {
            // Arrange - create the mock repository
            var mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(
                new[]
                    {
                        new Product { ProductID = 1, Name = "P1" }, new Product { ProductID = 2, Name = "P2" }, 
                        new Product { ProductID = 3, Name = "P3" }, 
                    }.AsQueryable());

            // Arrange - create the controller
            var target = new AdminController(mock.Object);

            // Act - delete using an ID that doesn't exist
            target.Delete(100);

            // Assert - ensure that the repository delete method was
            // called with the correct Product
            mock.Verify(m => m.DeleteProduct(It.IsAny<Product>()), Times.Never());
        }

        ///<summary>
        ///  Ensures that we do not get a product when we request an ID value that is not in the repository.
        ///</summary>
        [TestMethod]
        public void CannotEditNonexistentProduct()
        {
            // Arrange - create the mock repository
            var mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(
                new[]
                    {
                        new Product { ProductID = 1, Name = "P1" }, new Product { ProductID = 2, Name = "P2" }, 
                        new Product { ProductID = 3, Name = "P3" }, 
                    }.AsQueryable());

            // Arrange - create the controller
            var target = new AdminController(mock.Object);

            // Act
            var result = (Product)target.Edit(4).ViewData.Model;

            // Assert
            Assert.IsNull(result);
        }

        ///<summary>
        ///  Ensures that invalid updates (where a model error exists) are not passed to the repository.
        ///</summary>
        [TestMethod]
        public void CannotSaveInvalidChanges()
        {
            // Arrange - create mock repository
            var mock = new Mock<IProductRepository>();

            // Arrange - create the controller
            var target = new AdminController(mock.Object);

            // Arrange - create a product
            var product = new Product { Name = "Test" };

            // Arrange - add an error to the model state
            target.ModelState.AddModelError("error", "error");

            // Act - try to save the product
            ActionResult result = target.Edit(product, null);

            // Assert - check that the repository was not called
            mock.Verify(m => m.SaveProduct(It.IsAny<Product>()), Times.Never());

            // Assert - check the method result type
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        ///<summary>
        ///  Ensures that the Index method correctly returns the Product objects that are in the repository
        ///</summary>
        [TestMethod]
        public void IndexContainsAllProducts()
        {
            // Arrange - create the mock repository
            var mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(
                new[]
                    {
                        new Product { ProductID = 1, Name = "P1" }, new Product { ProductID = 2, Name = "P2" }, 
                        new Product { ProductID = 3, Name = "P3" }, 
                    }.AsQueryable());

            // Arrange - create a controller
            var target = new AdminController(mock.Object);

            // Action
            Product[] result = ((IEnumerable<Product>)target.Index().ViewData.Model).ToArray();

            // Assert
            Assert.AreEqual(result.Length, 3);
            Assert.AreEqual("P1", result[0].Name);
            Assert.AreEqual("P2", result[1].Name);
            Assert.AreEqual("P3", result[2].Name);
        }

        #endregion
    }
}