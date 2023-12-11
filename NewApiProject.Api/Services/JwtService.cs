using Microsoft.IdentityModel.Tokens;
using NewApiProject.Api.Entites;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace NewApiProject.Api.Services
{
    public static class JwtService
    {

        public static string CreateJwtTOKEN(User user) 
        {

            ///Creater of token
            var jwtTokenHandler = new JwtSecurityTokenHandler();

            //Key for token
            var key = Encoding.UTF8.GetBytes("veryveryverysecretkeythatnooneknows");

            //Payload or info from user
            var identity = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Role, user.Role),
                new Claim(ClaimTypes.Name, user.Username)
            });

            //The secret code encoded
            var credentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);

            //base of token
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = identity,
                Expires = DateTime.Now.AddHours(1),
                SigningCredentials = credentials
            };

            //final token to return
            var token = jwtTokenHandler.CreateToken(tokenDescriptor);

            //return token
            return jwtTokenHandler.WriteToken(token);
        }
    }
}
