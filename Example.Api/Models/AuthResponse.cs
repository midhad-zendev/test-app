namespace Example.Api.Models
{
    public class AuthResponse
    {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Surname { get; set; }
            public string Username { get; set; }
            public string Token { get; set; }

            public AuthResponse(Db.Entities.User user, string token)
            {
                Id = user.Id;
                Name = user.Name;
                Surname = user.Surname;
                Username = user.Username;
                Token = token;
            }
    }
}
