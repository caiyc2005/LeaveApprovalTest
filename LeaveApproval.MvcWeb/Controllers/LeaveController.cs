using Microsoft.AspNetCore.Mvc;
using LeaveApproval.IService;
using LeaveApproval.MvcWeb.Models;
using LeaveApproval.ServiceContainer;
using Microsoft.AspNetCore.Mvc.Rendering;
using LeaveApproval.DataModel;
using Microsoft.AspNetCore.Identity;
using LeaveApproval.IdentityModel;
using LeaveApproval.Common;
using Microsoft.AspNetCore.Authorization;

namespace LeaveApproval.MvcWeb.Controllers
{
    [Authorize()]
    public class LeaveController : Controller
    {
        private ILeaveService _leaveService = IOCContainer.Resolve<ILeaveService>();
        private IStaffService _staffService = IOCContainer.Resolve<IStaffService>();
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public LeaveController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        /// <summary>
        /// 填写请假单-显示
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Add()
        {
            var staff = Staff;
            if(staff == null)
            {
                return RedirectToAction("Error", new {id="User"});
            }
            var leaveViewModel = new AddLeaveViewModel();
            //当前登录用户的姓名，也是请假单的申请人姓名
            leaveViewModel.StaffName = staff.Name;
            //获取审批人列表，供前端下拉选择
            leaveViewModel.ApproverList = new SelectList(_staffService.Query(s=>s.IsApprover), "Id", "Name");
            return View(leaveViewModel);
        }


        /// <summary>
        /// 获取当前登录的用户
        /// </summary>
        private Staff? Staff
        {
            get
            {
                //可能存在浏览器记住用户，但数据已清除用户的情况
                var userId = _userManager.GetUserId(User);
                var _staff =_staffService.Query(s => s.UserId == userId).FirstOrDefault();
                return _staff;
            }
        }

        /// <summary>
        /// 错误处理
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult Error(string id)
        {
            ViewData["returnUrl"] = "/Account/Login";
            switch (id)
            {
                case "User":
                    _signInManager.SignOutAsync();
                    ViewData["ErrorMessage"]= "当前用户未找到，请重新登录！";
                    //ViewBag.ErrorMessage = "当前用户未找到，请联系管理员！";
                    break;
                default:
                    ViewData["ErrorMessage"] = "发生未知错误，请联系管理员！";
                    //ViewBag.ErrorMessage = "发生未知错误，请联系管理员！";
                    break;
            }

            return View();
        }

        /// <summary>
        /// 填写请假单--提交
        /// </summary>
        /// <param name="leaveViewModel"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Add(AddLeaveViewModel leaveViewModel)
        {
            var staff = Staff;
            if (staff == null)
            {
                return RedirectToAction("Error", new { id = "User" });
            }
            if (!ModelState.IsValid)
            {
                return NotFound();
            }
            var leave = new Leaves
            {
                Id = Guid.NewGuid().ToString(),
                Title = leaveViewModel.Title,
                StartDateTime = leaveViewModel.StartDateTime,
                EndDateTime = leaveViewModel.EndDateTime,
                Description = leaveViewModel.Description,
                ApplyId = staff.Id,
                ApproverId = leaveViewModel.ApproverId,
                IsApproved = false,
                CreateDateTime = DateTime.Now
            };
            var result = _leaveService.Create(leave);
            if (result)
            {
                return RedirectToAction("MyLeave");
            }
            return View(leaveViewModel);
        }


        /// <summary>
        /// 搜索/分页获取我的请假列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> MyLeave(string c,string s,int? p,string sp = "All")
        {
            var staff = Staff;
            if (staff == null)
            {
                return RedirectToAction("Error", new { id = "User" });
            }
            //获取要返回的URL
            ViewData["returnUrl"] = string.Concat(Request.Path, Request.QueryString);
            if (s != null)
            {
                p = 1;
            }
            else
            {
                s = c;
            }
            //将当前的搜索关键字存起来
            ViewData["keyWord"] = s;
            ViewData["IsApproval"] = sp.ToLower();
            //获取所有我的请假信息
            var staffId = staff.Id;

            //var isApproval = sp.ToLower() == "approved" ? true : false;
            //var myLeaves = _leaveService.QueryInclude(i => i.ApplyId == staffId && i.IsApproved == isApproval).OrderByDescending(i => i.CreateDateTime);


            var myLeaves = _leaveService.QueryInclude(i => i.ApplyId == staffId);
            // 根据 sp 过滤
            if (sp.ToLower() == "approved")
            {
                myLeaves = myLeaves.Where(i => i.IsApproved == true);
            }
            else if (sp.ToLower() == "notapproved")
            {
                myLeaves = myLeaves.Where(i => i.IsApproved == false);
            }
            myLeaves = myLeaves.OrderByDescending(i => i.CreateDateTime);


            if (!string.IsNullOrEmpty(s))
            {
                //myLeaves = myLeaves.Where(i => i.Title!.Contains(s) || i.Description.Contains(s)).OrderByDescending(i => i.CreateDateTime);
                myLeaves = myLeaves.Where(i => i.Title!.Contains(s)).OrderByDescending(i => i.CreateDateTime);
            }
            int pageSize = 5;
            return View(await PaginatedList<Leaves>.CreateAsync(myLeaves, p ?? 1, pageSize));
        }

