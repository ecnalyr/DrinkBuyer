namespace DrinkBuyer.WebUI.Models
{
    using System;

    public class PagingInfo
    {
        #region Public Properties

        public int CurrentPage { get; set; }

        public int ItemsPerPage { get; set; }

        public int TotalItems { get; set; }

        public int TotalPages
        {
            get
            {
                return (int)Math.Ceiling((decimal)this.TotalItems / this.ItemsPerPage);
            }
        }

        #endregion
    }
}