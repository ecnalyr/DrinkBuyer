// -----------------------------------------------------------------------
// <copyright file="EFDbContext.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

using System.Data.Entity;
namespace DrinkBuyer.Domain.Concrete
{
    using DrinkBuyer.Domain.Entities;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class EFDbContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
    }
}
