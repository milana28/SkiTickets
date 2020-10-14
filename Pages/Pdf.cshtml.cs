using Microsoft.AspNetCore.Mvc.RazorPages;
using SkiTickets.Models;

namespace SkiTickets.Pages
{
    public class Pdf : PageModel
    {
        public Ticket Ticket { get; }

        public Pdf(Ticket ticket)
        {
            Ticket = ticket;
        }
    }
}