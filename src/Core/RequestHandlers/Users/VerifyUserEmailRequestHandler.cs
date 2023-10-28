using Core.Commands.Users;
using Core.Dtos.Users;
using Core.Repositories.Users;
using MediatR;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace Core.RequestHandlers.Users
{
    public class VerifyUserEmailRequestHandler : IRequestHandler<VerifyUserEmailCommand, bool>
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;

        public VerifyUserEmailRequestHandler(IUserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;
        }

        public async Task<bool> Handle(VerifyUserEmailCommand request, CancellationToken cancellationToken)
        {

            try
            {
                var secretKey = _configuration["SecretTokenKey"]!;
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
                var tokenHandler = new JwtSecurityTokenHandler();
                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = securityKey,
                    ValidateIssuer = true,
                    ValidIssuer = "localhost",
                    ValidateAudience = false,
                    ValidAudience = null,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };

                SecurityToken validatedToken;

                tokenHandler.ValidateToken(request.Token, validationParameters, out validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var username = jwtToken.Claims.First(x => x.Type == "username").Value;

                if (string.IsNullOrWhiteSpace(username))
                {
                    throw new ArgumentNullException(nameof(username));
                }

                var user = await _userRepository.GetUserByUsernameAsync(username);

                if (user == null)
                {
                    throw new ArgumentNullException(nameof(username));
                }

                user.EmailVerified = true;

                await _userRepository.UpdateUserAsync(user);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
