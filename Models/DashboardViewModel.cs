namespace BTL_backend.Models;

public class DashboardViewModel
{
    public int TongNongDan { get; set; }
    public int TongHopDong { get; set; }
    public int TongHopDongDangThucHien { get; set; }
    public int TongVung { get; set; }
    public decimal TongNoVatTu { get; set; }
    public decimal TongTienThuMua { get; set; }
    public decimal TongDienTich { get; set; }
    public int TongQuyetToan { get; set; }

    public List<Contract> HopDongGanDay { get; set; } = new();
    public List<SupplyDebt> CongNoGanDay { get; set; } = new();
    public List<ZoneStatistic> ThongKeVung { get; set; } = new();
}

public class ZoneStatistic
{
    public string TenVung { get; set; } = string.Empty;
    public int SoNongDan { get; set; }
    public decimal TongDienTich { get; set; }
    public int SoHopDong { get; set; }
}
