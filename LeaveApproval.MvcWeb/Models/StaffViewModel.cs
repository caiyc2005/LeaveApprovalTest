using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace LeaveApproval.MvcWeb.Models
{
    /// <summary>
    /// 员工视图模型
    /// </summary>
    public class StaffViewModel
    {
        [Display(Name = "登录名")]
        public string? UserName { get; set; }

        [Display(Name = "员工姓名")]
        [Required(ErrorMessage = "{0}不可为空")]
        public string? Name { get; set; }

        [Display(Name ="邮箱")]
        //[EmailAddress(ErrorMessage = "{0}格式不正确")]
        public string? Email { get; set; }

        [Display(Name ="是否在职")]
        public bool? IsEnabled { get; set; }

        [Display(Name ="是否审批人")]
        public bool IsApprover { get; set; }


        [Display(Name ="入职日期")]
        [DataType(DataType.Date)]
        public DateTime? Created { get; set; } = DateTime.Now;

        [Display(Name = "年龄")]
        public int Age { get; set; }

        [Display(Name ="性别")]
        public bool Sex { get; set; }

        [Display(Name ="身份证号")]
        [Required(ErrorMessage ="{0}不可为空")]
        [MaxLength(18)]
        public string? CardId { get; set; }


        [Display(Name ="居住地址")]
        public string? Address { get; set; }

        [Display(Name ="部门")]
        public string? DepartmentId { get; set; }
        public SelectList? DepartmentList {  get; set; }


        [Display(Name ="职位")]
        public string? PositionId { get; set; }
        public SelectList? PositionList { get; set; }
    }
}
