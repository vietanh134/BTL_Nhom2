using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using BTL_backend.Data;
using BTL_backend.Models;

namespace BTL_backend.Controllers;

[Authorize]
public class ContractsController : Controller
{
    private readonly ApplicationDbContext _db;
    public ContractsController(ApplicationDbContext db) => _db = db;

    public async Task<IActionResult> Index(string? search, TrangThaiHopDong? status)
    {
        var query = _db.Contracts.Include(c => c.Farmer).ThenInclude(f => f!.Zone).AsQueryable();
        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(c => c.MaHopDong.Contains(search) || c.Farmer!.HoTen.Contains(search));
            ViewBag.Search = search;
        }
        if (status.HasValue)
        {
            query = query.Where(c => c.TrangThai == status);
            ViewBag.Status = status;
        }
        return View(await query.OrderByDescending(c => c.NgayTao).ToListAsync());
    }

    public async Task<IActionResult> Create()
    {
        ViewBag.Farmers = new SelectList(await _db.Farmers.ToListAsync(), "Id", "HoTen");
        return View(new Contract());
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Contract contract)
    {
        if (await _db.Contracts.AnyAsync(c => c.MaHopDong == contract.MaHopDong))
            ModelState.AddModelError("MaHopDong", "Mã hợp đồng đã tồn tại!");

        if (ModelState.IsValid)
        {
            contract.NgayTao = DateTime.Now;
            _db.Contracts.Add(contract);
            await _db.SaveChangesAsync();
            TempData["Success"] = "Tạo hợp đồng thành công!";
            return RedirectToAction(nameof(Index));
        }
        ViewBag.Farmers = new SelectList(await _db.Farmers.ToListAsync(), "Id", "HoTen", contract.FarmerId);
        return View(contract);
    }

    public async Task<IActionResult> Edit(int id)
    {
        var contract = await _db.Contracts.FindAsync(id);
        if (contract == null) return NotFound();
        ViewBag.Farmers = new SelectList(await _db.Farmers.ToListAsync(), "Id", "HoTen", contract.FarmerId);
        return View(contract);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Contract contract)
    {
        if (id != contract.Id) return NotFound();
        if (await _db.Contracts.AnyAsync(c => c.MaHopDong == contract.MaHopDong && c.Id != id))
            ModelState.AddModelError("MaHopDong", "Mã hợp đồng đã tồn tại!");

        if (ModelState.IsValid)
        {
            _db.Update(contract);
            await _db.SaveChangesAsync();
            TempData["Success"] = "Cập nhật hợp đồng thành công!";
            return RedirectToAction(nameof(Index));
        }
        ViewBag.Farmers = new SelectList(await _db.Farmers.ToListAsync(), "Id", "HoTen", contract.FarmerId);
        return View(contract);
    }

    public async Task<IActionResult> Details(int id)
    {
        var contract = await _db.Contracts
            .Include(c => c.Farmer).ThenInclude(f => f!.Zone)
            .Include(c => c.SupplyDebts).ThenInclude(sd => sd.Supply)
            .Include(c => c.Harvests)
            .Include(c => c.Settlement)
            .FirstOrDefaultAsync(c => c.Id == id);
        if (contract == null) return NotFound();
        return View(contract);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var contract = await _db.Contracts
            .Include(c => c.SupplyDebts)
            .Include(c => c.Harvests)
            .FirstOrDefaultAsync(c => c.Id == id);
        if (contract == null) return NotFound();
        _db.Contracts.Remove(contract);
        await _db.SaveChangesAsync();
        TempData["Success"] = "Xóa hợp đồng thành công!";
        return RedirectToAction(nameof(Index));
    }
}
