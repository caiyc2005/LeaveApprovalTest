using LeaveApproval.DataModel;
using LeaveApproval.IService;
using LeaveApproval.IStore;
using LeaveApproval.Store;
using LeaveApproval.StoreContainer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeaveApproval.Service
{
    public class DepartmentService : BaseService<Department>, IDepartmentService
    {
        //从IOC容器中得到IDepartmentStore的实例

        public DepartmentService() : base(IOCContainer.Resolve<IDepartmentStore>()) { }

    }
}
