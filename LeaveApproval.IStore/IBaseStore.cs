using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace LeaveApproval.IStore
{
    /// <summary>
    /// 数据仓储公用接口
    /// </summary>
    public interface IBaseStore<T>
    {
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="entity">数据实体</param>
        /// <returns></returns>
        int Create(T entity);

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        int Update(T entity);

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        int Delete(T entity);

        /// <summary>
        /// 根据主键获取单个数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        T? Find(string id);

        /// <summary>
        /// 根据Lanbda表达式获取数据
        /// </summary>
        /// <param name="lambda">Lanbda表达式</param>
        /// <returns>查询后的结果集合</returns>
        IQueryable<T> Query(Expression<Func<T, bool>> lambda);
    }
}
