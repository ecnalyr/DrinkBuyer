namespace DrinkBuyer.WebUI.Models
{
    using DrinkBuyer.Domain.Entities;

    public class CartIndexViewModel
    {
        public Cart Cart { get; set; }

        public string ReturnUrl { get; set; }
    }
}