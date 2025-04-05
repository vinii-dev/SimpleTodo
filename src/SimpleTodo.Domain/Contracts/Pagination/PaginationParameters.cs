namespace SimpleTodo.Domain.Contracts.Pagination;

public record PaginationParameters(
    int Page = 1,
    int PageSize = 10);
