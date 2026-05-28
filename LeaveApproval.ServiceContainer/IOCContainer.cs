using Autofac;
using LeaveApproval.DataModel;
using LeaveApproval.IService;
using LeaveApproval.IStore;
using LeaveApproval.Service;
using LeaveApproval.Store;

namespace LeaveApproval.ServiceContainer
{
    /// <summary>
    /// 业务服务层IOC容器
    /// </summary>
    public class IOCContainer
    {
        public static IContainer? container = null;

        public static T Resolve<T>() where T : notnull
        {
            if (container == null)
            {
                Instance();
            }
            return container!.Resolve<T>();
        }

        public static void Instance()
        {
            var builder = new ContainerBuilder();
            //注册组件

            //builder.RegisterType<类>().As<接口>().InstancePerLifetimeScope;

            builder.RegisterType<DepartmentService>().As<IDepartmentService>().InstancePerLifetimeScope();
            builder.RegisterType<LeaveService>().As<ILeaveService>().InstancePerLifetimeScope();
            builder.RegisterType<StaffService>().As<IStaffService>().InstancePerLifetimeScope();
            builder.RegisterType<PositionService>().As<IPositionService>().InstancePerLifetimeScope();

            container = builder.Build();
        }
    }
}
