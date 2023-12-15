using HW_4.Data.Entities;
using System.Security.Claims;

namespace HW_4.Middleware
{
    public class AuthSessionMiddleware
    {
        private readonly RequestDelegate _next;

        public AuthSessionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, DataContext _dataContext)
        {
            if(context.Session.Keys.Contains("AuthUserId"))
            {
                var user = _dataContext
                    .Users
                    .Find(Guid.Parse(context.Session.GetString("AuthUserId")!));
                if(user != null)
                {
                    Claim[] claims = new Claim[]
                    {
                        new Claim(ClaimTypes.Sid, user.Id.ToString()),
                        new(ClaimTypes.Name, user.Fullname??""),
                        new(ClaimTypes.Email, user.Email),
                        new(ClaimTypes.UserData, user.Avatar??"")
                    };
                    context.User = new ClaimsPrincipal(
                        new ClaimsIdentity(claims, nameof(AuthSessionMiddleware))
                        );
                }
            }
            await _next(context);
        }
    }
}
