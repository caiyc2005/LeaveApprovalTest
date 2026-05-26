using LeaveApproval.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace LeaveApproval.IStore
{
    /// <summary>
    /// 请假数据处理接口
    /// </summary>
    public interface ILeaveStore : IBaseStore<Leaves>
    {
        /// <summary>
        /// 根据Lambda表达式查询请假信息
        /// </summary>
        /// <param name="lambda"></param>
        /// <returns></returns>
        IQueryable<Leaves> QueryInclude(Expression<Func<Leaves, bool>> lambda);
    }
}
