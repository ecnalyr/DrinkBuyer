// -----------------------------------------------------------------------
// <copyright file="Cart.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace DrinkBuyer.Domain.Entities
{
    #region

    using System.Collections.Generic;
    using System.Linq;

    #endregion

    /// <summary>
    ///   Shopping cart manager, a CartLine is a line in the shopping cart
    /// </summary>
    public class Cart
    {
        #region Fields

        private readonly List<CartLine> lineCollection = new List<CartLine>();

        #endregion

        #region Public Properties

        public IEnumerable<CartLine> Lines
        {
            get
            {
                return this.lineCollection;
            }
        }

        #endregion

        #region Public Methods and Operators

        public void AddItem(Product product, int quantity)
        {
            CartLine line = this.lineCollection.Where(p => p.Product.ProductID == product.ProductID).FirstOrDefault();
            if (line == null)
            {
                this.lineCollection.Add(new CartLine { Product = product, Quantity = quantity });
            }
            else
            {
                line.Quantity += quantity;
            }
        }

        public void Clear()
        {
            this.lineCollection.Clear();
        }

        public decimal ComputeTotalValue()
        {
            return this.lineCollection.Sum(e => e.Product.Price * e.Quantity);
        }

        public void RemoveLine(Product product)
        {
            this.lineCollection.RemoveAll(l => l.Product.ProductID == product.ProductID);
        }

        #endregion
    }

    public class CartLine
    {
        #region Public Properties

        public Product Product { get; set; }

        public int Quantity { get; set; }

        #endregion
    }
}