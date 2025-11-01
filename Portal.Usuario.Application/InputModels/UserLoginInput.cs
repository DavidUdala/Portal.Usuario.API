using MediatR;
using Portal.Usuario.Application.OutputModels;

namespace Portal.Usuario.Application.InputModels
{
    public class UserLoginInput : IRequest<RequestResult<string>>
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

}
