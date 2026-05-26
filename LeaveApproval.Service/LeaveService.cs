using LeaveApproval.DataModel;
using LeaveApproval.IService;
using LeaveApproval.IStore;
using LeaveApproval.StoreContainer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace LeaveApproval.Service
{
    public class LeaveService:BaseService<Leaves>, ILeaveService
    {
        /// <summary>
        /// 请假业务处理具体实现
        /// </summary>
        private ILeaveStore _leaveStore = IOCContainer.Resolve<ILeaveStore>();
        public LeaveService() : base(IOCContainer.Resolve<ILeaveStore>())
        {
        }
        public IQueryable<Leaves> QueryInclude(Expression<Func<Leaves, bool>> lambda)
        {
            return _leaveStore.QueryInclude(lambda);
        }
    }
}
