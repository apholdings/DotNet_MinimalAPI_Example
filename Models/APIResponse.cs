using System.Net;

namespace DotNet_MinimalAPI_Example.Models
{
    public class APIResponse
    {

        public APIResponse() { 
            ErrorMessages= new List<string>();
        }

        public bool IsSuccess { get; set; } = true;
        public Object Result { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public List<string> ErrorMessages { get; set; }
    }
}
