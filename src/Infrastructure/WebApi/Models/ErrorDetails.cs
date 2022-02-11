using Newtonsoft.Json;

namespace Infrastructure.WebApi.Models
{
    public class ErrorDetails
    {
        public int StatusCode { get; set; }
        public string Error { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
