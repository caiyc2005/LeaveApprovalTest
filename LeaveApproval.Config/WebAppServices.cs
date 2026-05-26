
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
            service.AddIdentity<AppUser, AppRole>(options=>
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
        private static void InitdalDbAndData(IServiceCollection service)
        {
            //得到生存服务的提供程序
            var provider = service.BuildServiceProvider();

            //获取AppDbContext实例
            dbContext = provider.GetRequiredService<AppDbContext>();

            //生成数据库，只生成一次
            dbContext.Database.EnsureCreated();

        }
    }
}
