using System;
using System.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using API.Entities;
using API.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace API.Services;

public class TokenService(IConfiguration config) : ITokenService
{
    public string CreateToken(AppUser user)
    {
        var tokenKey = config["TokenKey"]?? throw new Exception("Cannot access tokenKey from appsettings");
        if(tokenKey.Length<64) throw new Exception("The key neeeds to be longer");

         var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey));

         var clsims = new List<Claim>{
            // new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.NameIdentifier, user.UserName) 
         };

         var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

         
          var tokenDescriptor = new SecurityTokenDescriptor
          {
            Subject = new ClaimsIdentity(clsims),
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials = creds
          };

          var tokenHandler = new JwtSecurityTokenHandler();
          var token = tokenHandler.CreateToken(tokenDescriptor);

          return tokenHandler.WriteToken(token);


    }
    
}
