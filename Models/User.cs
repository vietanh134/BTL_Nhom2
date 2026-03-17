using System.ComponentModel.DataAnnotations;

namespace BTL_backend.Models;

public enum VaiTro
{
    [Display(Name = "Quản trị viên")]
    Admin = 0,
    [Display(Name = "Nhân viên")]
    NhanVien = 1
}

public class User
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Vui lòng nhập tên đăng nhập")]
    [Display(Name = "Tên đăng nhập")]
    [StringLength(50)]
    public string TenDangNhap { get; set; } = string.Empty;

    [Required(ErrorMessage = "Vui lòng nhập mật khẩu")]
    [Display(Name = "Mật khẩu")]
    [StringLength(256)]
    public string MatKhau { get; set; } = string.Empty; // Hashed

    [Required(ErrorMessage = "Vui lòng nhập họ tên")]
    [Display(Name = "Họ và Tên")]
    [StringLength(150)]
    public string HoTen { get; set; } = string.Empty;

    [Display(Name = "Email")]
    [StringLength(100)]
    public string? Email { get; set; }

    [Display(Name = "Số điện thoại")]
    [StringLength(20)]
    public string? SoDienThoai { get; set; }

    [Display(Name = "Vai trò")]
    public VaiTro VaiTro { get; set; } = VaiTro.NhanVien;

    [Display(Name = "Hoạt động")]
    public bool HoatDong { get; set; } = true;

    [Display(Name = "Ngày tạo")]
    public DateTime NgayTao { get; set; } = DateTime.Now;
}
