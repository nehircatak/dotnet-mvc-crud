using Microsoft.EntityFrameworkCore;
using DotNet.Models;

namespace DotNet.Data
{
    public class UygulamaDbContext : DbContext
    {
        public UygulamaDbContext(DbContextOptions<UygulamaDbContext> options) : base(options)
        {
        }

        public DbSet<EgitimModel> Egitimler { get; set; } = null!;
        public DbSet<KullaniciModel> Kullanicilar { get; set; } = null!;
    }
}