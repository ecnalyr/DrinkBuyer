﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProductController.cs" company="">
//   
// </copyright>
// <summary>
//   The product controller.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DrinkBuyer.WebUI.Controllers
{
    using System.Linq;
    using System.Web.Mvc;

    using DrinkBuyer.Domain.Abstract;
    using DrinkBuyer.WebUI.Models;

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

        public ViewResult List(int page = 1)
        {
            var viewModel = new ProductsListViewModel
                {
                    Products =
                        this.repository.Products.OrderBy(p => p.ProductID).Skip((page - 1) * this.PageSize).Take(
                            this.PageSize), 
                    PagingInfo =
                        new PagingInfo
                            {
                                CurrentPage = page, 
                                ItemsPerPage = this.PageSize, 
                                TotalItems = this.repository.Products.Count()
                            }
                };
            return View(viewModel);
        }

        #endregion
    }
}