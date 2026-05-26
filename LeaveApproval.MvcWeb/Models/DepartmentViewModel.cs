using System.ComponentModel.DataAnnotations;

namespace LeaveApproval.MvcWeb.Models
{
    public class DepartmentViewModel
    {
        [Display(Name = "部门名称")]
        [Required(ErrorMessage = "{0}不可为空")]
        public string? Name { get; set; }
        [Display(Name="是否可用")]
        public bool IsEnabled { get; set; }
    }
}
