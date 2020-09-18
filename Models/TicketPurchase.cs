using System;

namespace SkiTickets.Models
{
    public class TicketPurchase
    {
        public int Id { set; get; }
        public string Ticket { set; get; }
        public string FirstName { set; get; }
        public string LastName { set; get; }
        public string Age { set; get; }
        public DateTime Date { set; get; }
        public float Price { set; get; }
    }
}