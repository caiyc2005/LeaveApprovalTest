using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeaveApproval.DataModel
{
    /// <summary>
    /// 请假
    /// </summary>
    public class Leaves
    {
        /// <summary>
        /// 唯一ID
        /// </summary>
        [Key]
        public string? Id { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        public string? Title { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime StartDateTime {  get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime EndDateTime { get; set; }

        /// <summary>
        /// 请假原因
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// 申请人
        /// </summary>
        [ForeignKey(nameof(ApplyPerson))]
        public string? ApplyId { get; set; }
        public Staff? ApplyPerson { get; set; }

        /// <summary>
        /// 审批人
        /// </summary>
        [ForeignKey(nameof(ApprovalPerson))]
        public string? ApproverId { get; set; }
        public Staff? ApprovalPerson { get; set; }

        /// <summary>
        /// 审批结果
        /// </summary>
        public bool ApprovalResult { get; set; }

        /// <summary>
        /// 审批意见
        /// </summary>
        public string? ApprovalComments { get; set; }

        /// <summary>
        /// 是否已审批
        /// </summary>
        public bool? IsApproved { get; set; }

        /// <summary>
        /// 创建日期和时间
        /// </summary>
        public DateTime? CreateDateTime { get; set; }

    }
}
