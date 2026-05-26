using LeaveApproval.DataModel;
using LeaveApproval.IService;
using LeaveApproval.IStore;
using LeaveApproval.StoreContainer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeaveApproval.Service
{
    /// <summary>
    /// 职位业务处理具体实现
    /// </summary>
    public class PositionService:BaseService<Position>,IPositionService
    {
        //从IOC容器中解析出职位数据处理对象，并传递给基类
        public PositionService():base(IOCContainer.Resolve<IPositionStore>())
        {
        }
    }
}
