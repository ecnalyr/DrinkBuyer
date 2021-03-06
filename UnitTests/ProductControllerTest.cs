﻿namespace UnitTests
{
    #region

    using System.Diagnostics;
    using System.Linq;
    using System.Web.Mvc;

    using DrinkBuyer.Domain.Abstract;
    using DrinkBuyer.Domain.Entities;
    using DrinkBuyer.WebUI.Controllers;
    using DrinkBuyer.WebUI.Models;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Moq;

    #endregion

    ///<summary>
    ///  This is a test class for ProductControllerTest and is intended to contain all ProductControllerTest Unit Tests
    ///</summary>
    [TestClass]
    public class ProductControllerTest
    {
        #region Public Properties

        ///<summary>
        ///  Gets or sets the test context which provides information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext { get; set; }

        #endregion

        #region Public Methods and Operators

        ///<summary>
        ///  A test to ensure we can filter products by category
        ///</summary>
        [TestMethod]
        public void CanFilterProducts()
        {
            // Arrange
            // - create the mock repository
            var mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(
                new[]
                    {
                        new Product { ProductID = 1, Name = "P1", Category = "Cat1" }, 
                        new Product { ProductID = 2, Name = "P2", Category = "Cat2" }, 
                        new Product { ProductID = 3, Name = "P3", Category = "Cat1" }, 
                        new Product { ProductID = 4, Name = "P4", Category = "Cat2" }, 
                        new Product { ProductID = 5, Name = "P5", Category = "Cat3" }
                    }.AsQueryable());

            // Arrange - create a controller and make the page size 3 items
            var controller = new ProductController(mock.Object);
            controller.PageSize = 3;

            // Action
            Product[] result = ((ProductsListViewModel)controller.List("Cat2", 1).Model).Products.ToArray();

            // Assert
            Assert.AreEqual(result.Length, 2);
            Assert.IsTrue(result[0].Name == "P2" && result[0].Category == "Cat2");
            Assert.IsTrue(result[1].Name == "P4" && result[1].Category == "Cat2");
        }

        ///<summary>
        ///  A test to see if we can Paginate - that is, create pages
        ///</summary>
        [TestMethod]
        public void CanPaginateProducts()
        {
            // Arrange
            // - create the mock repository
            var mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(
                new[]
                    {
                        new Product { ProductID = 1, Name = "P1" }, new Product { ProductID = 2, Name = "P2" }, 
                        new Product { ProductID = 3, Name = "P3" }, new Product { ProductID = 4, Name = "P4" }, 
                        new Product { ProductID = 5, Name = "P5" }
                    }.AsQueryable());

            // create a controller and make the page size 3 items
            var controller = new ProductController(mock.Object);
            controller.PageSize = 3;

            // Action
            var result = (ProductsListViewModel)controller.List(null, 2).Model;

            // Assert
            Product[] prodArray = result.Products.ToArray();
            Assert.IsTrue(prodArray.Length == 2);
            Assert.AreEqual(prodArray[0].Name, "P4");
            Assert.AreEqual(prodArray[1].Name, "P5");
        }

        ///<summary>
        ///  A test for List to see if we can send pagination to the view model
        ///</summary>
        [TestMethod]
        public void CanSendPaginationViewModel()
        {
            // Arrange
            // - create the mock repository
            var mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(
                new[]
                    {
                        new Product { ProductID = 1, Name = "P1" }, new Product { ProductID = 2, Name = "P2" }, 
                        new Product { ProductID = 3, Name = "P3" }, new Product { ProductID = 4, Name = "P4" }, 
                        new Product { ProductID = 5, Name = "P5" }
                    }.AsQueryable());

            // Arrange - create a controller and make the page size 3 items
            var controller = new ProductController(mock.Object);
            controller.PageSize = 3;

            // Action
            var result = (ProductsListViewModel)controller.List(null, 2).Model;

            // Assert
            PagingInfo pageInfo = result.PagingInfo;
            Assert.AreEqual(pageInfo.CurrentPage, 2);
            Assert.AreEqual(pageInfo.ItemsPerPage, 3);
            Assert.AreEqual(pageInfo.TotalItems, 5);
            Assert.AreEqual(pageInfo.TotalPages, 2);
        }

        ///<summary>
        ///  A test to see if the GetImage method returns the correct MIME type from the repository.
        ///</summary>
        [TestMethod]
        public void CanRetrieveImageData()
        {
            // Arrange - create a Product with image data
            var prod = new Product
                {
                   ProductID = 2, Name = "Test", ImageData = new byte[] { }, ImageMimeType = "image/png" 
                };

            // Arrange - create the mock repository
            var mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(
                new[] { new Product { ProductID = 1, Name = "P1" }, prod, new Product { ProductID = 3, Name = "P3" } }.
                    AsQueryable());

            // Arrange - create the controller
            var target = new ProductController(mock.Object);

            // Act - call the GetImage action method
            ActionResult result = target.GetImage(2);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(FileResult));
            Assert.AreEqual(prod.ImageMimeType, ((FileResult)result).ContentType);
        }

        ///<summary>
        ///  A test to make sure that no data is returned when we request a Product ID that doesn't exist.
        ///</summary>
        [TestMethod]
        public void CannotRetrieveImageDataForInvalidID()
        {
            // Arrange - create the mock repository
            var mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(
                new[] { new Product { ProductID = 1, Name = "P1" }, new Product { ProductID = 2, Name = "P2" } }.
                    AsQueryable());

            // Arrange - create the controller
            var target = new ProductController(mock.Object);

            // Act - call the GetImage action method
            ActionResult result = target.GetImage(100);

            // Assert
            Assert.IsNull(result);
        }

        ///<summary>
        ///  A test to see if we can generate a category-specific product count. Calls the List action method requesting each category in turn, then tries the List method with no category.
        ///</summary>
        [TestMethod]
        public void GenerateCategorySpecificProductCount()
        {
            // Arrange
            // - create the mock repository
            var mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(
                new[]
                    {
                        new Product { ProductID = 1, Name = "P1", Category = "Cat1" }, 
                        new Product { ProductID = 2, Name = "P2", Category = "Cat2" }, 
                        new Product { ProductID = 3, Name = "P3", Category = "Cat1" }, 
                        new Product { ProductID = 4, Name = "P4", Category = "Cat2" }, 
                        new Product { ProductID = 5, Name = "P5", Category = "Cat3" }
                    }.AsQueryable());

            // Arrange - create a controller and make the page size 3 items
            var target = new ProductController(mock.Object);
            target.PageSize = 3;

            // Action - test the product counts for different categories
            int category1ProductCount = ((ProductsListViewModel)target.List("Cat1").Model).PagingInfo.TotalItems;
            int category2ProductCount = ((ProductsListViewModel)target.List("Cat2").Model).PagingInfo.TotalItems;
            int category3ProductCount = ((ProductsListViewModel)target.List("Cat3").Model).PagingInfo.TotalItems;
            int allCategoriesProductCount = ((ProductsListViewModel)target.List(null).Model).PagingInfo.TotalItems;

            // Assert
            Assert.AreEqual(category1ProductCount, 2);
            Assert.AreEqual(category2ProductCount, 2);
            Assert.AreEqual(category3ProductCount, 1);
            Assert.AreEqual(allCategoriesProductCount, 5);
        }

        ///<summary>
        ///  A test to see if we can generate a sub-category-specific product count. Calls the List action method requesting various sub-categories from various categories
        ///</summary>
        [TestMethod]
        public void GenerateSubCategorySpecificProductCount()
        {
            // Arrange
            // - create the mock repository
            var mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(
                new[]
                    {
                        new Product { ProductID = 1, Name = "P1", Category = "Cat1", SubCategory = "Liquid" },
                        new Product { ProductID = 2, Name = "P2", Category = "Cat2", SubCategory = "Liquid" },
                        new Product { ProductID = 3, Name = "P3", Category = "Cat1", SubCategory = "Solid" },
                        new Product { ProductID = 4, Name = "P4", Category = "Cat2", SubCategory = "Liquid" }, 
                        new Product { ProductID = 5, Name = "P5", Category = "Cat2", SubCategory = "Solid" }
                    }.AsQueryable());

            // Arrange - create a controller and make the page size 3 items
            var target = new ProductController(mock.Object);

            // Action - test the product counts for different categories
            int subcatLiquidCat1ProductCount = ((SubCategoryViewModel)target.SubCategory("Cat1", "Liquid").Model).Products.Count();
            int subCatLiquidCat2ProductCount = ((SubCategoryViewModel)target.SubCategory("Cat2", "Liquid").Model).Products.Count();
            int subCatSolidCat2ProductCount = ((SubCategoryViewModel)target.SubCategory("Cat2", "Solid").Model).Products.Count();

            // Assert
            Assert.AreEqual(subcatLiquidCat1ProductCount, 1);
            Assert.AreEqual(subCatLiquidCat2ProductCount, 2);
            Assert.AreEqual(subCatSolidCat2ProductCount, 1);
        }

        #endregion
    }
}