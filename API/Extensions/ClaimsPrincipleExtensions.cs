using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace API.Extensions
{
    public static class ClaimsPrincipleExtensions
    {
        public static string GetUsername(this ClaimsPrincipal user)
        {
            var xx = ClaimTypes.Name;
            var sss = ClaimTypes.NameIdentifier;
            var username = user.FindFirstValue(ClaimTypes.Name);

            if (username == null) throw new Exception("Cannot get username from Token1");

            return username;
        }
        
        public static int GetUserId(this ClaimsPrincipal user)
        {
            var userId = int.Parse(user.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? throw new Exception("Cannot get username from Token2"));

            // if (userId == null) throw new Exception("Cannot get username from Token");

            return userId;
        }
    }
}