using System.Net;

namespace ProjectAPI.Model
{
    public class APIStatus
    {
        public APIStatus()
        {
            Errors = new List<string>();
        }
        public bool Accept { get; set; }
        public Object Result { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public List<string> Errors { get; set; }
    }
}
