namespace Core.Utilities
{
    using System;
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Text;
    using Microsoft.IdentityModel.Tokens;

    public class JwtTokenGenerator
    {
        private readonly IConfiguration _configuration;
        public JwtTokenGenerator(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string GenerateToken(string username)
        {
            var secretKey = _configuration["SecretTokenKey"]!;
            var expiryDurationMinutes = 30;

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var permClaims = new List<Claim>
            {
                new Claim("username", username)
            };

            var token = new JwtSecurityToken(
                issuer: "localhost",
                audience: null,
                claims: permClaims,
                expires: DateTime.Now.AddMinutes(expiryDurationMinutes),
                signingCredentials: credentials
            );

            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            var stringToken = jwtSecurityTokenHandler.WriteToken(token);
            return stringToken;
        }
    }

}
