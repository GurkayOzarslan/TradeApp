using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace TradeAppSharedKernel.Infrastructure.Helpers.TokenInfo
{
    public class TokenInfoHandler : ITokenInfoHandler
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TokenInfoHandler(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public TokenInfoResponse TokenInfo()
        {
            var claims = _httpContextAccessor.HttpContext?.User?.Claims?.ToList() ?? new List<Claim>();
            var response = new TokenInfoResponse
            {
                NameIdentifier = int.TryParse(claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value, out var nameId) ? nameId : 0,
                Email = claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value,
                Username = claims.FirstOrDefault(c => c.Type == "Username")?.Value,
                Name = claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value,
                MiddleName = claims.FirstOrDefault(c => c.Type == "MiddleName")?.Value,
                Surname = claims.FirstOrDefault(c => c.Type == ClaimTypes.Surname)?.Value,
                NotBefore = long.TryParse(claims.FirstOrDefault(c => c.Type == "nbf")?.Value, out var nbf) ? nbf : (long?)null,
                Expires = long.TryParse(claims.FirstOrDefault(c => c.Type == "exp")?.Value, out var exp) ? exp : (long?)null,
                IssuedAt = long.TryParse(claims.FirstOrDefault(c => c.Type == "iat")?.Value, out var iat) ? iat : (long?)null,
                Issuer = claims.FirstOrDefault(c => c.Type == "iss")?.Value,
                Audience = claims.FirstOrDefault(c => c.Type == "aud")?.Value,
                Roles = GetRoles()
            };

            return response;
        }


        public List<string> GetRoles()
        {
            return _httpContextAccessor.HttpContext?.User?
                .Claims
                .Where(c => c.Type == ClaimTypes.Role)
                .Select(c => c.Value)
                .ToList() ?? new List<string>();
        }
    }
}
