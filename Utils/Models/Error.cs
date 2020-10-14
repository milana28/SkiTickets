using System.Collections.Generic;
using System.Net;

namespace SkiTickets.Utils
{
    public class Error
    {
        public string Type { set; get; }
        public string Title { set; get; }
        public HttpStatusCode Status { set; get; }
        public string TraceId { set; get; }
        public List<string> Errors { set; get; }
    }
}