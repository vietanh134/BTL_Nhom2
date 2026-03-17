using System.ComponentModel.DataAnnotations;

namespace BTL_backend.Models;

public class LoginViewModel
{
    [Required(ErrorMessage = "Vui lòng nhập tên đăng nhập")]
    [Display(Name = "Tên đăng nhập")]
    public string TenDangNhap { get; set; } = string.Empty;

    [Required(ErrorMessage = "Vui lòng nhập mật khẩu")]
    [Display(Name = "Mật khẩu")]
    [DataType(DataType.Password)]
    public string MatKhau { get; set; } = string.Empty;

    [Display(Name = "Ghi nhớ đăng nhập")]
    public bool GhiNho { get; set; }
}

public class CreateUserViewModel
{
    [Required(ErrorMessage = "Vui lòng nhập tên đăng nhập")]
    [Display(Name = "Tên đăng nhập")]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "Tên đăng nhập từ 3-50 ký tự")]
    public string TenDangNhap { get; set; } = string.Empty;

    [Required(ErrorMessage = "Vui lòng nhập mật khẩu")]
    [Display(Name = "Mật khẩu")]
    [DataType(DataType.Password)]
    [StringLength(100, MinimumLength = 4, ErrorMessage = "Mật khẩu tối thiểu 4 ký tự")]
    public string MatKhau { get; set; } = string.Empty;

    [Required(ErrorMessage = "Vui lòng nhập họ tên")]
    [Display(Name = "Họ và Tên")]
    [StringLength(150)]
    public string HoTen { get; set; } = string.Empty;

    [Display(Name = "Email")]
    [StringLength(100)]
    [EmailAddress(ErrorMessage = "Email không hợp lệ")]
    public string? Email { get; set; }

    [Display(Name = "Số điện thoại")]
    [StringLength(20)]
    public string? SoDienThoai { get; set; }

    [Display(Name = "Vai trò")]
    public VaiTro VaiTro { get; set; } = VaiTro.NhanVien;
}

public class ChangePasswordViewModel
{
    public int UserId { get; set; }

    [Required(ErrorMessage = "Vui lòng nhập mật khẩu mới")]
    [Display(Name = "Mật khẩu mới")]
    [DataType(DataType.Password)]
    [StringLength(100, MinimumLength = 4, ErrorMessage = "Mật khẩu tối thiểu 4 ký tự")]
    public string MatKhauMoi { get; set; } = string.Empty;
}
