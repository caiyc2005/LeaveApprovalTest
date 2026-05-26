using Autofac;
using LeaveApproval.Store;
using LeaveApproval.IStore;

namespace LeaveApproval.StoreContainer
{
    /// <summary>
    /// 数据仓储层IOC容器
    /// </summary>
    public class IOCContainer
    {
        public static IContainer? container = null;

        public static T Resolve<T>() where T : notnull
        {
            if(container == null)
            {
                Instance();
            }
            return container!.Resolve<T>();

        }

        /// <summary>
        /// 注册组件
        /// </summary>
        private static void Instance()
        {
            var builder = new ContainerBuilder();
            //注册组件
            builder.RegisterType<DepartmentStore>().As<IDepartmentStore>().InstancePerLifetimeScope();

            container = builder.Build();
        }
    }
}
