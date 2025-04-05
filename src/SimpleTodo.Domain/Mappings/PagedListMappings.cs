using SimpleTodo.Domain.Common;

namespace SimpleTodo.Domain.Mappings;

public static class PagedListMappings
{
    public static PagedList<K> Map<T, K>(this PagedList<T> paged, Converter<T, K> converter)
    {
        var convertedItems = paged.Items.ConvertAll(converter);

        return new PagedList<K>(
            convertedItems, paged.CurrentPage, paged.PageSize, paged.TotalCount);
    }
}
