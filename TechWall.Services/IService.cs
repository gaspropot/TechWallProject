using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechWall.Entities;

namespace TechWall.Services
{
    public interface IService<T> where T:BaseEntity
    {
         T GetT<T>() where T : class;
    }
}
