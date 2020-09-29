using System.ComponentModel.DataAnnotations;

namespace SkiTickets.Models
{
    public class TicketPurchaseDto
    {
        public int Id { set; get; }
        [Required]
        public int TicketId { set; get; }
        [Required]
        [StringLength(200)]
        public string FirstName { set; get; }
        [Required]
        [StringLength(200)]
        public string LastName { set; get; }
        [Required]
        [StringLength(100)]
        public string Age { set; get; }
        [Required]
        public int SellingPointId { set; get; }
    }
}