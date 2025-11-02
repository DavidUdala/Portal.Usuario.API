using MediatR;
using Portal.Usuario.Application.InputModels;
using Portal.Usuario.Application.OutputModels;
using Portal.Usuario.Core.Entities;
using Portal.Usuario.Core.Interfaces;

namespace Portal.Usuario.Application.Handlers
{
    public class CreateUserHandler : IRequestHandler<PostUserInput, RequestResult<bool>>
    {
        private readonly IApplicationDbRepository<User> _repository;

        public CreateUserHandler(IMediator mediator, IApplicationDbRepository<User> repository)
        {
            _repository = repository;
        }

        public async Task<RequestResult<bool>> Handle(PostUserInput request, CancellationToken cancellationToken)
        {
            try
            {
                var validation = await Validate(request);
                if (!string.IsNullOrEmpty(validation))
                    return new RequestResult<bool>(false, validation);

                User user = request;

                await _repository.Create(user);
                await _repository.Save();

                return new RequestResult<bool>(true, "Cadastrado realizado com sucesso!", true);
            }
            catch (Exception ex)
            {
                return new RequestResult<bool>(false, ex.Message);
            }
        }

        protected async Task<string> Validate(PostUserInput request)
        {
            var user = await _repository.GetOne(usr => usr.Email.ToUpper().Trim() == request.Email.Trim().ToUpper());

            if (user is not null)
                return "Email já cadastrado";

            return "";
        }
    }
}
