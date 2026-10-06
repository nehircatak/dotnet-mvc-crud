using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DotNet.Data;
using DotNet.Models;

namespace DotNet.Controllers
{
    public class HesapController : Controller
    {
        private readonly UygulamaDbContext _context;

        public HesapController(UygulamaDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Kayit() => View();

        [HttpPost]
        public async Task<IActionResult> Kayit(KullaniciModel model)
        {
            if (ModelState.IsValid)
            {
                var varMi = await _context.Kullanicilar.AnyAsync(x => x.Email == model.Email);
                if (varMi)
                {
                    ModelState.AddModelError("Email", "Bu e-posta adresi zaten kayıtlı.");
                    return View(model);
                }

                _context.Kullanicilar.Add(model);
                await _context.SaveChangesAsync();

                TempData["Mesaj"] = "Kayıt başarılı! Şimdi giriş yapabilirsiniz.";
                return RedirectToAction("Giris");
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Giris() => View();

        [HttpPost]
        public async Task<IActionResult> Giris(KullaniciModel model)
        {
            var kullanici = await _context.Kullanicilar
                .FirstOrDefaultAsync(x => x.Email == model.Email && x.Sifre == model.Sifre);

            if (kullanici != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, kullanici.Id.ToString()),
                    new Claim(ClaimTypes.Name, kullanici.Email),
                    new Claim(ClaimTypes.Email, kullanici.Email)
                };

                var identity = new ClaimsIdentity(claims, "CookieAuth");
                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync("CookieAuth", principal);

                TempData["Mesaj"] = "Hoş geldiniz! Giriş başarılı.";
                return RedirectToAction("Liste", "Home");
            }

            ViewBag.Hata = "E-posta veya şifre hatalı!";
            return View(model);
        }

        public async Task<IActionResult> Cikis()
        {
            await HttpContext.SignOutAsync("CookieAuth");
            return RedirectToAction("Giris");
        }
    }
}