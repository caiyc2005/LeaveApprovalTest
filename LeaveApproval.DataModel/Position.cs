using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeaveApproval.DataModel
{
    public class Position
    {
        /// <summary>
        /// 唯一ID
        /// </summary>
        [Key]
        public string? Id { get; set; }
        /// <summary>
        /// 职位名称
        /// </summary>
        public string? Name { get; set; }
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsEnabled { get; set; }
        /// <summary>
        /// 创建日期和时间
        /// </summary>
        public DateTime? CreateDateTime { get; set; }
    }
}
