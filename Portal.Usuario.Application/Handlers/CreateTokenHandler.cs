using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualBasic;
using Portal.Usuario.Application.InputModels;
using Portal.Usuario.Application.OutputModels;
using Portal.Usuario.Core.Entities;
using Portal.Usuario.Core.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Portal.Usuario.Application.Handlers
{
    public class CreateTokenHandler : IRequestHandler<UserLoginInput, RequestResult<string>>
    {
        private readonly IConfiguration _config;
        private readonly IApplicationDbRepository<User> _repository;

        public CreateTokenHandler(IConfiguration config, IApplicationDbRepository<User> repository)
        {
            _config = config;
            _repository = repository;
        }

        public async Task<RequestResult<string>> Handle(UserLoginInput input, CancellationToken cancellationToken)
        {
            User? user = await _repository.GetOne(usr => usr.Email == input.Username && usr.Password == input.Password);

            if (user is null)
                return await Task.FromResult(new RequestResult<string>(true, "Usuário ou senha inválidos"));

            string token = await GenerateToken(user);

            return await Task.FromResult(new RequestResult<string>(true, "Login realizado com sucesso", token));
        }

        protected async Task<string> GenerateToken(User user)
        {
            var jwtSettings = _config.GetSection("Jwt");
            var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]!);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(double.Parse(jwtSettings["ExpireMinutes"]!)),
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256)

                );

            return await Task.FromResult(new JwtSecurityTokenHandler().WriteToken(token));
        }
    }
}
