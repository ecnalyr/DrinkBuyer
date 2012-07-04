using System.Collections.Generic;
using DrinkBuyer.Domain.Entities;

namespace DrinkBuyer.WebUI.Models
{
    public class ProductsListViewModel
    {
        public IEnumerable<Product> Products { get; set; }

        public PagingInfo PagingInfo { get; set; }
    }
}