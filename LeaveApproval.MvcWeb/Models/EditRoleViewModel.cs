using System.ComponentModel.DataAnnotations;

namespace LeaveApproval.MvcWeb.Models
{
    public class EditRoleViewModel
    {
        [Required]
        [StringLength(20)]
        [Display(Name ="角色名称")]
        public string? Name { get; set; }
    }
}
