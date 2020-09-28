using System;
using System.ComponentModel.DataAnnotations;

namespace SkiTickets.Models
{
    public class TicketPurchaseDto
    {
        public int Id { set; get; }
        [Required]
        public int TicketId { set; get; }
        [Required]
        public int PersonId { set; get; } 
        [Required]
        public int PlaceId { set; get; }
        [Required]
        public DateTime Date { set; get; }
    }
}