# 🍽️ Culinary Guide | 美食指南

[![.NET MAUI](https://img.shields.io/badge/.NET%20MAUI-9.0-512BD4?style=flat-square&logo=dotnet)](https://dotnet.microsoft.com/apps/maui)
[![Platform](https://img.shields.io/badge/Platform-Android%20%7C%20iOS%20%7C%20Windows%20%7C%20macOS-blue?style=flat-square)]()
[![License](https://img.shields.io/badge/License-MIT-green?style=flat-square)](LICENSE.txt)

A cross-platform mobile application for discovering and managing nearby restaurants, built with .NET MAUI.

一款基于 .NET MAUI 开发的跨平台美食探索应用，帮助用户发现、收藏和管理周边餐厅信息。

---

## 📖 Table of Contents | 目录

- [English](#english)
- [中文](#中文)

---

<a name="english"></a>

## 🇬🇧 English

### Overview

**Culinary Guide** is a cross-platform mobile application that helps users discover nearby restaurants, manage favorites, and explore culinary options. Built with .NET MAUI, it runs on Android, iOS, Windows, and macOS.

### Features

| Feature | Description |
|---------|-------------|
| 🔍 **Restaurant Discovery** | Browse nearby restaurants with distance, rating, and cuisine information |
| ⭐ **Smart Sorting** | Sort by distance, rating, or popularity |
| ❤️ **Favorites Management** | Save and manage your favorite restaurants |
| 📝 **Reviews & Ratings** | Read and write reviews, rate restaurants |
| 👤 **User Profile** | Customize your nickname, bio, and avatar |
| 🗺️ **Map Exploration** | View restaurants on map (simulated) |
| 📍 **GPS Location** | Real-time location with distance calculation |
| 🌐 **Multi-language** | Support for English and Chinese |

### Tech Stack

- **Framework**: .NET MAUI 9.0
- **Language**: C# 12
- **Database**: SQLite
- **Architecture**: MVVM-ready with Dependency Injection
- **Platforms**: Android, iOS, Windows, macOS

### Project Structure

```
Culinary Guide/
├── Models/              # Data models
├── Views/               # XAML pages
├── Services/            # Business logic & data access
├── Helpers/             # Utility classes
├── Resources/           # Images, fonts, styles, localization
├── Platforms/           # Platform-specific code
└── Resources/           # Localization resources
```

### Getting Started

#### Prerequisites

- [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) (v17.14+) or JetBrains Rider
- Workloads: `.NET MAUI`

#### Installation

1. **Clone the repository**
   ```bash
   git clone https://github.com/your-username/culinary-guide.git
   cd culinary-guide
   ```

2. **Restore dependencies**
   ```bash
   dotnet restore
   ```

3. **Build the project**
   ```bash
   dotnet build "Culinary Guide/Culinary Guide.csproj"
   ```

4. **Run the application**
   ```bash
   dotnet run --project "Culinary Guide/Culinary Guide.csproj"
   ```

### Screenshots

| Home Page | Restaurant Detail | Favorites |
|-----------|------------------|-----------|
| ![Home](Culinary%20Guide/Resources/Images/首页.png) | ![Detail](Culinary%20Guide/Resources/Images/详细页.png) | ![Favorites](Culinary%20Guide/Resources/Images/收藏页.png) |

| Settings | Edit Profile | Map Exploration |
|----------|--------------|-----------------|
| ![Settings](Culinary%20Guide/Resources/Images/设置页.png) | ![Edit Profile](Culinary%20Guide/Resources/Images/更改个人资料.png) | ![Map](Culinary%20Guide/Resources/Images/地图探索页.png) |

### Roadmap

- [ ] Integrate real map API (Amap/Baidu/Google Maps)
- [ ] Connect to real restaurant data API
- [ ] User authentication system
- [ ] Cloud sync for favorites
- [ ] Push notifications
- [ ] Advanced filters (price range, opening hours)

### Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

1. Fork the repository
2. Create your feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

### License

This project is licensed under the MIT License - see the [LICENSE.txt](LICENSE.txt) file for details.

---

<a name="中文"></a>

## 🇨🇳 中文

### 项目简介

**美食指南（Culinary Guide）** 是一款跨平台移动应用，帮助用户发现周边餐厅、管理收藏、探索美食选择。基于 .NET MAUI 开发，支持 Android、iOS、Windows 和 macOS 平台。

### 功能特性

| 功能 | 描述 |
|------|------|
| 🔍 **餐厅发现** | 浏览周边餐厅，查看距离、评分、菜系信息 |
| ⭐ **智能排序** | 按距离、评分、热度排序 |
| ❤️ **收藏管理** | 收藏和管理喜欢的餐厅 |
| 📝 **评论评分** | 查看和撰写评论，为餐厅评分 |
| 👤 **个人资料** | 自定义昵称、简介、头像 |
| 🗺️ **地图探索** | 在地图上查看餐厅（模拟实现） |
| 📍 **GPS定位** | 实时定位，计算餐厅距离 |
| 🌐 **多语言** | 支持中英文切换 |

### 技术栈

- **框架**: .NET MAUI 9.0
- **语言**: C# 12
- **数据库**: SQLite
- **架构**: MVVM 架构，依赖注入
- **平台**: Android、iOS、Windows、macOS

### 项目结构

```
Culinary Guide/
├── Models/              # 数据模型
├── Views/               # XAML 页面
├── Services/            # 业务逻辑与数据访问
├── Helpers/             # 工具类
├── Resources/           # 图片、字体、样式、本地化
├── Platforms/           # 平台特定代码
└── Resources/           # 本地化资源文件
```

### 快速开始

#### 环境要求

- [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) (v17.14+) 或 JetBrains Rider
- 工作负载：`.NET MAUI`

#### 安装步骤

1. **克隆仓库**
   ```bash
   git clone https://github.com/your-username/culinary-guide.git
   cd culinary-guide
   ```

2. **还原依赖**
   ```bash
   dotnet restore
   ```

3. **构建项目**
   ```bash
   dotnet build "Culinary Guide/Culinary Guide.csproj"
   ```

4. **运行应用**
   ```bash
   dotnet run --project "Culinary Guide/Culinary Guide.csproj"
   ```

### 应用截图

| 首页 | 餐厅详情 | 收藏页 |
|------|----------|--------|
| ![首页](Culinary%20Guide/Resources/Images/首页.png) | ![详细页](Culinary%20Guide/Resources/Images/详细页.png) | ![收藏页](Culinary%20Guide/Resources/Images/收藏页.png) |

| 设置页 | 编辑资料 | 地图探索 |
|--------|----------|----------|
| ![设置页](Culinary%20Guide/Resources/Images/设置页.png) | ![更改个人资料](Culinary%20Guide/Resources/Images/更改个人资料.png) | ![地图探索页](Culinary%20Guide/Resources/Images/地图探索页.png) |

### 开发路线

- [ ] 接入真实地图 API（高德/百度/Google Maps）
- [ ] 连接真实餐厅数据 API
- [ ] 用户认证系统
- [ ] 收藏云同步
- [ ] 推送通知
- [ ] 高级筛选（价格区间、营业时间）

### 贡献指南

欢迎贡献代码！请随时提交 Pull Request。

1. Fork 本仓库
2. 创建功能分支 (`git checkout -b feature/AmazingFeature`)
3. 提交更改 (`git commit -m 'Add some AmazingFeature'`)
4. 推送到分支 (`git push origin feature/AmazingFeature`)
5. 提交 Pull Request

### 许可证

本项目采用 MIT 许可证 - 详情请查看 [LICENSE.txt](LICENSE.txt) 文件。

---

## 🤝 Acknowledgments | 致谢

- [.NET MAUI](https://dotnet.microsoft.com/apps/maui) - Cross-platform framework
- [SQLite](https://www.sqlite.org/) - Embedded database
- All contributors and supporters

---

<p align="center">
  Made with ❤️ by the Culinary Guide Team
</p>