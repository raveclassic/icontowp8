using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iconto.PCL.Stores.Data
{
    public interface IRepository<T>
    {
        T GetAll(byte limit, long offset);
        T Create(T obj);
        void Remove(T obj);
        void Update(T obj);
    }
}
