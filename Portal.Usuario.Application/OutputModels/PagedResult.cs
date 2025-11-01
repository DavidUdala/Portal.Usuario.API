namespace Portal.Usuario.Application.OutputModels
{
    public record PagedResult<T>(int PageNumber, int PageSize, int TotalRecords, T? Data)
    {
        public int TotalPages
            => (int)Math.Ceiling((double)TotalRecords / PageSize);
    }
}
