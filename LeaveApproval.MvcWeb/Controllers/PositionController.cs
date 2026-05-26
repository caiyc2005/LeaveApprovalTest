using LeaveApproval.Common;
using LeaveApproval.DataModel;
using LeaveApproval.IService;
using LeaveApproval.MvcWeb.Models;
using LeaveApproval.ServiceContainer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;

namespace LeaveApproval.MvcWeb.Controllers
{
    [Authorize(Roles = "Admin")]
    public class PositionController : Controller
    {
        private IPositionService _positionService = IOCContainer.Resolve<IPositionService>();

        private IStaffService _staffService = IOCContainer.Resolve<IStaffService>();

        [HttpGet]
        public IActionResult Add(string returnUrl)
        {
            ViewData["returnUrl"] = returnUrl;
            return View();
        }

        /// <summary>
        /// 添加职位--提交
        /// </summary>
        /// <param name="positionViewModel"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Add(PositionViewModel positionViewModel)
        {
            if (!ModelState.IsValid)
            {
                return NotFound();
            }
            var position = new Position()
            {
                Id = Guid.NewGuid().ToString(),
                Name = positionViewModel.Name,
                IsEnabled = positionViewModel.IsEnabled,
                CreateDateTime = DateTime.Now,
            };
            var result = _positionService.Create(position);
            if (result)
            {
                return RedirectToAction("List");
            }
            return View(positionViewModel);
        }

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

            //将当前的搜索关键字存起来
            ViewData["keyWord"] = s;

            //获取所有的职位信息
            var positions = _positionService.Query(p => p.Id != null).OrderByDescending(p => p.CreateDateTime);
            if(!string.IsNullOrEmpty(s))
            {
                positions = positions.Where(p => p.Name!.Contains(s)).OrderByDescending(p=>p.CreateDateTime);
            }
            int pageSize = 8;
            return View(await PaginatedList<Position>.CreateAsync(positions, p ?? 1, pageSize));

        }


