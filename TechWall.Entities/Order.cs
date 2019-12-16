using System;
using System.Collections.Generic;

namespace TechWall.Entities
{

    public class Order : BaseEntity

    {
        //public string CustomerID { get; set; }
        //public string CustomerName { get; set; }
        //public string CustomerEmail { get; set; }
        //public string CustomerPhone { get; set; }
        //public string CustomerCountry { get; set; }
        //public string CustomerCity { get; set; }
        //public string CustomerAddress { get; set; }
        //public string CustomerZipCode { get; set; }

        //public string OrderCode { get; set; }
        //public int PaymentMethod { get; set; }
        //public decimal TotalAmmount { get; set; }
        //public decimal Discount { get; set; }
        //public decimal DeliveryCharges { get; set; }
        //public decimal FinalAmmount { get; set; }
        //public DateTime PlacedOn { get; set; }

        //public string TransactionID { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal Total { get; set; }
        public int Tax { get; set; }
        public decimal Subtotal { get; set; }
        public int Shipping { get; set; }
        public string PayPalReference { get; set; }

        public virtual ApplicationUser User { get; set; } //An to vgalw kai pawp pernaw karfwta ta stoixeia sthn allh vash gia kathe transaction
        public virtual ICollection<OrderItem> OrderItems { get; set; }//opote na pernaw karfwta ta stoixeia tou se ena table customer mazi me epipleon stoixeia to be reconsidered

        //public virtual ICollection<OrderHistory> OrderHistory { get; set; }

        public Order()
        {
            OrderItems = new List<OrderItem>();
        }
    }
}
