using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TechWall.ViewModels
{
    public class MessageDTO
    {
        public int ID { get; set; }
        public string Message { get; set; }
        public string Class { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public bool isArchived { get; set; }
    }
}