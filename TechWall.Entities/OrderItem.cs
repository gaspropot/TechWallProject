namespace TechWall.Entities
{

    public class OrderItem : BaseEntity
    {
        //public int OrderID { get; set; }
        //public int ProductID { get; set; }
        //public decimal ItemPrice { get; set; }
        //public int Quantity { get; set; }
        //public virtual Product Product { get; set; }

        public int OrderId { get; set; }
        public Order Order { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
