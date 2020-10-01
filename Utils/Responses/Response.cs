namespace SkiTickets.Models
{
    public class Response<T>
    {
        public T Data { set; get; }
        public T Metadata { set; get; }
        
        public Response(T response)
        {
            Data = response;
            Metadata = default(T);
        }
        
    }
}