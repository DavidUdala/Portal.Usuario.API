using MediatR;
using Microsoft.AspNetCore.Mvc;
using Portal.Usuario.Application.InputModels;
using Portal.Usuario.Application.OutputModels;

namespace Portal.Usuario.API.Controllers
{
    [Route("api/authentication")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {

        private readonly IMediator _mediator;

        public AuthenticationController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("login")]
        public async Task<RequestResult<string>> Login([FromBody]UserLoginInput input)
        {
            return await _mediator.Send(input);
        }
    }
}