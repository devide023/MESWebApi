using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MESWebApi.Models;
namespace MESWebApi.InterFaces
{
    /// <summary>
    /// 组合查询
    /// </summary>
    /// <typeparam name="T">返回类型</typeparam>
    /// <typeparam name="Q">查询参数</typeparam>
    interface IComposeQuery<T,Q> where Q : sys_page, new()
    {
        IEnumerable<T> Search(Q parm, out int resultcount);
    }
}
