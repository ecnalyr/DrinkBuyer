namespace UnitTests
{
    #region

    using System;
    using System.Reflection;
    using System.Web;
    using System.Web.Routing;

    using DrinkBuyer.WebUI;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Moq;

    #endregion

    ///<summary>
    ///  This is a test class for MvcApplicationTest and is intended to contain all MvcApplicationTest Unit Tests
    ///</summary>
    [TestClass]
    public class MvcApplicationTest
    {
        #region Public Properties

        ///<summary>
        ///  Gets or sets the test context which provides information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext { get; set; }

        #endregion

        #region Public Methods and Operators

        ///<summary>
        ///  A test for RegisterRoutes
        ///</summary>
        // TODO: Add more tests - must cover ALL routes
        [TestMethod]
        public void TestIncomingRoutes()
        {
            // check for the URL that we hope to receive
            this.TestRouteMatch("~/", "Product", "List");
            this.TestRouteMatch("~/Anything/Else", "Anything", "Else");
            this.TestRouteMatch("~/Product/SubCategory/category/subcategory", "Product", "SubCategory", new { category = "category", subCategory = "subcategory" });

            // Need more tests here for all routes
            //this.TestRouteFail("~/Product/List/All");
        }

        #endregion

        #region Methods

        private HttpContextBase CreateHttpContext(string targetUrl = null, string httpMethod = "GET")
        {
            // create the mock request
            var mockRequest = new Mock<HttpRequestBase>();
            mockRequest.Setup(m => m.AppRelativeCurrentExecutionFilePath).Returns(targetUrl);
            mockRequest.Setup(m => m.HttpMethod).Returns(httpMethod);

            // create the mock response
            var mockResponse = new Mock<HttpResponseBase>();
            mockResponse.Setup(m => m.ApplyAppPathModifier(It.IsAny<string>())).Returns<string>(s => s);

            // create the mock context, using the request and response
            var mockContext = new Mock<HttpContextBase>();
            mockContext.Setup(m => m.Request).Returns(mockRequest.Object);
            mockContext.Setup(m => m.Response).Returns(mockResponse.Object);

            // return the mocked context
            return mockContext.Object;
        }

        private bool TestIncomingRouteResult(
            RouteData routeResult, string controller, string action, object propertySet = null)
        {
            Func<object, object, bool> valCompare =
                (v1, v2) => { return StringComparer.InvariantCultureIgnoreCase.Compare(v1, v2) == 0; };
            bool result = valCompare(routeResult.Values["controller"], controller)
                          && valCompare(routeResult.Values["action"], action);
            if (propertySet != null)
            {
                PropertyInfo[] propInfo = propertySet.GetType().GetProperties();
                foreach (PropertyInfo pi in propInfo)
                {
                    if (
                        !(routeResult.Values.ContainsKey(pi.Name)
                          && valCompare(routeResult.Values[pi.Name], pi.GetValue(propertySet, null))))
                    {
                        result = false;
                        break;
                    }
                }
            }

            return result;
        }

        private void TestRouteFail(string url)
        {
            // Arrange
            var routes = new RouteCollection();
            MvcApplication.RegisterRoutes(routes);

            // Act - process the route
            RouteData result = routes.GetRouteData(this.CreateHttpContext(url));

            // Assert
            Assert.IsTrue(result == null || result.Route == null);
        }

        private void TestRouteMatch(
            string url, string controller, string action, object routeProperties = null, string httpMethod = "GET")
        {
            // Arrange
            var routes = new RouteCollection();
            MvcApplication.RegisterRoutes(routes);

            // Act - process the route
            RouteData result = routes.GetRouteData(this.CreateHttpContext(url, httpMethod));

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(this.TestIncomingRouteResult(result, controller, action, routeProperties));
        }

        #endregion
    }
}