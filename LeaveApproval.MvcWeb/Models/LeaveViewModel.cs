using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace LeaveApproval.MvcWeb.Models
{
    /// <summary>
    /// 请假单查看视图模型
    /// </summary>
    public class LeaveViewModel
    {
        [Display(Name = "标题")]
        [Required(ErrorMessage = "{0}不能为空")]
        public string? Title { get; set; }


        [Display(Name = "开始日期")]
        [Required]
        [DataType(DataType.Date)]
        public DateTime StartDateTime { get; set; }

        [Display(Name = "结束日期")]
        [Required]
        [DataType(DataType.Date)]
        public DateTime EndDateTime { get; set; }

        [Display(Name = "请假描述")]
        [Required(ErrorMessage = "{0}不能为空")]
        public string? Description { get; set; }


        [Display(Name = "审批人")]
        public string? ApproverName { get; set; }

        [Display(Name = "请假人")]
        public string? StaffName { get; set; }

        [Display(Name ="审批结果")]
        public bool ApprovalResults { get; set; }

        [Display(Name = "审批意见")]
        public string? ApprovalComments { get; set; }

        [Display(Name = "是否审批")]
        public bool IsApproved { get; set; }

    }
}
