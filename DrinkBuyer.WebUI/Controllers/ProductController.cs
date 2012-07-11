// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProductController.cs" company="">
// </copyright>
// <summary>
//   The product controller.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DrinkBuyer.WebUI.Controllers
{
    #region

    using System.Linq;
    using System.Web.Mvc;

    using DrinkBuyer.Domain.Abstract;
    using DrinkBuyer.WebUI.Models;
    using DrinkBuyer.Domain.Entities;

    #endregion

    public class ProductController : Controller
    {
        #region Fields

        public int PageSize = 4;

        private readonly IProductRepository repository;

        #endregion

        #region Constructors and Destructors

        public ProductController(IProductRepository productRepository)
        {
            this.repository = productRepository;
        }

        #endregion

        #region Public Methods and Operators

        public ViewResult List(string category, int page = 1)
        {
            var viewModel = new ProductsListViewModel
                {
                    Products =
                        this.repository.Products.Where(p => category == null || p.Category == category).OrderBy(
                            p => p.ProductID).Skip((page - 1) * this.PageSize).Take(this.PageSize), 
                    PagingInfo =
                        new PagingInfo
                            {
                                CurrentPage = page, 
                                ItemsPerPage = this.PageSize, 
                                TotalItems =
                                    category == null
                                        ? this.repository.Products.Count()
                                        : this.repository.Products.Count(e => e.Category == category)
                            }, 
                    Currentcategory = category
                };
            return View(viewModel);
        }

        public ViewResult SubCategory(string category, string subCategory)
        {
            var viewModel = new SubCategoryViewModel
                {
                    Products = this.repository.Products.Where(p => p.Category == category && p.SubCategory == subCategory),
                    CurrentCategory = category,
                    CurrentSubCategory = subCategory
                };
            return View(viewModel);
        }

        public FileContentResult GetImage(int productId)
        {
            Product product = repository.Products.FirstOrDefault(p => p.ProductID == productId);
            if (product != null)
            {
                return File(product.ImageData, product.ImageMimeType);
            }
            else
            {
                return null;
            }
        }

        #endregion
    }
}