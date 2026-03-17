using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using BTL_backend.Data;
using BTL_backend.Models;

namespace BTL_backend.Controllers;

[Authorize]
public class ZonesController : Controller
{
    private readonly ApplicationDbContext _db;
    public ZonesController(ApplicationDbContext db) => _db = db;

    public async Task<IActionResult> Index()
    {
        var zones = await _db.Zones
            .Select(z => new {
                Zone = z,
                SoNongDan = z.Farmers.Count,
                TongDienTich = z.Farmers.Sum(f => f.DienTichCanhTac),
                SoHopDong = z.Farmers.SelectMany(f => f.Contracts).Count()
            })
            .ToListAsync();
        ViewBag.Stats = zones.ToDictionary(
            z => z.Zone.Id,
            z => new { z.SoNongDan, z.TongDienTich, z.SoHopDong });
        return View(zones.Select(z => z.Zone).ToList());
    }

    public IActionResult Create() => View(new Zone());

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Zone zone)
    {
        if (ModelState.IsValid)
        {
            _db.Zones.Add(zone);
            await _db.SaveChangesAsync();
            TempData["Success"] = "Thêm vùng nguyên liệu thành công!";
            return RedirectToAction(nameof(Index));
        }
        return View(zone);
    }

    public async Task<IActionResult> Edit(int id)
    {
        var zone = await _db.Zones.FindAsync(id);
        if (zone == null) return NotFound();
        return View(zone);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Zone zone)
    {
        if (id != zone.Id) return NotFound();
        if (ModelState.IsValid)
        {
            _db.Update(zone);
            await _db.SaveChangesAsync();
            TempData["Success"] = "Cập nhật vùng thành công!";
            return RedirectToAction(nameof(Index));
        }
        return View(zone);
    }

    public async Task<IActionResult> Details(int id)
    {
        var zone = await _db.Zones
            .Include(z => z.Farmers).ThenInclude(f => f.Contracts)
            .FirstOrDefaultAsync(z => z.Id == id);
        if (zone == null) return NotFound();
        return View(zone);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var zone = await _db.Zones.Include(z => z.Farmers).FirstOrDefaultAsync(z => z.Id == id);
        if (zone == null) return NotFound();
        if (zone.Farmers.Any())
        {
            TempData["Error"] = "Không thể xóa vùng đang có nông dân!";
            return RedirectToAction(nameof(Index));
        }
        _db.Zones.Remove(zone);
        await _db.SaveChangesAsync();
        TempData["Success"] = "Xóa vùng thành công!";
        return RedirectToAction(nameof(Index));
    }
}
