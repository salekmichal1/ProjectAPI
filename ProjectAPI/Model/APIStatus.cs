using System.Net;

namespace ProjectAPI.Model
{
    public class APIStatus
    {
        public APIStatus()
        {
            Bledy = new List<string>();
        }
        public bool Akceptacja { get; set; }
        public Object Rezulstat { get; set; }
        public HttpStatusCode KodStanu { get; set; }
        public List<string> Bledy { get; set; }
    }
}
