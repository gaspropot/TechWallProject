using System;

namespace TechWall.Entities
{

    public class Comment : BaseEntity
    {
        public DateTime TimeStamp { get; set; }

        public int Rating { get; set; }
        public string Text { get; set; }

        public virtual ApplicationUser User { get; set; }


    }
}
