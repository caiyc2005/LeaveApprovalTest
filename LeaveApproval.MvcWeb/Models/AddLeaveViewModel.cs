using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace LeaveApproval.MvcWeb.Models
{
    /// <summary>
    /// 添加请假信息的视图模型
    /// </summary>
    public class AddLeaveViewModel
    {
        [Display(Name ="标题")]
        [Required(ErrorMessage ="{0}不能为空")]
        public string? Title { get; set; }


        [Display(Name ="开始日期")]
        [Required]
        [DataType(DataType.Date)]
        public DateTime StartDateTime { get; set; } = DateTime.Now;

        [Display(Name = "结束日期")]
        [Required]
        [DataType(DataType.Date)]
        public DateTime EndDateTime { get; set; } = DateTime.Now;

        [Display(Name ="请假描述")]
        [Required(ErrorMessage ="{0}不能为空")]
        public string? Description { get; set; }


        [Display(Name = "审批人")]
        public string? ApproverId { get; set; }

        [Display(Name ="请假人")]
        public string? StaffName { get; set; }

        public SelectList? ApproverList { get; set; }


    }
}
