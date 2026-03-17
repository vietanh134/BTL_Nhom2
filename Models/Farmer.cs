using System.ComponentModel.DataAnnotations;

namespace BTL_backend.Models;

public class Farmer
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Vui lòng nhập họ tên")]
    [Display(Name = "Họ và Tên")]
    [StringLength(150)]
    public string HoTen { get; set; } = string.Empty;

    [Display(Name = "Số điện thoại")]
    [StringLength(20)]
    public string? SoDienThoai { get; set; }

    [Display(Name = "Số CCCD/CMND")]
    [StringLength(20)]
    public string? SoCCCD { get; set; }

    [Display(Name = "Địa chỉ")]
    [StringLength(300)]
    public string? DiaChi { get; set; }

    [Display(Name = "Diện tích canh tác (ha)")]
    [Range(0, 10000, ErrorMessage = "Diện tích phải >= 0")]
    public decimal DienTichCanhTac { get; set; }

    [Required(ErrorMessage = "Vui lòng chọn vùng")]
    [Display(Name = "Vùng nguyên liệu")]
    public int ZoneId { get; set; }

    [Display(Name = "Ghi chú")]
    [StringLength(500)]
    public string? GhiChu { get; set; }

    [Display(Name = "Ngày tạo")]
    public DateTime NgayTao { get; set; } = DateTime.Now;

    // Navigation
    public Zone? Zone { get; set; }
    public ICollection<Contract> Contracts { get; set; } = new List<Contract>();
}
