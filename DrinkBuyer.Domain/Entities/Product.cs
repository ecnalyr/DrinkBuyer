﻿// -----------------------------------------------------------------------
// <copyright file="Product.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace DrinkBuyer.Domain.Entities
{
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;

    using DrinkBuyer.Domain.Properties;
    using DrinkBuyer.Domain.Validators;

    /// <summary>
    /// An entity that can have a description, price, category, and image.
    /// </summary>
    public class Product
    {
        /// <summary>
        /// Gets or sets the entity ID of a product
        /// </summary>
        /// <value>
        /// An integer identifying the entity.
        /// </value>
        [HiddenInput(DisplayValue = false)]
        public int ProductID { get; set; }

        /// <summary>
        /// Gets or sets the name of the product
        /// </summary>
        /// <value>
        /// A string.
        /// </value>
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "ProductNameRequired", ErrorMessageResourceType = typeof(Resources))]
        [Display(Name = "ProductNameLebelText", ResourceType = typeof(Resources))]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description of the product
        /// </summary>
        /// <value>
        /// A string.
        /// </value> 
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "ProductDescriptionRequired", ErrorMessageResourceType = typeof(Resources))]
        [DataType(DataType.MultilineText)]
        [Display(Name = "ProductDescriptionLabelText", ResourceType = typeof(Resources))]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the price of the product
        /// </summary>
        /// <value>
        /// A positive decimal.
        /// </value>
        [Required(ErrorMessageResourceName = "ProductPriceRequired", ErrorMessageResourceType = typeof(Resources))]
        [Range(0.01, double.MaxValue, ErrorMessageResourceName = "ProductPriceMustBePositiveError", ErrorMessageResourceType = typeof(Resources))]
        [Display(Name = "ProductPriceLabelText", ResourceType = typeof(Resources))]
        public decimal Price { get; set; }

        /// <summary>
        /// Gets or sets the category of the product
        /// </summary>
        /// <value>
        /// A one-word string, only alpha-numeric characters and [.,_-;] are allowed
        /// </value>
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "ProductCategoryRequired", ErrorMessageResourceType = typeof(Resources))]
        [StringLength(40, ErrorMessageResourceName = "ProductCategoryLengthError", ErrorMessageResourceType = typeof(Resources))]
        [TextLineInputValidatorAtribute]
        [Display(Name = "ProductCategoryLabelText", ResourceType = typeof(Resources))]
        public string Category { get; set; }

        /// <summary>
        /// Gets or sets the sub-category of the product
        /// </summary>
        /// <value>
        /// A one-word string, only alpha-numeric characters and [.,_-;] are allowed
        /// </value>
        [Required(AllowEmptyStrings = false, ErrorMessageResourceName = "ProductSubCategoryRequired",
            ErrorMessageResourceType = typeof(Resources))]
        [StringLength(40, ErrorMessageResourceName = "ProductSubCategoryLengthError",
            ErrorMessageResourceType = typeof(Resources))]
        [TextLineInputValidatorAtribute]
        [Display(Name = "ProductSubCategoryLabelText", ResourceType = typeof(Resources))]
        public string SubCategory { get; set; }


        public byte[] ImageData { get; set; }

        [HiddenInput(DisplayValue = false)]
        [StringLength(100)]
        public string ImageMimeType { get; set; }

    }
}
