using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DaySix.Models
{
    public class Blog
    {
        public int Id { get; set; }

        [DisplayName("Başlıq")]
        [Required(ErrorMessage = "Başlıq məcburidir")]
        [MaxLength(200,ErrorMessage = "Başlıq maximum 200 xarakter ola bilər")]
        public string Title { get; set; }

        [MaxLength(100)]
        public string Photo { get; set; }

        [DisplayName("Yayınlanma tarixi")]
        [Required(ErrorMessage = "Yayınlanma tarixi məcburidir")]
        public DateTime PublishDate { get; set; }

        [DisplayName("Kateqoriya")]
        [Required(ErrorMessage = "Kateqoriya məcburidir")]
        public int CategoryId { get; set; }

        [DisplayName("Mətn")]
        [Required(ErrorMessage = "Mətn məcburidir")]
        [MinLength(100, ErrorMessage = "Mətn minimum 100 xarakter ola bilər")]
        [Column(TypeName = "ntext")]
        public string Text { get; set; }

        [DisplayName("Şəkil(png,jpg,gif)")]
        [NotMapped]
        public IFormFile Upload { get; set; }

        public Category Category { get; set; }
    }
}
