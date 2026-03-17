using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using BTL_backend.Data;
using BTL_backend.Models;

namespace BTL_backend.Controllers;

[Authorize]
public class SupplyDebtsController : Controller
{
    private readonly ApplicationDbContext _db;
    public SupplyDebtsController(ApplicationDbContext db) => _db = db;

    public async Task<IActionResult> Index(int? contractId)
    {
        var query = _db.SupplyDebts
            .Include(sd => sd.Contract).ThenInclude(c => c!.Farmer)
            .Include(sd => sd.Supply)
            .AsQueryable();

        if (contractId.HasValue)
        {
            query = query.Where(sd => sd.ContractId == contractId);
            ViewBag.ContractId = contractId;
            ViewBag.Contract = await _db.Contracts.Include(c => c.Farmer).FirstOrDefaultAsync(c => c.Id == contractId);
        }

        return View(await query.OrderByDescending(sd => sd.NgayLay).ToListAsync());
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
        ViewBag.Supplies = new SelectList(await _db.Supplies.ToListAsync(), "Id", "TenVatTu");
        ViewBag.SupplyPrices = await _db.Supplies.ToDictionaryAsync(s => s.Id, s => s.DonGia);

        var debt = new SupplyDebt { NgayLay = DateTime.Now };
        if (contractId.HasValue) debt.ContractId = contractId.Value;
        return View(debt);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(SupplyDebt debt)
    {
        if (ModelState.IsValid)
        {
            var supply = await _db.Supplies.FindAsync(debt.SupplyId);
            if (supply != null)
            {
                debt.DonGia = supply.DonGia;
                debt.ThanhTien = debt.SoLuong * debt.DonGia;
            }
            _db.SupplyDebts.Add(debt);
            await _db.SaveChangesAsync();
            TempData["Success"] = $"Ghi nợ vật tư thành công! Thành tiền: {debt.ThanhTien:N0} VNĐ";
            return RedirectToAction(nameof(Index), new { contractId = debt.ContractId });
        }
        var activeContracts = await _db.Contracts.Include(c => c.Farmer)
            .Where(c => c.TrangThai == TrangThaiHopDong.DangThucHien).ToListAsync();
        ViewBag.Contracts = new SelectList(
            activeContracts.Select(c => new { c.Id, Display = $"{c.MaHopDong} - {c.Farmer?.HoTen}" }),
            "Id", "Display", debt.ContractId);
        ViewBag.Supplies = new SelectList(await _db.Supplies.ToListAsync(), "Id", "TenVatTu", debt.SupplyId);
        ViewBag.SupplyPrices = await _db.Supplies.ToDictionaryAsync(s => s.Id, s => s.DonGia);
        return View(debt);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var debt = await _db.SupplyDebts.FindAsync(id);
        if (debt == null) return NotFound();
        var contractId = debt.ContractId;
        _db.SupplyDebts.Remove(debt);
        await _db.SaveChangesAsync();
        TempData["Success"] = "Xóa công nợ thành công!";
        return RedirectToAction(nameof(Index), new { contractId });
    }
}
