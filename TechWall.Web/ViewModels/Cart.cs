using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TechWall.ViewModels
{
    public class Cart
    {
        public List<CartItem> CartItems { get; set; }
        public Cart()
        {
            CartItems = new List<CartItem>();
        }

        public decimal Total
        {
            get
            {
                return this.CartItems.Sum(x => x.Price * x.Quantity);
            }
        }
    }
}