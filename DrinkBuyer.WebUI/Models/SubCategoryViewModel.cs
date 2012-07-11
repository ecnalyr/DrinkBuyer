using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DrinkBuyer.WebUI.Models
{
    using DrinkBuyer.Domain.Entities;

    public class SubCategoryViewModel
    {
        public IEnumerable<Product> Products { get; set; }

        public string CurrentCategory { get; set; }

        public string CurrentSubCategory { get; set; }
    }
}