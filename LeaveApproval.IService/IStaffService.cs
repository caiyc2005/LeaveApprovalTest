using LeaveApproval.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace LeaveApproval.IService
{
    /// <summary>
    /// 员工业务处理接口
    /// </summary>
    public interface IStaffService:IBaseService<Staff>
    {
        /// <summary>
        /// 查找数据，带导航
        /// </summary>
        /// <param name="lambda"></param>
        /// <returns></returns>
        public IQueryable<Staff> QueryInclude(Expression<Func<Staff, bool>> lambda);
    }
}
