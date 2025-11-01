using MediatR;
using Portal.Usuario.Application.InputModels;
using Portal.Usuario.Application.OutputModels;
using Portal.Usuario.Core.Entities;
using Portal.Usuario.Core.Interfaces;

namespace Portal.Usuario.Application.Queries
{
    public class GetUsersByQuery : IRequestHandler<GetUsersByInput, RequestResult<PagedResult<List<UserOutput>>>>
    {
        private readonly IApplicationDbRepository<User> _repository;

        public GetUsersByQuery(IApplicationDbRepository<User> repository, IMediator mediator)
        {
            _repository = repository;
        }

        public async Task<RequestResult<PagedResult<List<UserOutput>>>> Handle(GetUsersByInput request, CancellationToken cancellationToken)
        {
            try
            {
                var userList = await _repository.List(t => t.Name.ToUpper().Contains(request.Term.ToUpper()));

                var totalRecords = userList.Count();

                var data = userList
                            .Skip((request.PageNumber) - 1 * request.PageSize)
                            .Take(request.PageSize)
                            .ToList();


                return new RequestResult<PagedResult<List<UserOutput>>>(true, "Consulta realizada com sucesso!", new PagedResult<List<UserOutput>>(request.PageNumber, request.PageSize, totalRecords, data.Select(usr => (UserOutput)usr).ToList()));
            }
            catch (Exception ex)
            {
                return new RequestResult<PagedResult<List<UserOutput>>>(false, ex.Message, new PagedResult<List<UserOutput>>(request.PageNumber, request.PageSize, 0, null));
            }
        }
    }
}
