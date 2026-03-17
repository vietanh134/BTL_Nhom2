using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using BTL_backend.Data;
using BTL_backend.Models;

namespace BTL_backend.Controllers;

[Authorize]
public class HarvestsController : Controller
{
    private readonly ApplicationDbContext _db;
    public HarvestsController(ApplicationDbContext db) => _db = db;

    public async Task<IActionResult> Index(int? contractId)
    {
        var query = _db.Harvests
            .Include(h => h.Contract).ThenInclude(c => c!.Farmer)
            .AsQueryable();

        if (contractId.HasValue)
        {
            query = query.Where(h => h.ContractId == contractId);
            ViewBag.ContractId = contractId;
            ViewBag.Contract = await _db.Contracts.Include(c => c.Farmer).FirstOrDefaultAsync(c => c.Id == contractId);
        }

        return View(await query.OrderByDescending(h => h.NgayThuMua).ToListAsync());
    }

    public async Task<IActionResult> Create(int? contractId)
    {
        var activeContracts = await _db.Contracts
            .Include(c => c.Farmer)
            .Where(c => c.TrangThai == TrangThaiHopDong.DangThucHien)
            .ToListAsync();
        ViewBag.Contracts = new SelectList(
            activeContracts.Select(c => new { c.Id, Display = $"{c.MaHopDong} - {c.Farmer?.HoTen}" }),
            "Id", "Display", contractId);
        ViewBag.ContractPrices = await _db.Contracts
            .Where(c => c.TrangThai == TrangThaiHopDong.DangThucHien)
            .ToDictionaryAsync(c => c.Id, c => new { c.GiaThuMua, c.SanPhamThuMua });

        var harvest = new Harvest { NgayThuMua = DateTime.Now };
        if (contractId.HasValue)
        {
            harvest.ContractId = contractId.Value;
            var contract = activeContracts.FirstOrDefault(c => c.Id == contractId);
            if (contract != null)
            {
                harvest.GiaThuMua = contract.GiaThuMua;
                harvest.TenNongSan = contract.SanPhamThuMua ?? "";
            }
        }
        return View(harvest);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Harvest harvest)
    {
        if (ModelState.IsValid)
        {
            harvest.ThanhTien = harvest.SoLuong * harvest.GiaThuMua;
            _db.Harvests.Add(harvest);
            await _db.SaveChangesAsync();
            TempData["Success"] = $"Ghi nhận thu mua thành công! Thành tiền: {harvest.ThanhTien:N0} VNĐ";
            return RedirectToAction(nameof(Index), new { contractId = harvest.ContractId });
        }
        var activeContracts = await _db.Contracts.Include(c => c.Farmer)
            .Where(c => c.TrangThai == TrangThaiHopDong.DangThucHien).ToListAsync();
        ViewBag.Contracts = new SelectList(
            activeContracts.Select(c => new { c.Id, Display = $"{c.MaHopDong} - {c.Farmer?.HoTen}" }),
            "Id", "Display", harvest.ContractId);
        return View(harvest);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var harvest = await _db.Harvests.FindAsync(id);
        if (harvest == null) return NotFound();
        var contractId = harvest.ContractId;
        _db.Harvests.Remove(harvest);
        await _db.SaveChangesAsync();
        TempData["Success"] = "Xóa bản ghi thu mua thành công!";
        return RedirectToAction(nameof(Index), new { contractId });
    }

    // ===== QUYẾT TOÁN MÙA VỤ =====
    public async Task<IActionResult> Settlement(int contractId)
    {
        var contract = await _db.Contracts
            .Include(c => c.Farmer).ThenInclude(f => f!.Zone)
            .Include(c => c.SupplyDebts).ThenInclude(sd => sd.Supply)
            .Include(c => c.Harvests)
            .Include(c => c.Settlement)
            .FirstOrDefaultAsync(c => c.Id == contractId);

        if (contract == null) return NotFound();

        // Calculate
        var tongThuMua = contract.Harvests.Sum(h => h.ThanhTien);
        var tongNoVatTu = contract.SupplyDebts.Sum(sd => sd.ThanhTien);

        // Interest calculation: for each supply debt, calculate interest from date taken to now
        decimal tienLai = 0;
        if (contract.LaiSuat > 0)
        {
            var settlementDate = DateTime.Now;
            foreach (var debt in contract.SupplyDebts)
            {
                var months = (decimal)(settlementDate - debt.NgayLay).TotalDays / 30m;
                if (months > 0)
                {
                    tienLai += debt.ThanhTien * (contract.LaiSuat / 100m) * months;
                }
            }
        }
        tienLai = Math.Round(tienLai, 0);

        ViewBag.TongThuMua = tongThuMua;
        ViewBag.TongNoVatTu = tongNoVatTu;
        ViewBag.TienLai = tienLai;
        ViewBag.ThucNhan = tongThuMua - tongNoVatTu - tienLai;

        return View(contract);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> ConfirmSettlement(int contractId, string? ghiChu)
    {
        var contract = await _db.Contracts
            .Include(c => c.SupplyDebts)
            .Include(c => c.Harvests)
            .Include(c => c.Settlement)
            .FirstOrDefaultAsync(c => c.Id == contractId);

        if (contract == null) return NotFound();
        if (contract.Settlement != null)
        {
            TempData["Error"] = "Hợp đồng này đã được quyết toán!";
            return RedirectToAction("Details", "Contracts", new { id = contractId });
        }

        var tongThuMua = contract.Harvests.Sum(h => h.ThanhTien);
        var tongNoVatTu = contract.SupplyDebts.Sum(sd => sd.ThanhTien);

        decimal tienLai = 0;
        if (contract.LaiSuat > 0)
        {
            var now = DateTime.Now;
            foreach (var debt in contract.SupplyDebts)
            {
                var months = (decimal)(now - debt.NgayLay).TotalDays / 30m;
                if (months > 0)
                    tienLai += debt.ThanhTien * (contract.LaiSuat / 100m) * months;
            }
        }
        tienLai = Math.Round(tienLai, 0);

        var settlement = new Settlement
        {
            ContractId = contractId,
            TongTienThuMua = tongThuMua,
            TongNoVatTu = tongNoVatTu,
            TienLai = tienLai,
            SoTienThucNhan = tongThuMua - tongNoVatTu - tienLai,
            NgayQuyetToan = DateTime.Now,
            GhiChu = ghiChu
        };

        _db.Settlements.Add(settlement);
        contract.TrangThai = TrangThaiHopDong.DaQuyetToan;
        await _db.SaveChangesAsync();

        TempData["Success"] = $"Quyết toán thành công! Số tiền thực nhận: {settlement.SoTienThucNhan:N0} VNĐ";
        return RedirectToAction("Details", "Contracts", new { id = contractId });
    }
}
