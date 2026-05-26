using System.ComponentModel.DataAnnotations;

namespace LeaveApproval.MvcWeb.Models
{
    public class ApprovalViewModel
    {
        [Display(Name ="请假人")]
        public string? ApplyName { get; set; }


        [Display(Name = "标题")]
        public string? Title { get; set; }

        [Display(Name = "开始日期")]
        [DataType(DataType.Date)]
        public DateTime StartDateTime {  get; set; } = DateTime.Now;

        [Display(Name = "结束日期")]
        [DataType(DataType.Date)]
        public DateTime EndDateTime { get; set; } = DateTime.Now;

        [Display(Name ="请假描述")]
        public string? Description { get; set; }

         [Display(Name = "审批结果")]
         public bool ApprovalResults { get; set; } = true;  // 添加默认值 = true，这样视图默认选中的是同意

        [Display(Name = "审批意见")]
        [Required(ErrorMessage ="{0}不可为空")]
        public string? ApprovalComments { get; set; }


    }
}
