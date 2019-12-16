namespace TechWall.Entities
{

    public class Picture : BaseEntity
    {

        public int ProductID { get; set; }
        public string URL { get; set; }
        public int Order { get; set; }
        public virtual Product Product { get; set; }
    }
}
