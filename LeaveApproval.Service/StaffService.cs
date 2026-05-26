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
    public class StaffService:BaseService<Staff>, IStaffService
    {
        private IStaffStore _staffStore = IOCContainer.Resolve<IStaffStore>();

        public StaffService() : base(IOCContainer.Resolve<IStaffStore>())
        {
        }

        public IQueryable<Staff> QueryInclude(Expression<Func<Staff, bool>> lambda)
        {
            return _staffStore.QueryInclude(lambda);
        }
    }
}
