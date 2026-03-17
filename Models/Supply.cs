using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BTL_backend.Models;

public class Supply
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Vui lòng nhập tên vật tư")]
    [Display(Name = "Tên vật tư")]
    [StringLength(200)]
    public string TenVatTu { get; set; } = string.Empty;

    [Display(Name = "Loại vật tư")]
    [StringLength(100)]
    public string? LoaiVatTu { get; set; } // Giống, Phân bón, Thuốc BVTV

    [Required(ErrorMessage = "Vui lòng nhập đơn vị")]
    [Display(Name = "Đơn vị tính")]
    [StringLength(50)]
    public string DonViTinh { get; set; } = string.Empty; // kg, bao, chai

    [Required(ErrorMessage = "Vui lòng nhập đơn giá")]
    [Display(Name = "Đơn giá (VNĐ)")]
    [Column(TypeName = "decimal(18,2)")]
    [Range(0, double.MaxValue, ErrorMessage = "Đơn giá phải >= 0")]
    public decimal DonGia { get; set; }

    [Display(Name = "Mô tả")]
    [StringLength(500)]
    public string? MoTa { get; set; }

    // Navigation
    public ICollection<SupplyDebt> SupplyDebts { get; set; } = new List<SupplyDebt>();
}
