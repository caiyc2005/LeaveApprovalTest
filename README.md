# 基于企业员工请假审批管理系统



基于ASP.NET MVC 架构的企业内部请假审批管理系统



## 项目结构

LeaveApproval/
├── Component/ # 通用组件层
│ ├── LeaveApproval.Common # 公共工具类
│ └── LeaveApproval.Config # 配置管理
├── DataModels/ # 数据模型层
│ ├── LeaveApproval.DataModel # 业务数据模型
│ └── LeaveApproval.IdentityModel # 身份认证模型
├── Services/ # 服务层（业务逻辑）
│ ├── LeaveApproval.Iservice # 服务接口定义
│ ├── LeaveApproval.Service # 服务实现
│ └── LeaveApproval.ServiceContainer # 服务容器配置
├── Stores/ # 数据存储层
│ ├── LeaveApproval.IStore # 存储接口定义
│ ├── LeaveApproval.Store # 存储实现
│ └── LeaveApproval.StoreContainer # 存储容器配置
└── UI/ # 表现层
└── LeaveApproval.MvcWeb # MVC Web 应用程序
├── Controllers/ # 控制器：处理HTTP请求
├── Models/ # 视图模型：页面数据交互
├── Views/ # 视图：Razor页面
└── wwwroot/ # 静态资源：CSS、JS、图片

