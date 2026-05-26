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
    public class StaffStore:BaseStore<Staff>,IStaffStore
    {
        /// <summary>
        /// 查数据，带导航
        /// </summary>
        /// <param name="lambda"></param>
        /// <returns></returns>
        public IQueryable<Staff> QueryInclude(Expression<Func<Staff,bool>> lambda)
        {
            return db.Set<Staff>().Include(s => s.Department).Include(s => s.Position).Where(lambda);
        }
    }
}
