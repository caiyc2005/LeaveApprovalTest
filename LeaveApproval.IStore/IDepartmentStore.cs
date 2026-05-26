using LeaveApproval.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeaveApproval.IStore
{
    /// <summary>
    /// 部门数据处理接口
    /// </summary>
    public interface IDepartmentStore:IBaseStore<Department>
    {
        //扩展部门专用数据处理接口
    }
}
