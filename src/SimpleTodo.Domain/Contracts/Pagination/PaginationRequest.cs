using Swashbuckle.AspNetCore.Annotations;

namespace SimpleTodo.Domain.Contracts.Pagination;

public record PaginationRequest(
    [SwaggerSchema(Description = "The page number for the paginated resource. ")]
    int Page = 1,

    [SwaggerSchema(Description = "The page size (limit) for the paginated resource. ")]
    int PageSize = 10
) : PaginationParameters(Page, PageSize);
