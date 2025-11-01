using MediatR;
using Portal.Usuario.Application.OutputModels;
namespace Portal.Usuario.Application.InputModels
{
    public class GetUsersByInput : IRequest<RequestResult<PagedResult<List<UserOutput>>>>
    {
        public string Term { get; set; } = string.Empty;
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 5;

        public GetUsersByInput(string term, int pageNumber, int pageSize)
        {
            Term = term;
            PageNumber = pageNumber;
            PageSize = pageSize;
        }
    }
}
