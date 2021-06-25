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
        int Modify(T entity);
        int Delete(T entity);
        int Delete(List<T> entitys);
    }
}
