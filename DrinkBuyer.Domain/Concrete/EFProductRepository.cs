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
    }
}
