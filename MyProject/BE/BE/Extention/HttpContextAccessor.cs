using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BE.Extention
{
    public static class HttpContextAccessor
    {
        public static int UserId(this IHttpContextAccessor httpContextAccessor)
        {
            var claims = httpContextAccessor.HttpContext.User.Claims;
            var userId = claims.FirstOrDefault(m => m.Type == ClaimTypes.NameIdentifier).Value;
            return Convert.ToInt32(userId);
        }

        public static string Role(this IHttpContextAccessor httpContextAccessor)
        {
            var claims = httpContextAccessor.HttpContext.User.Claims;
            var role = claims.FirstOrDefault(m => m.Type == ClaimTypes.Role).Value;
            return role;
        }
    }
}
