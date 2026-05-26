# 基于企业员工请假审批管理系统



基于ASP.NET MVC 架构的企业内部请假审批管理系统



## 项目结构

```
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
```



## 技术栈

- **后端框架**: ASP.NET MVC
- **前端**: Razor + jQuery + Bootstrap
- **数据库**: SQL Server
- **架构模式**: 多层架构（表现层 → 业务层 → 数据层）



## 功能模块

基于 Controller 分析，系统包含以下模块：

| Controller           | 功能                     |
| -------------------- | ------------------------ |
| AccountController    | 用户登录、注册、身份认证 |
| LeaveController      | 请假申请、查看、撤销     |
| DepartmentController | 部门管理                 |
| PositionController   | 职位管理                 |
| RoleController       | 角色管理                 |
| StaffController      | 员工管理                 |

## 快速开始

### 环境要求

- Visual Studio 2022+
- .NET Core
- SQL Server 2012+

### 运行步骤

1. 用 Visual Studio 打开 `LeaveApproval.sln` 解决方案
2. 还原 NuGet 包（右键解决方案 → 还原 NuGet 包）
3. 修改数据库连接字符串（在 `LeaveApproval.MvcWeb` 的 `appsettings.json` 中）
4. 按 `F5` 运行项目

## 配置文件示例

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=.;Database=LeaveApprovalDB;User Id=sa;Password=123456;"
  }
}
