using System;
using System.ComponentModel.DataAnnotations;

namespace Example.Api.Models
{

    public class RegisterRequest
    {
        [Required]
        [StringLength(30, MinimumLength = 3)]
        public string Username { get; set; }

        [Required]
        [StringLength(30, MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
    }

    public class RegisterResponse
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