        /// <summary>
        /// 查看请假单
        /// </summary>
        /// <param name="id"></param>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult ViewLeave(string id,string returnUrl)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }
            var leave = _leaveService.QueryInclude(i => i.Id == id).FirstOrDefault();
            if (leave == null)
            {
                return NotFound();
            }
            var leaveViewModel = new LeaveViewModel()
            {
                Title = leave.Title,
                StaffName = leave.ApplyPerson?.Name,
                ApproverName = leave.ApprovalPerson?.Name,
                Description = leave.Description,
                StartDateTime = leave.StartDateTime,
                EndDateTime = leave.EndDateTime,
                ApprovalResults = leave.ApprovalResult,
                ApprovalComments = leave.ApprovalComments,
                IsApproved = (bool)leave.IsApproved,

            };
            return View(leaveViewModel);
        }


        [Authorize(Roles = "Approval")]
        /// <summary>
        /// 搜索/分页获取我审批的请假列表
        /// </summary>
        /// <param name="c">当前关键字</param>
        /// <param name="s">搜索关键字</param>
        /// <param name="p">页码</param>
        /// <param name="sp">审批状态</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> MyApproval(string c, string s, int? p, string sp = "All")
        {
            var staff = Staff;
            if (staff == null)
            {
                return RedirectToAction("Error", new { id = "User" });
            }
            ViewData["returnUrl"]=string.Concat(Request.Path, Request.QueryString);
            if (s != null)
            {
                p = 1;
            }
            else
            {
                s = c;
            }
            //将当前的搜索关键字存起来
            ViewData["keyword"] = s;
            ViewData["IsApproval"] = sp.ToLower();
            ////获取所有我审批的请假信息
            var staffId = staff.Id;

            var myApproval = _leaveService.QueryInclude(i => i.ApproverId == staffId);

            // 根据 sp 过滤
            if (sp.ToLower() == "approved")
            {
                myApproval = myApproval.Where(i => i.IsApproved == true);
            }
            else if (sp.ToLower() == "notapproved")
            {
                myApproval = myApproval.Where(i => i.IsApproved == false);
            }
            myApproval = myApproval.OrderByDescending(i => i.CreateDateTime);

            
            //var isApproval = sp.ToLower() == "approved" ? true : false;
            //var myApproval = _leaveService.QueryInclude(i => i.ApproverId == staffId && i.IsApproved == isApproval).OrderByDescending(i => i.CreateDateTime);
            
            
            if (!string.IsNullOrEmpty(s))
            {
                myApproval = myApproval.Where(i => i.Title!.Contains(s)).OrderByDescending(i => i.CreateDateTime);
            }
            int pageSize = 5;
            return View(await PaginatedList<Leaves>.CreateAsync(myApproval, p ?? 1, pageSize));
        }

        [Authorize(Roles = "Approval")]
        /// <summary>
        /// 执行审批--显示
        /// </summary>
        /// <param name="id"></param>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult ExecuteApproval(string id,string returnUrl)
        {
            var staff = Staff;
            if(staff == null)
            {
                return RedirectToAction("Error", new { id = "User" });
            }
            ViewData["returnUrl"] = returnUrl;
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }
            var leave = _leaveService.Find(id);
            if(leave == null)
            {
                return NotFound();
            }
            var approvalViewModel = new ApprovalViewModel()
            {
                //ApplyName = staff.Name,
                ApplyName = leave.ApplyPerson?.Name,
                Title = leave.Title,
                Description = leave.Description,
                StartDateTime = leave.StartDateTime,
                EndDateTime = leave.EndDateTime,
            };
            return View(approvalViewModel);
        }

        [Authorize(Roles = "Approval")]
        /// <summary>
        /// 执行审批--提交
        /// </summary>
        /// <param name="id"></param>
        /// <param name="approvalViewModel"></param>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult ExecuteApproval(string id,ApprovalViewModel approvalViewModel,string returnUrl)
        {
            if(!ModelState.IsValid)
            {
                //return NotFound();//// 返回当前视图，显示验证错误信息,不是直接返回找不到资源404
                return View(approvalViewModel);
            }
            var leave = _leaveService.Find(id);
            if(leave == null)
            {
                return NotFound();
            }
            leave.ApprovalResult = approvalViewModel.ApprovalResults;
            leave.ApprovalComments = approvalViewModel.ApprovalComments;
            leave.IsApproved = true;//已审批
            var r = _leaveService.Update(leave);
            if (r)
            {
                return LocalRedirect(returnUrl);
            }
            return View(approvalViewModel);
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
