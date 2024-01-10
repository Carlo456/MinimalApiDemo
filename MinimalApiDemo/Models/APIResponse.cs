using System.Net;

namespace MinimalApiDemo.Models
{
    public class APIResponse
    {
        public APIResponse() 
        { 
            ErrorMessages = new List<string>();
        }
        public bool Success { get; set; }
        public Object Result { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public List<string> ErrorMessages { get; set; }
    }
}
