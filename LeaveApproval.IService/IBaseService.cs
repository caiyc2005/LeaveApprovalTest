using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace LeaveApproval.IService
{
    /// <summary>
    /// 业务处理服务基接口
    /// </summary>
    public interface IBaseService<T> where T : class
    {
        bool Create(T entity);

        bool Update(T entity);

        bool Delete(T entity);

        T? Find(string id);

        IQueryable<T> Query(Expression<Func<T, bool>> lambda);
    }
}
