using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DaySix.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        public ICollection<Blog> Blogs { get; set; }
    }
}
