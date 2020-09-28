using System;
using System.ComponentModel.DataAnnotations;

namespace SkiTickets.Models
{
    public class TicketDto
    {
        public int Id { set; get; }
        [Required]
        [StringLength(100)]
        public string TicketType { set; get; }
        [Required]
        [StringLength(100)]
        public string Age { set; get; }
        [Required]
        public float Price { set; get; }
        public DateTime FromDate { set; get; }
        public DateTime ToDate { set; get; }
    }
}