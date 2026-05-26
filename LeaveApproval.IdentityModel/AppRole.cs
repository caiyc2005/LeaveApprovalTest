using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
namespace LeaveApproval.IdentityModel
{
    /// <summary>
    /// 扩展角色
    /// </summary>
    public class AppRole:IdentityRole
    {
        /// <summary>
        /// 创建日期
        /// </summary>
        public DateTime? CreatedDateTime { get; set; }
    }
}
