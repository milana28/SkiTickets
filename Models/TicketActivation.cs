using System;

namespace SkiTickets.Models
{
    public class TicketActivation
    {
        public int Id { set; get; }
        public int TicketPurchaseId { set; get; }
        public bool IsActive { set; get; }
        public DateTime DateOfActivation { set; get; }
    }
}