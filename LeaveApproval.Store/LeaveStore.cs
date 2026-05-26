using LeaveApproval.Config;
using LeaveApproval.DataModel;
using LeaveApproval.IStore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace LeaveApproval.Store
{
    /// <summary>
    /// 请假数据的具体实现
    /// </summary>
    public class LeaveStore:BaseStore<Leaves>,ILeaveStore
    {
       /// <summary>
       /// 根据lambda表达式获取数据
       /// </summary>
       /// <param name="lambda"></param>
       /// <returns>查询后的结果实体</returns>
        public IQueryable<Leaves> QueryInclude(Expression<Func<Leaves, bool>> lambda)
        {
            return db.Set<Leaves>().Include(i => i.ApprovalPerson).Include(i => i.ApplyPerson).Where(lambda);
        }
    }
}
