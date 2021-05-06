using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MESWebApi.Models;
namespace MESWebApi.InterFaces
{
    public interface IDBOper<T> where T : class, new()
    {
        T Add(T entity);
        int Modify(T entity);
        bool Delete(int id);
        int Delete(List<int> ids);
        T Find(int id);
        IEnumerable<T> Find(sys_page parm,out int resultcount);
    }
}
