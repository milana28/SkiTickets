using System.ComponentModel.DataAnnotations;

namespace SkiTickets.Models
{
    public class PersonDto
    {
        public int Id { set; get; }
        [Required]
        [StringLength(100)]
        public string FirstName { set; get; }
        [Required]
        [StringLength(100)]
        public string LastName { set; get; }
        [Required]
        public int AgeId { set; get; }
    }
}