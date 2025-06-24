using TradeAppSharedKernel.Infrastructure.Helpers.TokenService;

namespace TradeAppAPI.Middleware
{
    public class TokenPropagationMiddleware
    {
        private readonly RequestDelegate _next;

        public TokenPropagationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, ITokenService tokenService)
        {
            var token = context.Request.Headers["Authorization"].ToString()?.Replace("Bearer ", "");

            if (!string.IsNullOrEmpty(token))
            {
                tokenService.Token = token;
            }

            await _next(context);
        }
    }
}
