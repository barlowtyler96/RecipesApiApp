namespace RecipeLibrary.DataAccess;

public class PaginationResponse<T> where T : class
{
    public int TotalCount { get; set; }

    public int PageSize { get; set; }

    public int CurrentPageNumber { get; set; }

    public int TotalPages { get; set; }

    public T Data { get; set; }

    public PaginationResponse(int totalCount, int pageSize, int currentPageNumber, T data)
    {
        TotalCount = totalCount;
        CurrentPageNumber = currentPageNumber;
        PageSize = pageSize;
        Data = data;

        TotalPages = (int)Math.Ceiling((double)TotalCount / (double)PageSize);
    }
}
