using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BTL_backend.Models;

public class Harvest
{
    public int Id { get; set; }

    [Required]
    [Display(Name = "Hợp đồng")]
    public int ContractId { get; set; }

    [Required(ErrorMessage = "Vui lòng nhập tên nông sản")]
    [Display(Name = "Tên nông sản")]
    [StringLength(200)]
    public string TenNongSan { get; set; } = string.Empty;

    [Required(ErrorMessage = "Vui lòng nhập số lượng")]
    [Display(Name = "Số lượng (kg)")]
    [Column(TypeName = "decimal(18,2)")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Số lượng phải > 0")]
    public decimal SoLuong { get; set; }

    [Display(Name = "Giá thu mua (VNĐ/kg)")]
    [Column(TypeName = "decimal(18,2)")]
    public decimal GiaThuMua { get; set; }

    [Display(Name = "Thành tiền (VNĐ)")]
    [Column(TypeName = "decimal(18,2)")]
    public decimal ThanhTien { get; set; } // = SoLuong * GiaThuMua

    [Required(ErrorMessage = "Vui lòng nhập ngày thu mua")]
    [Display(Name = "Ngày thu mua")]
    [DataType(DataType.Date)]
    public DateTime NgayThuMua { get; set; } = DateTime.Now;

    [Display(Name = "Chất lượng")]
    [StringLength(100)]
    public string? ChatLuong { get; set; } // Loại 1, Loại 2...

    [Display(Name = "Ghi chú")]
    [StringLength(500)]
    public string? GhiChu { get; set; }

    // Navigation
    public Contract? Contract { get; set; }
}
