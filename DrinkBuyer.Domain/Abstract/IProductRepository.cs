// -----------------------------------------------------------------------
// <copyright file="IProductRepository.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace DrinkBuyer.Domain.Abstract
{
    using System.Linq;
    using DrinkBuyer.Domain.Entities;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IProductRepository
    {
        IQueryable<Product> Products { get; }

        void SaveProduct(Product product);

        void DeleteProduct(Product product);
    }
}
