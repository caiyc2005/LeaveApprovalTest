using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeaveApproval.Config
{
    public class AppSettingInfo
    {
        private static IConfigurationRoot? _configuration { get; set; }

        public static IConfigurationRoot? Configuration { get
            {
                if(_configuration == null)
                {
                    _configuration = new ConfigurationBuilder().Add(new JsonConfigurationSource
                    {
                        Path = "appsettings.json",
                        ReloadOnChange = true,
                    }).Build();
                }
                return _configuration;
            }
        }

        /// <summary>
        /// 获取默认密码
        /// </summary>
        public static string? GetDefaultPwd
        {
            get
            {
                return Configuration.GetSection("DefaultPwd").Value;
            }
        }
    }
}
