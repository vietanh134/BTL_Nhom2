using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using BTL_backend.Data;
using BTL_backend.Models;

namespace BTL_backend.Controllers;

[Authorize]
public class FarmersController : Controller
{
    private readonly ApplicationDbContext _db;
    public FarmersController(ApplicationDbContext db) => _db = db;

    public async Task<IActionResult> Index(string? search, int? zoneId)
    {
        var query = _db.Farmers.Include(f => f.Zone).AsQueryable();
        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(f => f.HoTen.Contains(search) || (f.SoDienThoai != null && f.SoDienThoai.Contains(search)));
            ViewBag.Search = search;
        }
        if (zoneId.HasValue)
        {
            query = query.Where(f => f.ZoneId == zoneId);
            ViewBag.ZoneId = zoneId;
        }
        ViewBag.Zones = new SelectList(await _db.Zones.ToListAsync(), "Id", "TenThonXa", zoneId);
        return View(await query.OrderBy(f => f.HoTen).ToListAsync());
    }

    public async Task<IActionResult> Create()
    {
        ViewBag.Zones = new SelectList(await _db.Zones.ToListAsync(), "Id", "TenThonXa");
        return View(new Farmer());
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Farmer farmer)
    {
        if (ModelState.IsValid)
        {
            farmer.NgayTao = DateTime.Now;
            _db.Farmers.Add(farmer);
            await _db.SaveChangesAsync();
            TempData["Success"] = "Thêm nông dân thành công!";
            return RedirectToAction(nameof(Index));
        }
        ViewBag.Zones = new SelectList(await _db.Zones.ToListAsync(), "Id", "TenThonXa", farmer.ZoneId);
        return View(farmer);
    }

    public async Task<IActionResult> Edit(int id)
    {
        var farmer = await _db.Farmers.FindAsync(id);
        if (farmer == null) return NotFound();
        ViewBag.Zones = new SelectList(await _db.Zones.ToListAsync(), "Id", "TenThonXa", farmer.ZoneId);
        return View(farmer);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Farmer farmer)
    {
        if (id != farmer.Id) return NotFound();
        if (ModelState.IsValid)
        {
            _db.Update(farmer);
            await _db.SaveChangesAsync();
            TempData["Success"] = "Cập nhật nông dân thành công!";
            return RedirectToAction(nameof(Index));
        }
        ViewBag.Zones = new SelectList(await _db.Zones.ToListAsync(), "Id", "TenThonXa", farmer.ZoneId);
        return View(farmer);
    }

    public async Task<IActionResult> Details(int id)
    {
        var farmer = await _db.Farmers
            .Include(f => f.Zone)
            .Include(f => f.Contracts).ThenInclude(c => c.SupplyDebts).ThenInclude(sd => sd.Supply)
            .Include(f => f.Contracts).ThenInclude(c => c.Harvests)
            .Include(f => f.Contracts).ThenInclude(c => c.Settlement)
            .FirstOrDefaultAsync(f => f.Id == id);
        if (farmer == null) return NotFound();
        return View(farmer);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var farmer = await _db.Farmers.Include(f => f.Contracts).FirstOrDefaultAsync(f => f.Id == id);
        if (farmer == null) return NotFound();
        if (farmer.Contracts.Any())
        {
            TempData["Error"] = "Không thể xóa nông dân đang có hợp đồng!";
            return RedirectToAction(nameof(Index));
        }
        _db.Farmers.Remove(farmer);
        await _db.SaveChangesAsync();
        TempData["Success"] = "Xóa nông dân thành công!";
        return RedirectToAction(nameof(Index));
    }
}
