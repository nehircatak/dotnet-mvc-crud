using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DotNet.Data;
using DotNet.Models;

namespace DotNet.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly UygulamaDbContext _context;

        public HomeController(UygulamaDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(EgitimModel model)
        {
            if (ModelState.IsValid)
            {
                _context.Egitimler.Add(model);
                await _context.SaveChangesAsync();

                TempData["Mesaj"] = "Yeni kayıt başarıyla eklendi!";
                return RedirectToAction("Liste");
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Liste(string aramaMetni)
        {
            ViewBag.MevcutArama = aramaMetni;

            var sorgu = _context.Egitimler.AsQueryable();

            if (!string.IsNullOrEmpty(aramaMetni))
            {
                sorgu = sorgu.Where(e => e.OgrenciAd.ToLower().Contains(aramaMetni.ToLower()) 
                                      || e.TamamlananKonu.ToLower().Contains(aramaMetni.ToLower()));
            }

            var liste = await sorgu.ToListAsync();
            return View(liste);
        }

        [HttpGet]
        public async Task<IActionResult> Detay(int id)
        {
            var egitim = await _context.Egitimler.FirstOrDefaultAsync(x => x.Id == id);
            if (egitim == null)
            {
                return NotFound();
            }
            return View(egitim);
        }

        [HttpGet]
        public async Task<IActionResult> Duzenle(int id)
        {
            var egitim = await _context.Egitimler.FindAsync(id);
            if (egitim == null)
            {
                return NotFound();
            }
            return View(egitim);
        }

        [HttpPost]
        public async Task<IActionResult> Duzenle(EgitimModel model)
        {
            if (ModelState.IsValid)
            {
                _context.Egitimler.Update(model);
                await _context.SaveChangesAsync();

                TempData["Mesaj"] = "Kayıt başarıyla güncellendi!";
                return RedirectToAction("Liste");
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Sil(int id)
        {
            var egitim = await _context.Egitimler.FindAsync(id);
            if (egitim != null)
            {
                _context.Egitimler.Remove(egitim);
                await _context.SaveChangesAsync();

                TempData["Mesaj"] = "Kayıt başarıyla silindi.";
            }
            return RedirectToAction("Liste");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}