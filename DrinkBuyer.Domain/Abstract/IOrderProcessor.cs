// -----------------------------------------------------------------------
// <copyright file="IOrderProcessor.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace DrinkBuyer.Domain.Abstract
{
    using DrinkBuyer.Domain.Entities;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IOrderProcessor
    {
        void ProcessOrder(Cart cart, ShippingDetails shippingDetails);
    }
}
