using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Example.Db.Interfaces;

namespace Example.Api.Controllers
{
    [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
    public class BaseController : BaseControllerWithoutAuthorization
    {
        public BaseController(IDbContext dbContext) : base(dbContext)
        {
        }
    }

    public class BaseControllerWithoutAuthorization : ControllerBase
    {
        private readonly IDbContext _dbContext;
        private Example.Db.Entities.User _user;
        public BaseControllerWithoutAuthorization(IDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        protected Example.Db.Entities.User AuthUser
        {
            get
            {
                if (_user == null)
                {
                    int.TryParse(User.Claims.First(q => q.Type == "id").Value, out var userId);

                    _user = _dbContext.UsersService.GetById(userId);

                    HttpContext.Items["Username"] = _user.Username;
                }

                return _user;
            }
        }
    }
}
