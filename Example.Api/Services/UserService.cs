using AutoMapper;
using Example.Api.Models;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Example.Db.Interfaces;
using Example.Api.Helper;

namespace Example.Api.Services
{
    public interface IUserService
    {
        AuthResponse Authenticate(AuthRequest model);

        RegisterResponse Register(RegisterRequest model);

        AuthResponse Login(AuthRequest model);

        List<Models.User> UsersAll();

        IEnumerable<Db.Entities.User> GetAll();
    }

    public class UserService : IUserService
    {
        private readonly IDbContext _dbContext;
        private readonly AppSettingsModel _appSettings;
        private readonly IMapper _mapper;
        public UserService(IDbContext dbContext, AppSettingsModel appSettings,IMapper mapper)
        {
            _dbContext = dbContext;
            _appSettings = appSettings;
            _mapper = mapper;
        }
        //Used by Api
        public AuthResponse Authenticate(AuthRequest model)
        {
            try
            {
                var user = _dbContext.UsersService.GetByUsername(model.Username);

                // return null if user not found
                if (user == null)
                {
                    return null;
                }

                // return null if passwords does not match
                var passwordHash = Hasher.ComputeHash(model.Password, user.Salt);
                if (user != null && !user.Password.Equals(passwordHash))
                {
                    return null;
                }

                // authentication successful so generate jwt token
                var token = GenerateJwtToken(user);

                return new AuthResponse(user, token);
            }
            catch (Exception)
            {
                //log error if required
                return null;
            }
     
        }
        public IEnumerable<Db.Entities.User> GetAll()
        {
            try
            {
                return _dbContext.UsersService.All();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public AuthResponse Login(AuthRequest model)
        {
            try
            {
                var user = _dbContext.UsersService.GetByUsername(model.Username);

                // return null if user not found
                if (user == null)
                {
                    return null;
                }

                // return null if passwords does not match
                var passwordHash = Hasher.ComputeHash(model.Password, user.Salt);
                if (user != null && !user.Password.Equals(passwordHash))
                {
                    return null;
                }
                return new AuthResponse(user, null);
            }
            catch (Exception)
            {
                return null;
            }
      
        }

        public RegisterResponse Register(RegisterRequest model)
        {
            try
            {
                var existing = _dbContext.UsersService.GetByUsername(model.Username);
                if (string.IsNullOrEmpty(model.Username) || string.IsNullOrEmpty(model.Password) || existing != null)
                    return null;

                var user = _mapper.Map<Db.Entities.User>(model);
                user.Salt = Hasher.GetSalt();
                user.CreateDate = DateTime.UtcNow;
                user.Password = Hasher.ComputeHash(model.Password, user.Salt);
                _dbContext.UsersService.Insert(user);
                return _mapper.Map<RegisterResponse>(user);
            }
            catch (Exception)
            {
                return null;
            }
          
        }
        public List<Models.User> UsersAll()
        {
            try
            {
                var users = _dbContext.UsersService.All();
                return _mapper.Map<List<Models.User>>(users);
            }
            catch (Exception)
            {
                return null;
            }
            
        }


        // helper methods
        private string GenerateJwtToken(Db.Entities.User user)
        {
            try
            {
                // generate token that is valid for 7 days
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_appSettings.AppSettings.TokenAuthenticationConfig.SecretKey);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                    new Claim("id", user.Id.ToString()),
                    new Claim("username", user.Username.ToString()),
                    new Claim(ClaimTypes.Role, "User"),
                    }),
                    Expires = DateTime.UtcNow.AddHours(_appSettings.AppSettings.TokenAuthenticationConfig.ExpiresAfterHours),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                return tokenHandler.WriteToken(token);
            }
            catch (Exception)
            {
                return null;
            }
        
        }
    }
}
