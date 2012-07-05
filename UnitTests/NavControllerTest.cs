namespace UnitTests
{
    #region

    using System.Collections.Generic;
    using System.Linq;

    using DrinkBuyer.Domain.Abstract;
    using DrinkBuyer.Domain.Entities;
    using DrinkBuyer.WebUI.Controllers;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Moq;

    #endregion

    ///<summary>
    ///  This is a test class for NavControllerTest and is intended to contain all NavControllerTest Unit Tests
    ///</summary>
    [TestClass]
    public class NavControllerTest
    {
        #region Public Properties

        ///<summary>
        ///  Gets or sets the test context which provides information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext { get; set; }

        #endregion

        #region Public Methods and Operators

        ///<summary>
        ///  A test to see if we can create category links in the proper alphabetical order with no duplicates
        ///</summary>
        [TestMethod]
        public void CanCreateCategories()
        {
            // Arrange
            // - create the mock repository
            var mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(
                new[]
                    {
                        new Product { ProductID = 1, Name = "P1", Category = "Apples" }, 
                        new Product { ProductID = 2, Name = "P2", Category = "Apples" }, 
                        new Product { ProductID = 3, Name = "P3", Category = "Plums" }, 
                        new Product { ProductID = 4, Name = "P4", Category = "Oranges" }, 
                    }.AsQueryable());

            // Arrange - create the controller
            var target = new NavController(mock.Object);

            // Act = get the set of categories
            string[] results = ((IEnumerable<string>)target.Menu().Model).ToArray();

            // Assert
            Assert.AreEqual(results.Length, 3);
            Assert.AreEqual(results[0], "Apples");
            Assert.AreEqual(results[1], "Oranges");
            Assert.AreEqual(results[2], "Plums");
        }

        ///<summary>
        ///  A test to see if the ViewBag.SelectedCategory contains an appropriate Category
        ///</summary>
        [TestMethod]
        public void IndicatesSelectedCategory()
        {
            // Arrange
            // - create the mock repository
            var mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(
                new[]
                    {
                        new Product { ProductID = 1, Name = "P1", Category = "Apples" }, 
                        new Product { ProductID = 4, Name = "P2", Category = "Oranges" }, 
                    }.AsQueryable());

            // Arrange - create the controller
            var target = new NavController(mock.Object);

            // Arrange - define the category to selected
            string categoryToSelect = "Apples";

            // Action
            string result = target.Menu(categoryToSelect).ViewBag.SelectedCategory;

            // Assert
            Assert.AreEqual(categoryToSelect, result);
        }

        #endregion
    }
}