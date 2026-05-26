using LeaveApproval.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeaveApproval.IService
{
    /// <summary>
    /// 部门业务处理接口
    /// </summary>
    public interface IDepartmentService:IBaseService<Department>
    {
        //扩展部门专用的业务处理接口
    }
}
