using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using LeaveApproval.IdentityModel;
using Microsoft.AspNetCore.Authorization;
using LeaveApproval.MvcWeb.Models;

namespace LeaveApproval.MvcWeb.Controllers
{
    [Authorize()]
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        public AccountController(UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register(string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterViewModel model,string? returnUrl = null)
        {
            //返回url
            returnUrl = returnUrl ?? Url.Content("~/");
            ViewData["ResultUrl"] = returnUrl;

            //服务器端验证
            if (ModelState.IsValid)
            {
                //构建AppUser实体对象
                var user = new AppUser { UserName = model.UserName, EmailConfirmed = true };

                //创建用户
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    //注册后，也就是已登录
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return LocalRedirect(returnUrl);
                }
            }
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model,string? returnUrl = null)
        {
            //返回url
            returnUrl = returnUrl ?? Url.Content("~/");
            ViewData["ResultUrl"] = returnUrl;

            //服务器端验证
            if (ModelState.IsValid)
            {
                //登录信息匹配
                var result = await _signInManager.PasswordSignInAsync(model.UserName!, model.Password!,
                    model.RememberMe, lockoutOnFailure: false);
                
                //登录成功
                if (result.Succeeded)
                {
                    //转到返回页面
                    return LocalRedirect(returnUrl);
                }
                //是否锁定，True表示已锁定
                if (result.IsLockedOut)
                {
                    return RedirectToAction("Lockout");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "尝试登录无效");
                    return View(model);
                }
            }
            return View(model);
        }

        /// <summary>
        /// 账户锁定
        /// </summary>
        /// <returns></returns>
        public IActionResult Lockout()
        {
            return View();
        }


        /// <summary>
        /// 重置密码--显示
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult ResetPassword(string returnUrl)
        {
            ViewData["returnUrl"] = returnUrl;
            return View();
        }

        /// <summary>
        /// 重置密码--提交
        /// </summary>
        /// <param name="model"></param>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model, string? returnUrl)
        {
            ViewData["returnUrl"] = returnUrl;
            if (!ModelState.IsValid)
            {
                return NotFound();
            }
            var user = await _userManager.FindByIdAsync(_userManager.GetUserId(HttpContext.User)!);
            if (user == null)
            {
                return NotFound();
            }
            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            var r = await _userManager.ResetPasswordAsync(user, code, model.Password);
            if (r.Succeeded)
            {
                ViewData["Message"] = "新密码重置成功。";
            }
            return View(model);
        }


        /// <summary>
        /// 注销用户
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return LocalRedirect("~/");
        }
    }
}
