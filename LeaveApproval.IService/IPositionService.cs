using LeaveApproval.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeaveApproval.IService
{
    /// <summary>
    /// 职位业务服务接口
    /// </summary>
    public interface IPositionService:IBaseService<Position>
    {
        //扩展职位专用业务服务接口
    }
}