        /// <summary>
        /// 编辑职位--显示
        /// </summary>
        /// <param name="id"></param>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Edit(string id,string returnUrl)
        {
            ViewData["returnUrl"] = returnUrl;
            //如果ID的值为空
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }
            var position = _positionService.Find(id);
            //如果根据ID找不到职位实体
            if (position == null)
            {
                return NotFound();
            }
            var positionViewModel = new PositionViewModel()
            {
                Name = position.Name,
                IsEnabled = position.IsEnabled
            };
            return View(positionViewModel);
        }

        /// <summary>
        /// 编辑职位--提交
        /// </summary>
        /// <param name="id"></param>
        /// <param name="positionViewModel"></param>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Edit(string id,PositionViewModel positionViewModel,string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return NotFound();
            }
            var position = _positionService.Find(id);
            if(position == null)
            {
                return NotFound();
            }
            position.Name = positionViewModel.Name;
            position.IsEnabled = positionViewModel.IsEnabled;

            var r = _positionService.Update(position);
            if (r)
            {
                return LocalRedirect(returnUrl);
            }

            return View(positionViewModel);
        }

        /// <summary>
        /// 删除职位
        /// </summary>
        /// <param name="id"></param>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        public IActionResult Delete(string id, string returnUrl)
        {
            if (string.IsNullOrEmpty(id))
            {
                return View();
            }
            var position = _positionService.Find(id);
            if( position == null)
            {
                return NotFound();
            }

            var staff = _staffService.Query(s => s.PositionId == position.Id);
            if (staff.Count() > 0) //有员工数据关联，不可删除
            {
                return RedirectToAction("Error", new { id = "Delete", returnUrl = returnUrl });
            }

            _positionService.Delete(position);

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
                case "Delete":
                    ViewData["ErrorMessage"] = "无法删除职位，请先删除关联的员工数据！";
                    break;
                default:
                    ViewData["ErrorMessage"] = "发生未知错误！";
                    break;
            }
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            //return View();
        }


        #region 导出Excel功能

        /// <summary>
        /// 导出职位信息到Excel
        /// </summary>
        /// <param name="keyWord">搜索关键字（可选，用于导出筛选后的数据）</param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult ExportToExcel(string keyWord = null)
        {
            try
            {
                // 设置EPPlus的许可证上下文（EPPlus 5+需要）
                //ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
                // 或者明确设置为非商业用途
                // EPPlus 8.0+ 的新写法
                ExcelPackage.License.SetNonCommercialPersonal("cyc");

                // 获取要导出的数据（与List页面逻辑一致）
                var positionsQuery = _positionService.Query(p => p.Id != null);

                if (!string.IsNullOrEmpty(keyWord))
                {
                    positionsQuery = positionsQuery.Where(p => p.Name != null && p.Name.Contains(keyWord));
                }

                var positions = positionsQuery.OrderByDescending(p => p.CreateDateTime).ToList();

                if (positions == null || !positions.Any())
                {
                    TempData["Error"] = "没有可导出的数据";
                    return RedirectToAction("List");
                }

                // 生成Excel文件
                byte[] fileContents = GenerateExcelFile(positions);

                // 设置文件名（包含时间戳）
                string fileName = $"职位信息_{DateTime.Now:yyyyMMddHHmmss}.xlsx";

                // 返回文件
                return File(fileContents, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"导出失败：{ex.Message}";
                return RedirectToAction("List");
            }
        }

        /// <summary>
        /// 导出选中的职位信息
        /// </summary>
        /// <param name="selectedIds">选中的职位ID列表</param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult ExportSelected(string[] selectedIds)
        {
            try
            {
                //ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
                ExcelPackage.License.SetNonCommercialPersonal("cyc");

                if (selectedIds == null || !selectedIds.Any())
                {
                    TempData["Error"] = "请至少选择一个职位";
                    return RedirectToAction("List");
                }

                var positions = _positionService.Query(p => selectedIds.Contains(p.Id)).ToList();

                if (!positions.Any())
                {
                    TempData["Error"] = "没有找到选中的数据";
                    return RedirectToAction("List");
                }

                byte[] fileContents = GenerateExcelFile(positions);
                string fileName = $"职位信息_导出_{DateTime.Now:yyyyMMddHHmmss}.xlsx";

                return File(fileContents, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"导出失败：{ex.Message}";
                return RedirectToAction("List");
            }
        }

        /// <summary>
        /// 生成Excel文件
        /// </summary>
        private byte[] GenerateExcelFile(List<Position> positions)
        {
            using (var package = new ExcelPackage())
            {
                // 添加工作表
                var worksheet = package.Workbook.Worksheets.Add("职位信息");

                // 设置表头样式
                using (var range = worksheet.Cells[1, 1, 1, 5])
                {
                    range.Style.Font.Bold = true;
                    range.Style.Font.Size = 11;
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Fill.BackgroundColor.SetColor(Color.LightBlue);
                    range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    range.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                }

                // 设置表头
                worksheet.Cells[1, 1].Value = "序号";
                worksheet.Cells[1, 2].Value = "职位ID";
                worksheet.Cells[1, 3].Value = "职位名称";
                worksheet.Cells[1, 4].Value = "状态";
                worksheet.Cells[1, 5].Value = "创建时间";

                // 设置列宽
                worksheet.Column(1).Width = 8;
                worksheet.Column(2).Width = 35;
                worksheet.Column(3).Width = 25;
                worksheet.Column(4).Width = 12;
                worksheet.Column(5).Width = 20;

                // 填充数据
                int rowIndex = 2;
                int serialNumber = 1;

                foreach (var position in positions)
                {
                    // 序号
                    worksheet.Cells[rowIndex, 1].Value = serialNumber++;
                    worksheet.Cells[rowIndex, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    // 职位ID
                    worksheet.Cells[rowIndex, 2].Value = position.Id;

                    // 职位名称
                    worksheet.Cells[rowIndex, 3].Value = position.Name;

                    // 状态（bool类型转中文）
                    worksheet.Cells[rowIndex, 4].Value = position.IsEnabled ? "启用" : "禁用";
                    worksheet.Cells[rowIndex, 4].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    // 创建时间
                    worksheet.Cells[rowIndex, 5].Value = position.CreateDateTime?.ToString("yyyy-MM-dd HH:mm:ss") ?? "";

                    // 设置边框
                    using (var range = worksheet.Cells[rowIndex, 1, rowIndex, 5])
                    {
                        range.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        range.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    }

                    // 禁用状态的行设置灰色背景
                    if (!position.IsEnabled)
                    {
                        using (var range = worksheet.Cells[rowIndex, 1, rowIndex, 5])
                        {
                            range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            range.Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                        }
                    }

                    rowIndex++;
                }

                // 冻结首行
                worksheet.View.FreezePanes(2, 1);

                return package.GetAsByteArray();
            }
        }

        #endregion

        public IActionResult Index()
        {
            return View();
        }
    }
}
