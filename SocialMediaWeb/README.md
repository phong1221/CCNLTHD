# SocialMediaWeb

Dự án mạng xã hội sử dụng **ASP.NET Core Web API** (MVC) cho backend và **Blazor WebAssembly** cho frontend.

---

## 📁 Cấu trúc dự án

```
SocialMediaWeb/
├── backend/                    # ASP.NET Core Web API (MVC)
│   ├── Controllers/            # Xử lý HTTP request
│   ├── Models/
│   │   ├── Entities/           # Entity classes (mapping database)
│   │   └── DTOs/               # Data Transfer Objects
│   ├── Views/                  # MVC Views (tùy chọn)
│   ├── Services/               # Business logic
│   │   └── Interfaces/         # Interface cho services
│   ├── Repositories/           # Data access layer
│   │   └── Interfaces/         # Interface cho repositories
│   ├── Data/                   # DbContext, EF Core config
│   ├── Middlewares/            # Custom middleware (auth, logging, ...)
│   ├── Helpers/                # Utility / helper classes
│   ├── Configurations/        # Cấu hình (JWT, CORS, ...)
│   ├── Migrations/             # EF Core migrations
│   ├── Program.cs              # Entry point
│   ├── appsettings.json        # Cấu hình chính
│   └── Backend.csproj          # Project file
│
├── fontend/                    # Blazor WebAssembly
│   ├── Layout/                 # MainLayout, NavMenu
│   ├── Pages/                  # Các trang Blazor (@page)
│   │   ├── Home/               # Trang chủ
│   │   ├── Auth/               # Đăng nhập, Đăng ký
│   │   ├── Profile/            # Trang cá nhân
│   │   ├── Post/               # Bài viết
│   │   └── Chat/               # Nhắn tin
│   ├── Components/             # Blazor components tái sử dụng
│   │   ├── Shared/             # Header, Footer, Sidebar
│   │   └── UI/                 # Button, Card, Modal
│   ├── Services/               # HttpClient services gọi API
│   │   └── Interfaces/         # Interface cho services
│   ├── Models/                 # DTO / View models
│   ├── wwwroot/                # Static files
│   │   ├── css/
│   │   ├── js/
│   │   └── images/
│   ├── App.razor               # Root component
│   ├── _Imports.razor          # Global using
│   ├── Program.cs              # Entry point
│   └── Frontend.csproj         # Project file
│
└── database/                   # Database scripts / backups
```

---

## 🚀 Yêu cầu

- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0) trở lên

Kiểm tra đã cài đặt chưa:

```bash
dotnet --version
```

---

## ▶️ Cách chạy Backend

```bash
cd backend
dotnet run
```

Mặc định backend sẽ chạy tại: `http://localhost:5099` (xem `Properties/launchSettings.json` để kiểm tra port).

Truy cập Swagger UI để test API: `http://localhost:5099/swagger`

---

## ▶️ Cách chạy Frontend

```bash
cd fontend
dotnet run
```

Mặc định frontend sẽ chạy tại: `http://localhost:5190` (xem `Properties/launchSettings.json` để kiểm tra port).

Mở trình duyệt và truy cập địa chỉ trên để xem giao diện Blazor.

---

## 🔧 Các lệnh hữu ích

| Lệnh | Mô tả |
|---|---|
| `dotnet build` | Build project |
| `dotnet run` | Chạy project |
| `dotnet watch run` | Chạy với hot-reload |
| `dotnet ef migrations add <Tên>` | Tạo migration mới (backend) |
| `dotnet ef database update` | Cập nhật database (backend) |

