# BTL Backend

## Mô tả dự án

Đây là ứng dụng backend cho hệ thống quản lý nông nghiệp BTL (Backend for Farm Management), được xây dựng bằng ASP.NET Core. Ứng dụng cung cấp các API để quản lý nông dân, hợp đồng, thu hoạch, vật tư, và các chức năng liên quan.

## Tính năng chính

- **Quản lý người dùng**: Đăng nhập, đăng ký, và phân quyền.
- **Quản lý nông dân**: Thêm, sửa, xóa thông tin nông dân.
- **Quản lý hợp đồng**: Tạo và quản lý hợp đồng giữa nông dân và hệ thống.
- **Quản lý thu hoạch**: Theo dõi và quản lý các vụ thu hoạch.
- **Quản lý vật tư**: Quản lý cung cấp vật tư và nợ vật tư.
- **Quản lý khu vực**: Phân chia và quản lý các khu vực canh tác.
- **Dashboard**: Hiển thị tổng quan dữ liệu.

## Công nghệ sử dụng

- **ASP.NET Core**: Framework chính cho ứng dụng web.
- **Entity Framework Core**: ORM để tương tác với cơ sở dữ liệu.
- **SQL Server**: Cơ sở dữ liệu chính.
- **Razor Views**: Giao diện người dùng (nếu có).
- **Bootstrap**: Framework CSS cho giao diện.

## Yêu cầu hệ thống

- .NET 10.0 SDK
- SQL Server (hoặc cơ sở dữ liệu tương thích)
- Visual Studio hoặc VS Code với extension C#

## Cài đặt và chạy

1. **Clone repository**:
   ```
   git clone <repository-url>
   cd "BTL backend"
   ```

2. **Khôi phục packages**:
   ```
   dotnet restore
   ```

3. **Cập nhật cơ sở dữ liệu**:
   ```
   dotnet ef database update
   ```

4. **Chạy ứng dụng**:
   ```
   dotnet run
   ```

Ứng dụng sẽ chạy trên `https://localhost:5001` (hoặc cổng được cấu hình trong `launchSettings.json`).

## Cấu trúc dự án

- **Controllers/**: Chứa các controller API (Account, Contracts, Farmers, v.v.)
- **Models/**: Định nghĩa các model dữ liệu (Farmer, Contract, Harvest, v.v.)
- **Data/**: ApplicationDbContext và cấu hình cơ sở dữ liệu.
- **Migrations/**: Các migration cho Entity Framework.
- **Views/**: Các view Razor cho giao diện web.
- **wwwroot/**: Tài nguyên tĩnh (CSS, JS, Bootstrap).

## API Endpoints

Dưới đây là một số endpoint chính (có thể thay đổi dựa trên cấu hình):

- `GET /Home/Index`: Trang chủ
- `GET /Account/Login`: Đăng nhập
- `GET /Farmers`: Danh sách nông dân
- `POST /Farmers/Create`: Tạo nông dân mới
- `GET /Contracts`: Danh sách hợp đồng
- `GET /Harvests`: Danh sách thu hoạch
- `GET /Supplies`: Danh sách vật tư
- `GET /SupplyDebts`: Danh sách nợ vật tư
- `GET /Zones`: Danh sách khu vực

Để xem chi tiết API, hãy sử dụng Swagger UI khi chạy ứng dụng (nếu được cấu hình).

## Cấu hình

- **appsettings.json**: Cấu hình chung.
- **appsettings.Development.json**: Cấu hình cho môi trường phát triển.
- **launchSettings.json**: Cấu hình chạy ứng dụng.

## Đóng góp

1. Fork dự án
2. Tạo branch mới: `git checkout -b feature/ten-tinh-nang`
3. Commit thay đổi: `git commit -m 'Thêm tính năng mới'`
4. Push: `git push origin feature/ten-tinh-nang`
5. Tạo Pull Request

## Giấy phép

Dự án này sử dụng giấy phép MIT. Xem file LICENSE để biết thêm chi tiết.
