using System.Collections.Generic;

namespace SkiTickets.Utils
{
    public class Error
    {
        public string Type { set; get; }
        public string Title { set; get; }
        public int Status { set; get; }
        public string TraceId { set; get; }
        public List<string> Errors { set; get; }
    }
}