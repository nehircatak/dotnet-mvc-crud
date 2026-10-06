using System.ComponentModel.DataAnnotations;

namespace DotNet.Models
{
    public class EgitimModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Lütfen öğrenci adını boş bırakmayınız.")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Öğrenci adı 2 ile 50 karakter arasında olmalıdır.")]
        [Display(Name = "Öğrenci Adı")]
        public string OgrenciAd { get; set; } = string.Empty;

        [Required(ErrorMessage = "Lütfen tamamlanan gün sayısını giriniz.")]
        [Range(1, 365, ErrorMessage = "Gün sayısı 1 ile 365 arasında bir değer olmalıdır.")]
        [Display(Name = "Tamamlanan Gün")]
        public int GunSayisi { get; set; }

        [Required(ErrorMessage = "Lütfen öğrenilen konuyu belirtiniz.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Öğrenilen konu en az 3, en fazla 100 karakter olabilir.")]
        [Display(Name = "Öğrenilen Konu")]
        public string TamamlananKonu { get; set; } = string.Empty;
    }
}