using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeaveApproval.DataModel
{
    public class Staff
    {
        /// <summary>
        /// 唯一ID
        /// </summary>
        [Key]
        public string? Id { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsEnabled { get; set; }

        /// <summary>
        /// 入职时间
        /// </summary>
        public DateTime? Created { get; set; }

        /// <summary>
        /// 年龄
        /// </summary>
        public int Age { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        public bool Sex { get; set; }

        /// <summary>
        /// 身份证号
        /// </summary>
        public string? CardId { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        public string? Address { get; set; }

        /// <summary>
        /// 部门ID
        /// </summary>
        public string? DepartmentId { get; set; }

        /// <summary>
        /// 部门导航
        /// </summary>
        public Department? Department { get; set; }

        /// <summary>
        /// 职位ID
        /// </summary>
        public string? PositionId { get; set; }

        /// <summary>
        /// 职位导航
        /// </summary>
        public Position? Position { get; set; }

        /// <summary>
        /// 关联用户表的ID值
        /// </summary>
        public string? UserId { get; set; }

        /// <summary>
        /// 是否审批人
        /// </summary>
        public bool IsApprover { get; set; }

        /// <summary>
        /// 创建日期和时间
        /// </summary>
        public DateTime? CreateDateTime { get; set; }
    }
}
