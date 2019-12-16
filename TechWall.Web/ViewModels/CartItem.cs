using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TechWall.ViewModels
{
    public class CartItem
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }

        public string pictureUrl { get; set; }

        public decimal SubTotal
        {
            get { return this.Price * this.Quantity; }
        }
    }
}