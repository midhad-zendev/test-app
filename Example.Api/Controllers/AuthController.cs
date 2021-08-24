using AutoMapper;
using Example.Api.Helper;
using Example.Api.Models;
using Example.Api.Services;
using Example.Db.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Example.Api.Controllers
{
    //[ApiExplorerSettings(IgnoreApi = true)]
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController:BaseControllerWithoutAuthorization
    {
        private readonly IDbContext _dbContext;
        private readonly ILogger<AuthController> _logger;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        public AuthController(IDbContext dbContext,IUserService userService, ILogger<AuthController> logger, IMapper mapper) :base(dbContext)
        {
            _dbContext = dbContext;
            _userService = userService;
            _logger = logger;
            _mapper = mapper;
        }

        /// <summary>
        /// Get a jwt token (supplying username,password) to be used in a request header
        /// </summary>
        /// <param name="model"></param>
        /// <returns>string</returns>
        /// <response code="200">Returns the newly created token</response>
        /// <response code="401">If the user or password combination is wrong</response> 
        [HttpPost("authenticate")]
       // [ApiExplorerSettings(IgnoreApi = false)]
        public IActionResult Authenticate([FromBody] AuthRequest model)
        {
            var response = _userService.Authenticate(model);

            if (response == null)
                return Unauthorized();

            return new OkObjectResult(response.Token);
        }

        /// <summary>
        /// Creates new user
        /// </summary>
        /// <return>void</return>
        /// <response code="200">Returns the newly created user</response>
        /// <response code="400">If the user is null or user already exists</response> 
        [HttpPost("register")]
        //[Obsolete]
        public IActionResult Register([FromBody] RegisterRequest model)
        {
            try
            {
                // register user
                var response = _userService.Register(model);
                if (response == null)
                    return new BadRequestObjectResult(new { message = "Registration unsuccessful!" });
                
                return new  OkObjectResult(response);
            }
            catch (ApiException ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }


    }
}
