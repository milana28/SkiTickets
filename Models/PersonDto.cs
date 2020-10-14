using System.ComponentModel.DataAnnotations;

namespace SkiTickets.Models
{
    public class AgeInfo
    {
        [Required]
        public string Age { set; get; }
    }
    public class PersonDto : AgeInfo
    {
        public int Id { set; get; }
        [Required]
        [StringLength(100)]
        public string FirstName { set; get; }
        [Required]
        [StringLength(100)]
        public string LastName { set; get; }
    }
}