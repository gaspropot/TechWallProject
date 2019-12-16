using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechWall.Entities
{
    public class UserConnection
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public Nullable<System.Guid> ConnectionId { get; set; }

        public virtual ApplicationUser User { get; set; }
    }
}
