using System.Security.Claims;
using ALEXforums.Data;

namespace ALEXforums.Middleware
{
    public class SessionAuthMiddleware
    {

        private readonly RequestDelegate _next;

        public SessionAuthMiddleware(RequestDelegate next)
        {
            _next = next;
        }

  
        public async Task InvokeAsync(HttpContext context, DataContext dataContext)
        {
            if (context.Session.Keys.Contains("AuthUserId"))
            {

                var user = dataContext.Users.Find(Guid.Parse(
                    context.Session.GetString("AuthUserId")!));
                if (user != null)
                {

                    Claim[] claims = new Claim[]
                    {
                        new Claim(ClaimTypes.NameIdentifier, user.Login),
                        new Claim(ClaimTypes.Sid, user.Id.ToString()),
                        new Claim(ClaimTypes.UserData, user.Avatar ?? "")
                    };

                    context.User =
                        new ClaimsPrincipal(
                            new ClaimsIdentity(
                                claims, nameof(SessionAuthMiddleware)));
                   
                }
            }

            await _next(context);   
        }
    }

    public static class SessionAuthMiddlewareExtension
    {
        public static IApplicationBuilder
            UseSessionAuth(this IApplicationBuilder app)
        {
            return app.UseMiddleware<SessionAuthMiddleware>();
        }
    }
}
