using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MESWebApi.InterFaces
{
    public interface IOperateData<T> where T : class, new()
    {
        int Add(T entity);
        int Add(List<T> entitys);
        bool Modify(T entity);
        bool Delete(T entity);
        bool Delete(List<T> entitys);
    }
}
