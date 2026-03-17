using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using BTL_backend.Data;
using BTL_backend.Models;

namespace BTL_backend.Controllers;

[Authorize]
public class HomeController : Controller
{
    private readonly ApplicationDbContext _db;
    public HomeController(ApplicationDbContext db) => _db = db;

    public async Task<IActionResult> Index()
    {
        var vm = new DashboardViewModel
        {
            TongNongDan = await _db.Farmers.CountAsync(),
            TongHopDong = await _db.Contracts.CountAsync(),
            TongHopDongDangThucHien = await _db.Contracts.CountAsync(c => c.TrangThai == TrangThaiHopDong.DangThucHien),
            TongVung = await _db.Zones.CountAsync(),
            TongNoVatTu = await _db.SupplyDebts.SumAsync(sd => (decimal?)sd.ThanhTien) ?? 0,
            TongTienThuMua = await _db.Harvests.SumAsync(h => (decimal?)h.ThanhTien) ?? 0,
            TongDienTich = await _db.Farmers.SumAsync(f => (decimal?)f.DienTichCanhTac) ?? 0,
            TongQuyetToan = await _db.Settlements.CountAsync(),

            HopDongGanDay = await _db.Contracts
                .Include(c => c.Farmer)
                .OrderByDescending(c => c.NgayTao)
                .Take(5)
                .ToListAsync(),

            CongNoGanDay = await _db.SupplyDebts
                .Include(sd => sd.Contract).ThenInclude(c => c!.Farmer)
                .Include(sd => sd.Supply)
                .OrderByDescending(sd => sd.NgayLay)
                .Take(5)
                .ToListAsync(),

            ThongKeVung = await _db.Zones
                .Select(z => new ZoneStatistic
                {
                    TenVung = z.TenThonXa,
                    SoNongDan = z.Farmers.Count,
                    TongDienTich = z.Farmers.Sum(f => f.DienTichCanhTac),
                    SoHopDong = z.Farmers.SelectMany(f => f.Contracts).Count()
                })
                .ToListAsync()
        };
        return View(vm);
    }
}
