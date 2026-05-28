
using LeaveApproval.DataModel;
using LeaveApproval.IdentityModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeaveApproval.Config
{
    /// <summary>
    /// Web应用程序服务配置
    /// </summary>
    public class WebAppServices
    {
        public static AppDbContext? dbContext { get; set; }
        /// <summary>
        /// 注册Identity相关的服务
        /// </summary>
        /// <param name="service"></param>
        public static void IdentityAppService(IServiceCollection service)
        {
            ///在appsettings.json获取配置
            var configuration = new ConfigurationBuilder()
                .Add(new JsonConfigurationSource { Path = "appsettings.json", ReloadOnChange = true })
                .Build();

            //注册AppDbContext
            service.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("YidoConnection")),
                contextLifetime: ServiceLifetime.Scoped);

            //注册Identity服务，并指定用户和角色对象
            service.AddIdentity<AppUser, AppRole>(options =>
                options.SignIn.RequireConfirmedAccount = true)
                    .AddEntityFrameworkStores<AppDbContext>()
                    .AddDefaultTokenProviders();

            //生成数据库和表结构。
            InitdalDbAndData(service);
        }

        /// <summary>
        /// 生成数据库并初始化数据
        /// </summary>
        /// <param name="service"></param>
        private static async Task InitdalDbAndData(IServiceCollection service)
        {
            //得到生存服务的提供程序
            var provider = service.BuildServiceProvider();

            //获取AppDbContext实例
            dbContext = provider.GetRequiredService<AppDbContext>();

            //生成数据库，只生成一次
            var r = dbContext.Database.EnsureCreated();

            if (r)
            {
                //获取用户和角色管理器实例
                var userManager = provider.GetRequiredService<UserManager<AppUser>>();
                var roleManager = provider.GetRequiredService<RoleManager<AppRole>>();

                //生成超级管理员
                var adminUser = new AppUser()
                {
                    Id = Guid.NewGuid().ToString(),
                    UserName = "admin",
                    Email = "admin@cyc.com",
                    EmailConfirmed = true
                };
                await userManager.CreateAsync(adminUser, AppSettingInfo.GetDefaultPwd);

                //建立用户与员工的关系
                await dbContext.Staffs.AddAsync(new Staff()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = adminUser.UserName,
                    UserId = adminUser.Id,
                    IsApprover = true,
                    IsEnabled = true,
                });

                //生成管理员组和审批组
                var adminRole = new AppRole()
                {
                    Name = "Admin",
                    CreatedDateTime = DateTime.Now,
                };
                await roleManager.CreateAsync(adminRole);

                var approverRole = new AppRole()
                {
                    Name = "Approval",
                    CreatedDateTime = DateTime.Now,
                };
                await roleManager.CreateAsync(approverRole);

                //将管理员用户添加到管理员组
                await userManager.AddToRolesAsync(adminUser, new string[] { adminRole.Name, approverRole.Name });
                userManager.Dispose();
                roleManager.Dispose();
            }
        }
    }
}
