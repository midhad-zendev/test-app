using System;

namespace Example.Api.Models
{
    public class HttpResponseException : Exception
    {
        public HttpResponseException(string title, string description, int status = 500, string details = null)
        {
            Status = status;
            Value = new ErrorResponse() { Title = title, Description = description,Details=details};
        }

        public int Status { get; set; }

        public object Value { get; set; }
    }
}
