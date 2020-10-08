using System.ComponentModel.DataAnnotations;

namespace SkiTickets.Models
{
    public class AgeDto
    {
        public int Id { set; get; }
        [Required]
        [StringLength(100)]
        public string Type { set; get; }
        [Required]
        [Range(6, 100)]
        public int MinYears { set; get; }
        [Range(6, 100)]
        public int MaxYears { set; get; }
    }
}