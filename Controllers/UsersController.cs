using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BTL_backend.Data;
using BTL_backend.Models;

namespace BTL_backend.Controllers;

[Authorize(Roles = "Admin")]
public class UsersController : Controller
{
    private readonly ApplicationDbContext _db;
    public UsersController(ApplicationDbContext db) => _db = db;

    public async Task<IActionResult> Index()
    {
        return View(await _db.Users.OrderBy(u => u.HoTen).ToListAsync());
    }

    public IActionResult Create() => View();

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateUserViewModel model)
    {
        if (await _db.Users.AnyAsync(u => u.TenDangNhap == model.TenDangNhap))
            ModelState.AddModelError("TenDangNhap", "Tên đăng nhập đã tồn tại!");

        if (ModelState.IsValid)
        {
            var user = new User
            {
                TenDangNhap = model.TenDangNhap,
                MatKhau = ApplicationDbContext.HashPassword(model.MatKhau),
                HoTen = model.HoTen,
                Email = model.Email,
                SoDienThoai = model.SoDienThoai,
                VaiTro = model.VaiTro,
                HoatDong = true,
                NgayTao = DateTime.Now
            };
            _db.Users.Add(user);
            await _db.SaveChangesAsync();
            TempData["Success"] = $"Thêm tài khoản \"{user.TenDangNhap}\" thành công!";
            return RedirectToAction(nameof(Index));
        }
        return View(model);
    }

    public async Task<IActionResult> Edit(int id)
    {
        var user = await _db.Users.FindAsync(id);
        if (user == null) return NotFound();
        return View(user);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, User user)
    {
        if (id != user.Id) return NotFound();

        var existing = await _db.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id);
        if (existing == null) return NotFound();

        if (await _db.Users.AnyAsync(u => u.TenDangNhap == user.TenDangNhap && u.Id != id))
            ModelState.AddModelError("TenDangNhap", "Tên đăng nhập đã tồn tại!");

        // Keep existing password (don't overwrite)
        user.MatKhau = existing.MatKhau;

        // Remove password validation for edit
        ModelState.Remove("MatKhau");

        if (ModelState.IsValid)
        {
            _db.Update(user);
            await _db.SaveChangesAsync();
            TempData["Success"] = "Cập nhật tài khoản thành công!";
            return RedirectToAction(nameof(Index));
        }
        return View(user);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> ResetPassword(ChangePasswordViewModel model)
    {
        var user = await _db.Users.FindAsync(model.UserId);
        if (user == null) return NotFound();

        if (!ModelState.IsValid)
        {
            TempData["Error"] = "Mật khẩu mới tối thiểu 4 ký tự!";
            return RedirectToAction(nameof(Edit), new { id = model.UserId });
        }

        user.MatKhau = ApplicationDbContext.HashPassword(model.MatKhauMoi);
        await _db.SaveChangesAsync();
        TempData["Success"] = $"Đặt lại mật khẩu cho \"{user.TenDangNhap}\" thành công!";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> ToggleActive(int id)
    {
        var user = await _db.Users.FindAsync(id);
        if (user == null) return NotFound();

        // Don't let admin deactivate themselves
        var currentUserId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? "0");
        if (user.Id == currentUserId)
        {
            TempData["Error"] = "Không thể khóa tài khoản của chính mình!";
            return RedirectToAction(nameof(Index));
        }

        user.HoatDong = !user.HoatDong;
        await _db.SaveChangesAsync();
        TempData["Success"] = user.HoatDong
            ? $"Đã kích hoạt tài khoản \"{user.TenDangNhap}\""
            : $"Đã khóa tài khoản \"{user.TenDangNhap}\"";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var user = await _db.Users.FindAsync(id);
        if (user == null) return NotFound();

        var currentUserId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? "0");
        if (user.Id == currentUserId)
        {
            TempData["Error"] = "Không thể xóa tài khoản của chính mình!";
            return RedirectToAction(nameof(Index));
        }

        _db.Users.Remove(user);
        await _db.SaveChangesAsync();
        TempData["Success"] = $"Xóa tài khoản \"{user.TenDangNhap}\" thành công!";
        return RedirectToAction(nameof(Index));
    }
}
