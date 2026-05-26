using System.ComponentModel.DataAnnotations;

namespace LeaveApproval.MvcWeb.Models
{
    public class AddRoleViewModel
    {
        [Required(ErrorMessage ="{0}不可为空")]
        [StringLength(20)]
        [Display(Name ="角色名称")]
        public string? Name { get; set; }
    }
}
