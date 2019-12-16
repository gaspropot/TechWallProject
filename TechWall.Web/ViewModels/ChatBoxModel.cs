using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TechWall.Entities;

namespace TechWall.ViewModels
{
    public class ChatBoxModel
    {
        public UserDTO ToUser { get; set; }
        public List<MessageDTO> Messages { get; set; }
    }
}