using LeaveApproval.DataModel;
using LeaveApproval.IStore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeaveApproval.Store
{
    public class DepartmentStore:BaseStore<Department>,IDepartmentStore
    {
        //扩展部门专用数据处理具体实现

    }
}
