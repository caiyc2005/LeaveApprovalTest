using System.ComponentModel.DataAnnotations;

namespace LeaveApproval.MvcWeb.Models
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage ="{0}不可为空")]
        [Display(Name ="用户名")]
        public string? UserName { get; set; }


        [Required(ErrorMessage = "{0}不可为空")]
        [StringLength(20,ErrorMessage ="{0} 必须至少包含{2}个字符。",MinimumLength =6)]
        [DataType(DataType.Password)]
        [Display(Name = "密码")]
        public string? Password { get; set; }


        [DataType(DataType.Password)]
        [Display(Name = "确认密码")]
        [Required(ErrorMessage = "{0}不可为空")]
        [Compare("Password", ErrorMessage = "密码和确认密码不一致")]
        public string ConfirmPassword { get; set; }
    }
}
