namespace UnitTests
{
    #region

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
    ///  This is a test class for CartControllerTest and is intended to contain all CartControllerTest Unit Tests
    ///</summary>
    [TestClass]
    public class CartControllerTest
    {
        #region Public Properties

        ///<summary>
        ///  Gets or sets the test context which provides information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext { get; set; }

        #endregion

        #region Public Methods and Operators

        ///<summary>
        ///  Ensure we are redirected to the Index view after adding a product to the cart, and ensure that the URL that the user can follow to return to the catalog should be correctly passed to the Index action method
        ///</summary>
        [TestMethod]
        public void AddingProductToCartGoesToCartScreen()
        {
            // Arrange - create the mock repository
            var mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(
                new[] { new Product { ProductID = 1, Name = "P1", Category = "Apples" }, }.AsQueryable());

            // Arrange - create a Cart
            var cart = new Cart();

            // Arrange - create the controller
            var target = new CartController(mock.Object, null);

            // Act - add a product to the cart
            RedirectToRouteResult result = target.AddToCart(cart, 2, "myUrl");

            // Assert
            Assert.AreEqual(result.RouteValues["action"], "Index");
            Assert.AreEqual(result.RouteValues["returnUrl"], "myUrl");
        }

        ///<summary>
        ///  Ensure we can add selected items to the user's cart.
        ///</summary>
        [TestMethod]
        public void CanAddToCart()
        {
            // Arrange - create the mock repository
            var mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(
                new[] { new Product { ProductID = 1, Name = "P1", Category = "Apples" }, }.AsQueryable());

            // Arrange - create a Cart
            var cart = new Cart();

            // Arrange - create the controller
            var target = new CartController(mock.Object, null);

            // Act - add a product to the cart
            target.AddToCart(cart, 1, null);

            // Assert
            Assert.AreEqual(cart.Lines.Count(), 1);
            Assert.AreEqual(cart.Lines.ToArray()[0].Product.ProductID, 1);
        }

        ///<summary>
        ///  Ensures that we can process orders when appropriate - as opposed to when the cart is empty or there are invalid shipping details.
        ///</summary>
        [TestMethod]
        public void CanCheckoutAndSubmitOrder()
        {
            // Arrange - create a mock order processor
            var mock = new Mock<IOrderProcessor>();

            // Arrange - create a cart with an item
            var cart = new Cart();
            cart.AddItem(new Product(), 1);

            // Arrange - create an instance of the controller
            var target = new CartController(null, mock.Object);

            // Act - try to checkout
            ViewResult result = target.Checkout(cart, new ShippingDetails());

            // Assert - check that the order has been passed on to the processor
            mock.Verify(m => m.ProcessOrder(It.IsAny<Cart>(), It.IsAny<ShippingDetails>()), Times.Once());

            // Assert - check that the method is returning the Completed view
            Assert.AreEqual("Completed", result.ViewName);

            // Assert - check that we are passing a valid model to the view
            Assert.AreEqual(true, result.ViewData.ModelState.IsValid);
        }

        ///<summary>
        ///  Ensure we can view the appropriate cart (not some other session's cart), and ensure that the URL that the user can follow to return to the catalog should be correctly passed to the Index action method
        ///</summary>
        [TestMethod]
        public void Can_View_Cart_Contents()
        {
            // Arrange - create a Cart
            var cart = new Cart();

            // Arrange - create the controller
            var target = new CartController(null, null);

            // Act - call the Index action method
            var result = (CartIndexViewModel)target.Index(cart, "myUrl").ViewData.Model;

            // Assert
            Assert.AreSame(result.Cart, cart);
            Assert.AreEqual(result.ReturnUrl, "myUrl");
        }

        ///<summary>
        ///  Ensure that we cannot checkout with an empty cart.
        ///</summary>
        [TestMethod]
        public void CannotCheckoutEmptyCart()
        {
            // Arrange - create a mock order processor
            var mock = new Mock<IOrderProcessor>();

            // Arrange - create an empty cart
            var cart = new Cart();

            // Arrange - create shipping details
            var shippingDetails = new ShippingDetails();

            // Arrange - create an instance of the controller
            var target = new CartController(null, mock.Object);

            // Act
            ViewResult result = target.Checkout(cart, shippingDetails);

            // Assert - check that the order hasn't been passed on to the processor
            mock.Verify(m => m.ProcessOrder(It.IsAny<Cart>(), It.IsAny<ShippingDetails>()), Times.Never());

            // Assert - check that the method is returning the default view
            Assert.AreEqual(string.Empty, result.ViewName);

            // Assert - check that we are passing an invalid model to the view
            Assert.AreEqual(false, result.ViewData.ModelState.IsValid);
        }

        ///<summary>
        ///  Behaves similarly to CannotCheckoutEmptyCart, but injects an error into the view model to simulate a problem reported by the model binder (which would happen in production when the customer enters invalid shipping data).
        ///</summary>
        [TestMethod]
        public void CannotCheckoutInvalidShippingDetails()
        {
            // Arrange - create a mock order processor
            var mock = new Mock<IOrderProcessor>();

            // Arrange - create a cart with an item
            var cart = new Cart();
            cart.AddItem(new Product(), 1);

            // Arrange - create an instance of the controller
            var target = new CartController(null, mock.Object);

            // Arrange - add an error to the model
            target.ModelState.AddModelError("error", "error");

            // Act - try to checkout
            ViewResult result = target.Checkout(cart, new ShippingDetails());

            // Assert - check that the order hasn't been passed on to the processor
            mock.Verify(m => m.ProcessOrder(It.IsAny<Cart>(), It.IsAny<ShippingDetails>()), Times.Never());

            // Assert - check that the method is returning the default view
            Assert.AreEqual(string.Empty, result.ViewName);

            // Assert - check that we are passing an invalid model to the view
            Assert.AreEqual(false, result.ViewData.ModelState.IsValid);
        }

        #endregion
    }
}