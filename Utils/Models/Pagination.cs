namespace SkiTickets.Utils.Models
{
    public class Pagination
    {
        public int Offset { set; get; }
        public int Limit { set; get; }
        public int PreviousOffset { set; get; }
        public int NextOffset { set; get; }
        public int CurrentPage { set; get; }
        public int PageCount { set; get; }
        public int TotalCount { set; get; }
    }
} 