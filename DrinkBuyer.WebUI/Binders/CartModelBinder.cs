namespace DrinkBuyer.WebUI.Binders
{
    #region

    using System.Web.Mvc;

    using DrinkBuyer.Domain.Entities;

    #endregion

    public class CartModelBinder : IModelBinder
    {
        #region Constants

        private const string sessionKey = "Cart";

        #endregion

        #region Public Methods and Operators

        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var cart = (Cart)controllerContext.HttpContext.Session[sessionKey];
            if (cart == null)
            {
                cart = new Cart();
                controllerContext.HttpContext.Session[sessionKey] = cart;
            }

            return cart;
        }

        #endregion
    }
}