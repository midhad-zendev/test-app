using System.Text.Json.Serialization;

namespace Example.Api.Models
{
    public class ErrorResponse
    {
        public string Title { get; set; }

        public string Description { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string Details { get; set; }
    }
}
