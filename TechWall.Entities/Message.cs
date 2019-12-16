using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechWall.Entities
{
    public class Message
    {
        public int Id { get; set; }
        public string Message1 { get; set; }
        public string FromUser { get; set; }
        public string ToUser { get; set; }
        public bool IsArchived { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public virtual ApplicationUser User { get; set; }
        public virtual ApplicationUser User1 { get; set; }
    }
}
