using Example.Api.Models;
using Example.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using Example.Db.Interfaces;

namespace Example.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersApiController : BaseController
    {
   
        private static readonly string[] LogHeaders = new[]
        {
           "Accept", "Host", "Referer", "User-Agent"
        };

        private readonly ILogger<UsersApiController> _logger;
        private readonly IUserService _userService;

        public UsersApiController(IUserService userService,IDbContext dbContext,ILogger<UsersApiController> logger):base(dbContext)
        {
            _userService = userService;
            _logger = logger;
        }
       // [Obsolete]
        [HttpGet("list")]
        public IEnumerable<User> Get()
        {
           // _logger.LogInformation($"GET Users  {Request.Headers.Where(entry => LogHeaders.Contains(entry.Key)).Dump()}");

            return _userService.UsersAll();
        }
   
    }
}

