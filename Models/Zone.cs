using System.ComponentModel.DataAnnotations;

namespace BTL_backend.Models;

public class Zone
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Vui lòng nhập tên thôn/xã")]
    [Display(Name = "Tên Thôn/Xã")]
    [StringLength(100)]
    public string TenThonXa { get; set; } = string.Empty;

    [Display(Name = "Huyện/Quận")]
    [StringLength(100)]
    public string? HuyenQuan { get; set; }

    [Display(Name = "Tỉnh/Thành phố")]
    [StringLength(100)]
    public string? TinhThanhPho { get; set; }

    [Display(Name = "Ghi chú")]
    [StringLength(500)]
    public string? GhiChu { get; set; }

    // Navigation
    public ICollection<Farmer> Farmers { get; set; } = new List<Farmer>();
}
