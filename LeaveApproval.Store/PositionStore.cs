using LeaveApproval.DataModel;
using LeaveApproval.IStore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeaveApproval.Store
{
    public class PositionStore:BaseStore<Position>,IPositionStore
    {
    }
}
