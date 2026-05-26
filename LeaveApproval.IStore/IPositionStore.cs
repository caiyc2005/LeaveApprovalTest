using LeaveApproval.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeaveApproval.IStore
{
    /// <summary>
    /// 职位数据处理接口
    /// </summary>
    public interface IPositionStore:IBaseStore<Position>
    {
        //扩展职位专用数据处理接口
    }
}
