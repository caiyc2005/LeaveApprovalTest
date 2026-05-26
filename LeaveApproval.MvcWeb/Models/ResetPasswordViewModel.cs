using System.ComponentModel.DataAnnotations;

namespace LeaveApproval.MvcWeb.Models
{
    public class ResetPasswordViewModel
    {
        [Display(Name ="用户名")]
        public string? UserName { get; set; }

        [Display(Name ="新密码")]
        [Required(ErrorMessage ="{0}不可为空")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

    }
}
