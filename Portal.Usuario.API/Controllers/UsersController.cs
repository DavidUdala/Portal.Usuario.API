using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Portal.Usuario.Application.InputModels;
using Portal.Usuario.Application.OutputModels;

namespace Portal.Usuario.API.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: api/<UsersController>
        [HttpGet("getBy")]
        [Authorize]
        public async Task<RequestResult<PagedResult<List<UserOutput>>>> Get([FromQuery] string term = "", int pageNumber = 1, int pageSize = 5)
        {
            var input = new GetUsersByInput(term, pageNumber, pageSize);
            var result = await _mediator.Send(input);

            return result;
        }

        // POST api/<UsersController>
        [HttpPost("create")]
        public async Task<RequestResult<bool>> Post([FromBody] PostUserInput input)
        {
            var result = await _mediator.Send(input);

            return result;
        }
    }
}
