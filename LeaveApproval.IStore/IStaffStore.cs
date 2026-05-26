using LeaveApproval.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace LeaveApproval.IStore
{
    public interface IStaffStore:IBaseStore<Staff>
    {
        public IQueryable<Staff> QueryInclude(Expression<Func<Staff, bool>> lambda);
    }
}
