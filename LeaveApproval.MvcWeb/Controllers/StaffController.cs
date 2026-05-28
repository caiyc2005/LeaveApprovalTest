using LeaveApproval.Common;
using LeaveApproval.Config;
using LeaveApproval.DataModel;
using LeaveApproval.IdentityModel;
using LeaveApproval.IService;
using LeaveApproval.MvcWeb.Models;
using LeaveApproval.ServiceContainer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

//using LeaveApproval.StoreContainer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace LeaveApproval.MvcWeb.Controllers
{
    [Authorize(Roles = "Admin")]
    public class StaffController : Controller
    {
        private ILeaveService _leaveService = IOCContainer.Resolve<ILeaveService>();
        private IPositionService _positionService = IOCContainer.Resolve<IPositionService>();
        private IDepartmentService _departmentService = IOCContainer.Resolve<IDepartmentService>();
        private IStaffService _staffService = IOCContainer.Resolve<IStaffService>();

        private readonly UserManager<AppUser> _userManager;
        public StaffController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        /// <summary>
        /// 添加员工--显示
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Add(string returnUrl)
        {
            ViewData["returnUrl"]= returnUrl;
            var StaffViewModel = new StaffViewModel();
            StaffViewModel.DepartmentList = new SelectList(_departmentService.Query(d => d.IsEnabled).ToList(), "Id", "Name");
            StaffViewModel.PositionList = new SelectList(_positionService.Query(p => p.IsEnabled).ToList(), "Id", "Name");
            return View(StaffViewModel);
        }


        /// <summary>
        /// 添加员工--提交
        /// </summary>
        /// <param name="staffViewModel"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Add(StaffViewModel staffViewModel)
        {
            if (!ModelState.IsValid)
            {
                return NotFound();
            }
            var appUser = new AppUser()
            {
                UserName = staffViewModel.UserName,
                Email = staffViewModel.Email,
                EmailConfirmed = true,
            };
            var defaultPwd = AppSettingInfo.GetDefaultPwd;
            if (defaultPwd == null)
            {
                return NotFound();
            }
            var identityResult = await _userManager.CreateAsync(appUser, defaultPwd);
            //var identityResult = await _userManager.CreateAsync(appUser, "123@Cyc");
            //if (ModelState.IsValid)
            if(identityResult.Succeeded)
            {
                //Console.WriteLine("是否启用："+ staffViewModel.IsEnabled);
                var user = new Staff()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = staffViewModel.Name,
                    IsEnabled = staffViewModel.IsEnabled ?? true,
                    Age = staffViewModel.Age,
                    Sex = staffViewModel.Sex,
                    CardId = staffViewModel.CardId,
                    Address = staffViewModel.Address,
                    Created = staffViewModel.Created ?? DateTime.Now,

                    DepartmentId = staffViewModel.DepartmentId,
                    PositionId = staffViewModel.PositionId,
                    UserId = appUser.Id,

                    CreateDateTime = DateTime.Now,
                    //IsApprover = staffViewModel.IsApprover,
                };
                var r = _staffService.Create(user);
                if (r)
                {
                    return RedirectToAction("List");
                }
            }
            else
            {
                // 用户名重复或其他错误
                TempData["ErrorMessage"] = "用户名已被使用，请选择其他用户名";

                // 重新绑定下拉列表
                var departments = await _departmentService.Query(d => true).ToListAsync();
                var positions = await _positionService.Query(p => true).ToListAsync();
                staffViewModel.DepartmentList = new SelectList(departments, "Id", "Name", staffViewModel.DepartmentId);
                staffViewModel.PositionList = new SelectList(positions, "Id", "Name", staffViewModel.PositionId);

                //ViewData["returnUrl"] = returnUrl;
                return View(staffViewModel);
            }
            return View(staffViewModel);

        }

        /// <summary>
        /// 显示员工
        /// </summary>
        /// <param name="c"></param>
        /// <param name="s"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> List(string c, string s, int? p)
        {
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
            //将搜索关键字保存起来
            ViewData["keyWord"] = s;
            //提取所有的员工信息，延迟加载，并未执行，Admin默认管理员不会查询出来
            var staffs = _staffService.QueryInclude(s => s.Id != null && s.Name != "Admin").OrderByDescending(s => s.CreateDateTime);
            if (!string.IsNullOrEmpty(s))
            {
                staffs = staffs.Where(d => d.Name!.Contains(s)).OrderByDescending(d => d.CreateDateTime);
            }
            int pageSize = 10;
            return View(await PaginatedList<Staff>.CreateAsync(staffs, p ?? 1, pageSize));
        }


        /// <summary>
        /// 编辑员工 -- 反填显示数据
        /// </summary>
        /// <param name="id"></param>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Edit(string id, string returnUrl)
        {
            ViewData["returnUrl"] = returnUrl;
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }
            var staff = _staffService.Find(id);
            if (staff == null)
            {
                return NotFound();
            }
            var staffViewModel = new StaffViewModel()
            {
                Name = staff.Name,
                Sex = staff.Sex,
                Address = staff.Address,
                Age = staff.Age,
                CardId = staff.CardId,
                Created = staff.Created,
                DepartmentList = new SelectList(_departmentService.Query(d => d.IsEnabled).ToList(), "Id", "Name", staff.DepartmentId),
                PositionList = new SelectList(_positionService.Query(p => p.IsEnabled).ToList(), "Id", "Name", staff.PositionId),
                DepartmentId = staff.DepartmentId,
                PositionId = staff.PositionId,
                IsEnabled = staff.IsEnabled,
                //IsApprover = staff.IsApprover,

            };
            Console.WriteLine("【传出】staffViewModel=" + staffViewModel.IsApprover);
            return View(staffViewModel);
        }

        /// <summary>
        /// 编辑员工--提交
        /// </summary>
        /// <param name="id"></param>
        /// <param name="staffViewModel"></param>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Edit(string id,StaffViewModel staffViewModel,string returnUrl)
        {
            if(!ModelState.IsValid)
            {
                return NotFound();
            }
            var staff = _staffService.Find(id);
            if (staff == null)
            {
                return NotFound();
            }
            staff.Name = staffViewModel.Name;
            staff.Age = staffViewModel.Age;
            staff.Sex = staffViewModel.Sex;
            staff.CardId = staffViewModel.CardId;
            staff.Address = staffViewModel.Address;
            staff.Created = (DateTime)staffViewModel.Created;//入职时间
            staff.IsEnabled = (bool)staffViewModel.IsEnabled;
            staff.DepartmentId = staffViewModel.DepartmentId;
            staff.PositionId = staffViewModel.PositionId;
            //staff.IsApprover = staffViewModel.IsApprover;
            Console.WriteLine("【传入】staffViewModel=" + staffViewModel.IsApprover);
            var r = _staffService.Update(staff);
            if (r)
            {
                return RedirectToAction("List");
                //return RedirectToAction(returnUrl);
            }
            return View(staffViewModel);

        }


        /// <summary>
        /// 删除员工
        /// </summary>
        /// <param name="id"></param>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        public async Task<IActionResult> Delete(string id, string returnUrl)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }
            var staff = _staffService.Find(id);
            if (staff == null)
            {
                return NotFound();
            }
            //获取AppUser中的UserId
            var userId = staff.UserId;
            //查找到Identity中的用户
            if (userId == null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            //判断是否加入角色组，如果加入了角色组，则不能删除
            var role = await _userManager.GetRolesAsync(user);
            if(role.Count > 0)
            {
                return RedirectToAction("Error", new { id = "Role", returnUrl = returnUrl });
                //TempData["ErrorMessage"] = "该员工已分配角色，无法删除";
                //return RedirectToAction("List");
            }

            //判断是否参与请假审批，如果参与了请假审批，则不能删除
            var leave = _leaveService.Query(l => l.ApplyId == staff.Id || l.ApproverId == staff.Id);//.FirstOrDefault();
            if (leave == null)
            {
                return NotFound();
            }
            if (leave.Count()>0)
            {
                return RedirectToAction("Error", new { id = "Leave", returnUrl = returnUrl });
            }

            //删除员工
            _staffService.Delete(staff);
            //删除用户
            await _userManager.DeleteAsync(user);
            return LocalRedirect(returnUrl);
        }


        /// <summary>
        /// 错误处理
        /// </summary>
        /// <param name="id"></param>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(string id, string returnUrl)
        {

            ViewData["returnUrl"] = returnUrl;
            switch (id)
            {
                case "Role":
                    ViewData["ErrorMessage"] = "该员工与角色关联，不可删除！";
                    break;
                case "Leave":
                    ViewData["ErrorMessage"] = "该员工存在请假数据，不可删除！";
                    break;
                default:
                    ViewData["ErrorMessage"] = "发生未知错误！";
                    break;
            }
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            //return View();
        }


        /// <summary>
        /// 设置/取消审批人
        /// </summary>
        /// <param name="id"></param>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        public async Task<IActionResult> SetApprover(string id,string returnUrl)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }
            var staff = _staffService.Find(id);
            if (staff == null)
            {
                return NotFound();
            }
            staff.IsApprover = !staff.IsApprover;
            var r = _staffService.Update(staff);

            //--start--
            var user = await _userManager.FindByIdAsync(staff.UserId);
            if(user == null)
            {
                return NotFound();
            }
            //添加到审批角色
            if (staff.IsApprover == true)
            {
                var r2 = await _userManager.AddToRoleAsync(user, "Approval");
            }
            //移除审批角色
            else if (staff.IsApprover == false)
            {
                var r2 = await _userManager.RemoveFromRoleAsync(user,"Approval");
            }
            //---end---



            if (r)
            {
                return RedirectToAction("List");
                //return RedirectToAction(returnUrl);
            }
            return LocalRedirect(returnUrl);
        }


        


        public IActionResult Index()
        {
            return View();
        }
    }
}
