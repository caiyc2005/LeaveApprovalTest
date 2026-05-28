using LeaveApproval.Common;
using LeaveApproval.DataModel;
using LeaveApproval.IdentityModel;
using LeaveApproval.IService;
using LeaveApproval.MvcWeb.Models;
using LeaveApproval.ServiceContainer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LeaveApproval.MvcWeb.Controllers
{
    [Authorize(Roles = "Admin")]
    public class DepartmentController : Controller
    {
        private IDepartmentService _departmentService = IOCContainer.Resolve<IDepartmentService>();
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Add(string returnUrl)
        {
            ViewData["returnUrl"]= returnUrl;
            return View();
        }

        /// <summary>
        /// 添加部门--提交
        /// </summary>
        /// <param name="departmentViewModel"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Add(DepartmentViewModel departmentViewModel)
        {
            if (!ModelState.IsValid)
            {
                return NotFound();
            }
            var department = new Department()
            {
                Id = Guid.NewGuid().ToString(),
                Name = departmentViewModel.Name,
                IsEnabled = departmentViewModel.IsEnabled,
                CreateDateTime = DateTime.Now,
            };
            var result = _departmentService.Create(department);
            if (result)
            {
                return RedirectToAction("List");
            }
            return View(departmentViewModel);
        }

        /// <summary>
        /// 搜索/分页获取部门列表
        /// </summary>
        /// <param name="c"></param>
        /// <param name="s"></param>
        /// <param name="p"></param>
        /// <returns></returns>
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
            var departments = _departmentService.Query(d => d.Id != null).OrderByDescending(d => d.CreateDateTime);
            if (!string.IsNullOrEmpty(s))
            {
                departments = departments.Where(d => d.Name!.Contains(s)).OrderByDescending(d=>d.CreateDateTime);
            }
            int pageSize = 5;
            return View(await PaginatedList<Department>.CreateAsync(departments, p ?? 1, pageSize));

        }



        /// <summary>
        /// 编辑部门--显示
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Edit(string? id,string returnUrl)
        {
            ViewData["returnUrl"] = returnUrl;
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }
            var department = _departmentService.Find(id);
            DepartmentViewModel? departmentViewModel = null;
            if (department != null)
            {
                departmentViewModel = new DepartmentViewModel()
                {
                    Name = department.Name,
                    IsEnabled = department.IsEnabled
                };
            }
            return View(departmentViewModel);
        }

        [HttpPost]
        public IActionResult Edit(string id,DepartmentViewModel departmentViewModel,string returnUrl)
        {
            if(!ModelState.IsValid)
            {
                return NotFound();
            }
            var department = _departmentService.Find(id);
            if(department != null)
            {
                department.Name = departmentViewModel.Name;
                department.IsEnabled = departmentViewModel.IsEnabled;
                var result = _departmentService.Update(department);
                if (result)
                {
                    return LocalRedirect(returnUrl);
                }
            }
            return View(departmentViewModel);
        }

        [HttpGet]
        public IActionResult Delete(string id,string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return NotFound();
            }
            var department = _departmentService.Find(id);
            if (department == null)
            {
                return NotFound();
            }
            _departmentService.Delete(department);
            return LocalRedirect(returnUrl) ;
        }


        public IActionResult List()
        {
            return View();
        }
    }
}
