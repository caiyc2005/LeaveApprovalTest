using LeaveApproval.DataModel;
using LeaveApproval.IdentityModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeaveApproval.Config
{
    public class AppDbContext:IdentityDbContext<AppUser, AppRole, string>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        /// <summary>
        /// 部门表
        /// </summary>
        public DbSet<Department> Departments { get; set; }

        /// <summary>
        /// 职位表
        /// </summary>
        public DbSet<Position> Positions { get; set; }

        /// <summary>
        /// 员工表
        /// </summary>
        public DbSet<Staff> Staffs { get; set; }

        /// <summary>
        /// 请假表
        /// </summary>
        public DbSet<Leaves> Leaves { get; set; }
    }
}
