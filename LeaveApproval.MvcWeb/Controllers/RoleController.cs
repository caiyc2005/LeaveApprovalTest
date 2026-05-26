using LeaveApproval.IdentityModel;
using LeaveApproval.MvcWeb.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using LeaveApproval.Common;

namespace LeaveApproval.MvcWeb.Controllers
{
    public class RoleController : Controller
    {


        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;
        public RoleController(RoleManager<AppRole> roleManager, UserManager<AppUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Add(string returnUrl)
        {
            ViewData["returnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(AddRoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _roleManager.CreateAsync(new AppRole() { Name = model.Name ,CreatedDateTime=DateTime.Now});
                if (result.Succeeded)
                {
                    return RedirectToAction("List");
                }
            }
            return View();
        }


        [HttpGet]
        public async Task<IActionResult> List(string c,string s, int? p)
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

            //将当前的搜索关键字存起来
            ViewData["keyWord"] = s;

            //获取所有的角色信息
            var roles = _roleManager.Roles.OrderByDescending(r => r.CreatedDateTime);
            if (!string.IsNullOrEmpty(s))
            {
                roles = roles.Where(d => d.Name!.Contains(s)).OrderByDescending(r=>r.CreatedDateTime);
            }
            int pageSize = 5;
            return View(await PaginatedList<AppRole>.CreateAsync(roles,p ?? 1,pageSize));
        }

        /// <summary>
        /// 编辑角色--显示
        /// </summary>
        /// <param name="id"></param>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Edit(string id,string returnUrl)
        {
            ViewData["returnUrl"]=returnUrl;
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }
            var role = await _roleManager.FindByIdAsync(id);
            if(role == null)
            {
                return NotFound();
            }
            var model = new EditRoleViewModel()
            {
                Name = role.Name
            };
            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id,EditRoleViewModel model,string returnUrl)
        {
            if(ModelState.IsValid && !string.IsNullOrEmpty(id))
            {
                var role = await _roleManager.FindByIdAsync (id);
                if(role == null)
                {
                    return NotFound();
                }
                role.Name = model.Name;
                var result = await _roleManager.UpdateAsync(role);
                if (result.Succeeded)
                {
                    return LocalRedirect(returnUrl);
                }
            }
            return View(model);
        }

        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="id"></param>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Delete(string id, string returnUrl)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }
            //获取要删除的角色实体
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                return NotFound();
            }
            //判断角色下是否存在用户，存在用户则不可删除
            if (_userManager.GetUsersInRoleAsync(role.Name).Result.Count > 0)
            {
                return NotFound();
            }
            await _roleManager.DeleteAsync(role);
            return LocalRedirect(returnUrl);
        }

        /// <summary>
        /// 显示未加入角色的用户列表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> UserToRole(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                return NotFound();
            }
            ViewBag.Rolename = role.Name;
            ViewBag.RoleId = role.Id;
            //存储未加入注定角色的用户
            var userList = new List<AppUser>();
            //获取所有的用户
            var users = _userManager.Users.ToList();
            foreach(var u in users)
            {
                //判断用户是否加入角色
                if(!await _userManager.IsInRoleAsync(u, role.Name!))
                {
                    userList.Add(u);
                }
            }
            return View(userList);
        }


        /// <summary>
        /// 添加用户到角色
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="roleName"></param>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public async Task<IActionResult> AddToRole(string userId,string roleName,string roleId)
        {
            if(!string.IsNullOrEmpty(userId) && !string.IsNullOrWhiteSpace(roleName))
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    return NotFound();
                }
                await _userManager.AddToRoleAsync(user, roleName);
            }
            return RedirectToAction("UserToRole", new {id = roleId});
        }

        /// <summary>
        /// 查看角色中的用户
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public async Task<IActionResult> ShowRoleUser(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return BadRequest();
            }
            ViewBag.RoleName = name;
            //获取角色下所有关联的用户
            var users = await _userManager.GetUsersInRoleAsync(name);
            return View(users);
        }

        /// <summary>
        /// 移除用户
        /// </summary>
        /// <param name="roleName"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> RemoveRoleUser(string roleName, string userId)
        {
            if (string.IsNullOrWhiteSpace(roleName) && string.IsNullOrWhiteSpace(userId))
            {
                return BadRequest();
            }
            //
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }
            var result = await _userManager.RemoveFromRoleAsync(user, roleName);
            if (!result.Succeeded)
            {
                return View("Error");
            }
            return RedirectToAction("ShowRoleUser", new { name = roleName });
        }

    }
}
