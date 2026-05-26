using System.ComponentModel.DataAnnotations;

namespace LeaveApproval.MvcWeb.Models
{
    /// <summary>
    /// 职位视图模型
    /// </summary>
    public class PositionViewModel
    {
        [Display(Name = "职位名称")]
        [Required(ErrorMessage = "{0}不能为空")]

        public string? Name { get; set; }

        [Display(Name = "是否可用")]
        public bool IsEnabled { get; set; }
    }
}
