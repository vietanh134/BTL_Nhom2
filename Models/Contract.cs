using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BTL_backend.Models;

public enum TrangThaiHopDong
{
    [Display(Name = "Đang thực hiện")]
    DangThucHien = 0,
    [Display(Name = "Đã quyết toán")]
    DaQuyetToan = 1,
    [Display(Name = "Đã hủy")]
    DaHuy = 2
}

public class Contract
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Vui lòng nhập mã hợp đồng")]
    [Display(Name = "Mã hợp đồng")]
    [StringLength(50)]
    public string MaHopDong { get; set; } = string.Empty;

    [Required(ErrorMessage = "Vui lòng chọn nông dân")]
    [Display(Name = "Nông dân")]
    public int FarmerId { get; set; }

    [Required(ErrorMessage = "Vui lòng nhập tên mùa vụ")]
    [Display(Name = "Mùa vụ")]
    [StringLength(100)]
    public string MuaVu { get; set; } = string.Empty; // VD: Đông-Xuân 2025

    [Display(Name = "Sản phẩm thu mua")]
    [StringLength(200)]
    public string? SanPhamThuMua { get; set; } // VD: Lúa IR50404

    [Display(Name = "Giá thu mua (VNĐ/kg)")]
    [Column(TypeName = "decimal(18,2)")]
    [Range(0, double.MaxValue, ErrorMessage = "Giá phải >= 0")]
    public decimal GiaThuMua { get; set; }

    [Required(ErrorMessage = "Vui lòng nhập ngày bắt đầu")]
    [Display(Name = "Ngày bắt đầu")]
    [DataType(DataType.Date)]
    public DateTime NgayBatDau { get; set; }

    [Required(ErrorMessage = "Vui lòng nhập ngày kết thúc")]
    [Display(Name = "Ngày kết thúc")]
    [DataType(DataType.Date)]
    public DateTime NgayKetThuc { get; set; }

    [Display(Name = "Lãi suất nợ vật tư (%/tháng)")]
    [Column(TypeName = "decimal(5,2)")]
    [Range(0, 100, ErrorMessage = "Lãi suất phải từ 0-100%")]
    public decimal LaiSuat { get; set; } = 0; // % per month

    [Display(Name = "Trạng thái")]
    public TrangThaiHopDong TrangThai { get; set; } = TrangThaiHopDong.DangThucHien;

    [Display(Name = "Ghi chú")]
    [StringLength(500)]
    public string? GhiChu { get; set; }

    [Display(Name = "Ngày tạo")]
    public DateTime NgayTao { get; set; } = DateTime.Now;

    // Navigation
    public Farmer? Farmer { get; set; }
    public ICollection<SupplyDebt> SupplyDebts { get; set; } = new List<SupplyDebt>();
    public ICollection<Harvest> Harvests { get; set; } = new List<Harvest>();
    public Settlement? Settlement { get; set; }
}
