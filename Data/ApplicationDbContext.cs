using Microsoft.EntityFrameworkCore;
using BTL_backend.Models;
using System.Security.Cryptography;
using System.Text;

namespace BTL_backend.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }

    public DbSet<Zone> Zones => Set<Zone>();
    public DbSet<Farmer> Farmers => Set<Farmer>();
    public DbSet<Supply> Supplies => Set<Supply>();
    public DbSet<Contract> Contracts => Set<Contract>();
    public DbSet<SupplyDebt> SupplyDebts => Set<SupplyDebt>();
    public DbSet<Harvest> Harvests => Set<Harvest>();
    public DbSet<Settlement> Settlements => Set<Settlement>();
    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Zone - Farmer (1:N)
        modelBuilder.Entity<Farmer>()
            .HasOne(f => f.Zone)
            .WithMany(z => z.Farmers)
            .HasForeignKey(f => f.ZoneId)
            .OnDelete(DeleteBehavior.Restrict);

        // Farmer - Contract (1:N)
        modelBuilder.Entity<Contract>()
            .HasOne(c => c.Farmer)
            .WithMany(f => f.Contracts)
            .HasForeignKey(c => c.FarmerId)
            .OnDelete(DeleteBehavior.Restrict);

        // Contract - SupplyDebt (1:N)
        modelBuilder.Entity<SupplyDebt>()
            .HasOne(sd => sd.Contract)
            .WithMany(c => c.SupplyDebts)
            .HasForeignKey(sd => sd.ContractId)
            .OnDelete(DeleteBehavior.Cascade);

        // Supply - SupplyDebt (1:N)
        modelBuilder.Entity<SupplyDebt>()
            .HasOne(sd => sd.Supply)
            .WithMany(s => s.SupplyDebts)
            .HasForeignKey(sd => sd.SupplyId)
            .OnDelete(DeleteBehavior.Restrict);

        // Contract - Harvest (1:N)
        modelBuilder.Entity<Harvest>()
            .HasOne(h => h.Contract)
            .WithMany(c => c.Harvests)
            .HasForeignKey(h => h.ContractId)
            .OnDelete(DeleteBehavior.Cascade);

        // Contract - Settlement (1:1)
        modelBuilder.Entity<Settlement>()
            .HasOne(s => s.Contract)
            .WithOne(c => c.Settlement)
            .HasForeignKey<Settlement>(s => s.ContractId)
            .OnDelete(DeleteBehavior.Cascade);

        // Unique constraint on MaHopDong
        modelBuilder.Entity<Contract>()
            .HasIndex(c => c.MaHopDong)
            .IsUnique();

        // Unique constraint on TenDangNhap
        modelBuilder.Entity<User>()
            .HasIndex(u => u.TenDangNhap)
            .IsUnique();

        // ===== SEED DATA =====
        modelBuilder.Entity<Zone>().HasData(
            new Zone { Id = 1, TenThonXa = "Xã An Phú", HuyenQuan = "Huyện Củ Chi", TinhThanhPho = "TP. Hồ Chí Minh" },
            new Zone { Id = 2, TenThonXa = "Xã Tân Thạnh", HuyenQuan = "Huyện Thủ Thừa", TinhThanhPho = "Long An" },
            new Zone { Id = 3, TenThonXa = "Xã Mỹ Lộc", HuyenQuan = "Huyện Cao Lãnh", TinhThanhPho = "Đồng Tháp" },
            new Zone { Id = 4, TenThonXa = "Xã Long Hòa", HuyenQuan = "Huyện Châu Thành", TinhThanhPho = "Tiền Giang" },
            new Zone { Id = 5, TenThonXa = "Xã Phước Thới", HuyenQuan = "Huyện Ô Môn", TinhThanhPho = "Cần Thơ" }
        );

        modelBuilder.Entity<Supply>().HasData(
            new Supply { Id = 1, TenVatTu = "Giống lúa IR50404", LoaiVatTu = "Giống", DonViTinh = "kg", DonGia = 15000, MoTa = "Giống lúa năng suất cao" },
            new Supply { Id = 2, TenVatTu = "Giống lúa Jasmine 85", LoaiVatTu = "Giống", DonViTinh = "kg", DonGia = 22000, MoTa = "Giống lúa thơm chất lượng" },
            new Supply { Id = 3, TenVatTu = "Phân NPK 20-20-15", LoaiVatTu = "Phân bón", DonViTinh = "bao (50kg)", DonGia = 850000, MoTa = "Phân bón tổng hợp" },
            new Supply { Id = 4, TenVatTu = "Phân Ure Phú Mỹ", LoaiVatTu = "Phân bón", DonViTinh = "bao (50kg)", DonGia = 520000, MoTa = "Phân đạm" },
            new Supply { Id = 5, TenVatTu = "Thuốc trừ sâu Regent 800WG", LoaiVatTu = "Thuốc BVTV", DonViTinh = "gói", DonGia = 45000, MoTa = "Thuốc trừ sâu đục thân" }
        );

        modelBuilder.Entity<Farmer>().HasData(
            new Farmer { Id = 1, HoTen = "Nguyễn Văn An", SoDienThoai = "0901234567", SoCCCD = "079201001234", DiaChi = "Ấp 1, Xã An Phú", DienTichCanhTac = 2.5m, ZoneId = 1, NgayTao = new DateTime(2025, 1, 15) },
            new Farmer { Id = 2, HoTen = "Trần Thị Bé", SoDienThoai = "0912345678", SoCCCD = "079201005678", DiaChi = "Ấp 3, Xã An Phú", DienTichCanhTac = 1.8m, ZoneId = 1, NgayTao = new DateTime(2025, 1, 20) },
            new Farmer { Id = 3, HoTen = "Lê Văn Cường", SoDienThoai = "0923456789", SoCCCD = "080201002345", DiaChi = "Ấp 2, Xã Tân Thạnh", DienTichCanhTac = 3.2m, ZoneId = 2, NgayTao = new DateTime(2025, 2, 1) },
            new Farmer { Id = 4, HoTen = "Phạm Thị Diệu", SoDienThoai = "0934567890", SoCCCD = "082201003456", DiaChi = "Ấp 5, Xã Mỹ Lộc", DienTichCanhTac = 4.0m, ZoneId = 3, NgayTao = new DateTime(2025, 2, 10) },
            new Farmer { Id = 5, HoTen = "Huỳnh Văn Em", SoDienThoai = "0945678901", SoCCCD = "086201004567", DiaChi = "Ấp 1, Xã Long Hòa", DienTichCanhTac = 2.0m, ZoneId = 4, NgayTao = new DateTime(2025, 3, 1) }
        );

        modelBuilder.Entity<Contract>().HasData(
            new Contract
            {
                Id = 1, MaHopDong = "HD-2025-001", FarmerId = 1, MuaVu = "Đông-Xuân 2025",
                SanPhamThuMua = "Lúa IR50404", GiaThuMua = 7500,
                NgayBatDau = new DateTime(2025, 1, 15), NgayKetThuc = new DateTime(2025, 5, 15),
                LaiSuat = 0.5m, TrangThai = TrangThaiHopDong.DangThucHien,
                NgayTao = new DateTime(2025, 1, 15)
            },
            new Contract
            {
                Id = 2, MaHopDong = "HD-2025-002", FarmerId = 3, MuaVu = "Đông-Xuân 2025",
                SanPhamThuMua = "Lúa Jasmine 85", GiaThuMua = 8200,
                NgayBatDau = new DateTime(2025, 2, 1), NgayKetThuc = new DateTime(2025, 6, 1),
                LaiSuat = 0, TrangThai = TrangThaiHopDong.DangThucHien,
                NgayTao = new DateTime(2025, 2, 1)
            },
            new Contract
            {
                Id = 3, MaHopDong = "HD-2025-003", FarmerId = 4, MuaVu = "Hè-Thu 2025",
                SanPhamThuMua = "Lúa IR50404", GiaThuMua = 7200,
                NgayBatDau = new DateTime(2025, 5, 20), NgayKetThuc = new DateTime(2025, 9, 20),
                LaiSuat = 0.5m, TrangThai = TrangThaiHopDong.DangThucHien,
                NgayTao = new DateTime(2025, 5, 20)
            }
        );

        modelBuilder.Entity<SupplyDebt>().HasData(
            new SupplyDebt { Id = 1, ContractId = 1, SupplyId = 1, SoLuong = 50, DonGia = 15000, ThanhTien = 750000, NgayLay = new DateTime(2025, 1, 20) },
            new SupplyDebt { Id = 2, ContractId = 1, SupplyId = 3, SoLuong = 10, DonGia = 850000, ThanhTien = 8500000, NgayLay = new DateTime(2025, 2, 5) },
            new SupplyDebt { Id = 3, ContractId = 1, SupplyId = 4, SoLuong = 5, DonGia = 520000, ThanhTien = 2600000, NgayLay = new DateTime(2025, 3, 1) },
            new SupplyDebt { Id = 4, ContractId = 2, SupplyId = 2, SoLuong = 80, DonGia = 22000, ThanhTien = 1760000, NgayLay = new DateTime(2025, 2, 10) },
            new SupplyDebt { Id = 5, ContractId = 2, SupplyId = 3, SoLuong = 15, DonGia = 850000, ThanhTien = 12750000, NgayLay = new DateTime(2025, 2, 20) }
        );

        modelBuilder.Entity<Harvest>().HasData(
            new Harvest { Id = 1, ContractId = 1, TenNongSan = "Lúa IR50404", SoLuong = 5000, GiaThuMua = 7500, ThanhTien = 37500000, NgayThuMua = new DateTime(2025, 5, 10), ChatLuong = "Loại 1" },
            new Harvest { Id = 2, ContractId = 2, TenNongSan = "Lúa Jasmine 85", SoLuong = 7000, GiaThuMua = 8200, ThanhTien = 57400000, NgayThuMua = new DateTime(2025, 5, 25), ChatLuong = "Loại 1" }
        );

        // Seed admin user (password: admin123)
        modelBuilder.Entity<User>().HasData(
            new User
            {
                Id = 1,
                TenDangNhap = "admin",
                MatKhau = HashPassword("admin123"),
                HoTen = "Quản trị viên",
                Email = "admin@agricontract.vn",
                SoDienThoai = "0900000000",
                VaiTro = VaiTro.Admin,
                HoatDong = true,
                NgayTao = new DateTime(2025, 1, 1)
            },
            new User
            {
                Id = 2,
                TenDangNhap = "nhanvien1",
                MatKhau = HashPassword("123456"),
                HoTen = "Nguyễn Thị Hoa",
                Email = "hoa@agricontract.vn",
                SoDienThoai = "0911111111",
                VaiTro = VaiTro.NhanVien,
                HoatDong = true,
                NgayTao = new DateTime(2025, 1, 10)
            }
        );
    }

    public static string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(bytes);
    }
}
