// -----------------------------------------------------------------------
// <copyright file="EFProductRepository.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace DrinkBuyer.Domain.Concrete
{
    using System.Linq;

    using DrinkBuyer.Domain.Abstract;
    using DrinkBuyer.Domain.Entities;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class EFProductRepository : IProductRepository
    {
        private EFDbContext context = new EFDbContext();

        public IQueryable<Product> Products
        {
            get
            {
                return context.Products; 
            }
        }

        public void SaveProduct(Product product)
        {
            if (product.ProductID == 0)
            {
                context.Products.Add(product);
            }
            else
            {
                context.Entry(product).State = System.Data.EntityState.Modified;
            }
            context.SaveChanges();
        }

        public void DeleteProduct(Product product)
        {
            context.Products.Remove(product);
            context.SaveChanges();
        }
    }
}
