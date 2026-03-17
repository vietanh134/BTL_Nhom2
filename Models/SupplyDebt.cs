using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BTL_backend.Models;

public class SupplyDebt
{
    public int Id { get; set; }

    [Required]
    [Display(Name = "Hợp đồng")]
    public int ContractId { get; set; }

    [Required]
    [Display(Name = "Vật tư")]
    public int SupplyId { get; set; }

    [Required(ErrorMessage = "Vui lòng nhập số lượng")]
    [Display(Name = "Số lượng")]
    [Column(TypeName = "decimal(18,2)")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Số lượng phải > 0")]
    public decimal SoLuong { get; set; }

    [Display(Name = "Đơn giá tại thời điểm (VNĐ)")]
    [Column(TypeName = "decimal(18,2)")]
    public decimal DonGia { get; set; }

    [Display(Name = "Thành tiền (VNĐ)")]
    [Column(TypeName = "decimal(18,2)")]
    public decimal ThanhTien { get; set; } // = SoLuong * DonGia

    [Required(ErrorMessage = "Vui lòng nhập ngày lấy vật tư")]
    [Display(Name = "Ngày lấy vật tư")]
    [DataType(DataType.Date)]
    public DateTime NgayLay { get; set; } = DateTime.Now;

    [Display(Name = "Ghi chú")]
    [StringLength(500)]
    public string? GhiChu { get; set; }

    // Navigation
    public Contract? Contract { get; set; }
    public Supply? Supply { get; set; }
}
