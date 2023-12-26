using HW_4.Data.Entities;
using HW_4.Services.Hash;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HW_4.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly DataContext _dataContext;
        private readonly IHashService _hashService;

        public AuthController(DataContext dataContext, IHashService hashService)
        {
            _dataContext = dataContext;
            _hashService = hashService;
        }

        [HttpGet]
        public object Authenticate(String login, String password)
        {
            var user = _dataContext
                .Users.Where(u => u.DeleteDt == null).FirstOrDefault(u => u.Login == login);
            if (user == null)
            {
                HttpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return new { status = "Credentials rejected" };
            }
            String dk = _hashService.HexString(user.PasswordSalt + password);
            if (user.PasswordDk != dk)
            {
                HttpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return new { status = "Credentials rejected" };
            }
            HttpContext.Session.SetString("AuthUserId", user.Id.ToString());
            return new { status = "OK" };
        }

        [HttpDelete]
        public object UserSignOut()
        {
            if (HttpContext.Session.GetString("AuthUserId") != null)
            {
                HttpContext.Response.StatusCode = StatusCodes.Status200OK;
               
                HttpContext.Session.Remove("AuthUserId");
                return new { status = "User is sign out" };
            }

            HttpContext.Response.StatusCode = StatusCodes.Status503ServiceUnavailable;
            return new { status = "Server is unavailable" };
        }
    }
}
