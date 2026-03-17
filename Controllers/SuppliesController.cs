using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using BTL_backend.Data;
using BTL_backend.Models;

namespace BTL_backend.Controllers;

[Authorize]
public class SuppliesController : Controller
{
    private readonly ApplicationDbContext _db;
    public SuppliesController(ApplicationDbContext db) => _db = db;

    public async Task<IActionResult> Index()
    {
        return View(await _db.Supplies.ToListAsync());
    }

    public IActionResult Create() => View(new Supply());

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Supply supply)
    {
        if (ModelState.IsValid)
        {
            _db.Supplies.Add(supply);
            await _db.SaveChangesAsync();
            TempData["Success"] = "Thêm vật tư thành công!";
            return RedirectToAction(nameof(Index));
        }
        return View(supply);
    }

    public async Task<IActionResult> Edit(int id)
    {
        var supply = await _db.Supplies.FindAsync(id);
        if (supply == null) return NotFound();
        return View(supply);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Supply supply)
    {
        if (id != supply.Id) return NotFound();
        if (ModelState.IsValid)
        {
            _db.Update(supply);
            await _db.SaveChangesAsync();
            TempData["Success"] = "Cập nhật vật tư thành công!";
            return RedirectToAction(nameof(Index));
        }
        return View(supply);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var supply = await _db.Supplies.Include(s => s.SupplyDebts).FirstOrDefaultAsync(s => s.Id == id);
        if (supply == null) return NotFound();
        if (supply.SupplyDebts.Any())
        {
            TempData["Error"] = "Không thể xóa vật tư đang được sử dụng!";
            return RedirectToAction(nameof(Index));
        }
        _db.Supplies.Remove(supply);
        await _db.SaveChangesAsync();
        TempData["Success"] = "Xóa vật tư thành công!";
        return RedirectToAction(nameof(Index));
    }
}
