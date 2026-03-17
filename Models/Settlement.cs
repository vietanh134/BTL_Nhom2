using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BTL_backend.Models;

public class Settlement
{
    public int Id { get; set; }

    [Required]
    [Display(Name = "Hợp đồng")]
    public int ContractId { get; set; }

    [Display(Name = "Tổng tiền thu mua (VNĐ)")]
    [Column(TypeName = "decimal(18,2)")]
    public decimal TongTienThuMua { get; set; }

    [Display(Name = "Tổng nợ vật tư (VNĐ)")]
    [Column(TypeName = "decimal(18,2)")]
    public decimal TongNoVatTu { get; set; }

    [Display(Name = "Tiền lãi (VNĐ)")]
    [Column(TypeName = "decimal(18,2)")]
    public decimal TienLai { get; set; }

    [Display(Name = "Số tiền thực nhận (VNĐ)")]
    [Column(TypeName = "decimal(18,2)")]
    public decimal SoTienThucNhan { get; set; } // = TongTienThuMua - TongNoVatTu - TienLai

    [Display(Name = "Ngày quyết toán")]
    [DataType(DataType.Date)]
    public DateTime NgayQuyetToan { get; set; } = DateTime.Now;

    [Display(Name = "Ghi chú")]
    [StringLength(500)]
    public string? GhiChu { get; set; }

    // Navigation
    public Contract? Contract { get; set; }
}
