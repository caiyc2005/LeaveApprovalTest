using System.ComponentModel.DataAnnotations;

namespace LeaveApproval.MvcWeb.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "{0}不可为空")]
        [Display(Name = "用户名")]
        public string? UserName { get; set; }


        [Required(ErrorMessage = "{0}不可为空")]
        [DataType(DataType.Password)]
        [Display(Name = "密码")]
        public string? Password { get; set; }


        [Display(Name ="记住我")]
        public bool RememberMe { get; set; }
    }
}
