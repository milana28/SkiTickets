using System.ComponentModel.DataAnnotations;

namespace SkiTickets.Models
{
    public class SellingPointDto
    {
        public int Id { set; get; }
        [Required]
        [StringLength(100)]
        public string Name { set; get; }
        [Required]
        [StringLength(100)]
        public string Location { set; get; }
    }
}