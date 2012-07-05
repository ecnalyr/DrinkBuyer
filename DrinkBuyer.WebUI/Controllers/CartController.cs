namespace DrinkBuyer.WebUI.Controllers
{
    #region

    using System.Linq;
    using System.Web.Mvc;

    using DrinkBuyer.Domain.Abstract;
    using DrinkBuyer.Domain.Entities;
    using DrinkBuyer.WebUI.Models;

    #endregion

    public class CartController : Controller
    {
        #region Fields

        private readonly IOrderProcessor orderProcessor;

        private readonly IProductRepository repository;

        #endregion

        #region Constructors and Destructors

        public CartController(IProductRepository repo, IOrderProcessor processor)
        {
            this.repository = repo;
            this.orderProcessor = processor;
        }

        #endregion

        #region Public Methods and Operators

        public RedirectToRouteResult AddToCart(Cart cart, int productId, string returnUrl)
        {
            Product product = this.repository.Products.FirstOrDefault(p => p.ProductID == productId);

            if (product != null)
            {
                cart.AddItem(product, 1);
            }

            return this.RedirectToAction("Index", new { returnUrl });
        }

        public ViewResult Checkout()
        {
            return this.View(new ShippingDetails());
        }

        [HttpPost]
        public ViewResult Checkout(Cart cart, ShippingDetails shippingDetails)
        {
            if (cart.Lines.Count() == 0)
            {
                this.ModelState.AddModelError(string.Empty, "Sorry, your cart is empty!");
            }

            if (this.ModelState.IsValid)
            {
                this.orderProcessor.ProcessOrder(cart, shippingDetails);
                cart.Clear();
                return this.View("Completed");
            }
            else
            {
                return this.View(shippingDetails);
            }
        }

        public ViewResult Index(Cart cart, string returnUrl)
        {
            return this.View(new CartIndexViewModel { Cart = cart, ReturnUrl = returnUrl });
        }

        public RedirectToRouteResult RemoveFromCart(Cart cart, int productId, string returnUrl)
        {
            Product product = this.repository.Products.FirstOrDefault(p => p.ProductID == productId);
            if (product != null)
            {
                cart.RemoveLine(product);
            }

            return this.RedirectToAction("Index", new { returnUrl });
        }

        public ViewResult Summary(Cart cart)
        {
            return this.View(cart);
        }

        #endregion

        #region Methods

        private Cart GetCart()
        {
            var cart = (Cart)this.Session["Cart"];
            if (cart == null)
            {
                cart = new Cart();
                this.Session["Cart"] = cart;
            }

            return cart;
        }

        #endregion
    }
}