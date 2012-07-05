namespace DrinkBuyer.WebUI.Controllers
{
    #region

    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    using DrinkBuyer.Domain.Abstract;

    #endregion

    public class NavController : Controller
    {
        #region Fields

        private readonly IProductRepository repository;

        #endregion

        #region Constructors and Destructors

        public NavController(IProductRepository repo)
        {
            this.repository = repo;
        }

        #endregion

        #region Public Methods and Operators

        public PartialViewResult Menu(string category = null)
        {
            ViewBag.SelectedCategory = category;

            IEnumerable<string> categories = this.repository.Products.Select(x => x.Category).Distinct().OrderBy(x => x);

            return this.PartialView(categories);
        }

        #endregion
    }
}