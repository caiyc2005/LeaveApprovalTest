using LeaveApproval.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace LeaveApproval.IService
{
    public interface ILeaveService:IBaseService<Leaves>
    {
        /// <summary>
        /// 请假业务处理接口
        /// </summary>
        /// <param name="lambda"></param>
        /// <returns></returns>
        public IQueryable<Leaves> QueryInclude(Expression<Func<Leaves, bool>> lambda);
    }
}
