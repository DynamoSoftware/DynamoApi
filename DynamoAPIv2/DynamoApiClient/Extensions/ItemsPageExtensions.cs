using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DynamoApiClient.Models;

namespace DynamoApiClient.Extensions
{
    public static class ItemsPageExtensions
    {
        private class PagedEnumerable<T>: IReadOnlyCollection<T> where T: IDictionary<string, object>
        {
            private readonly IEnumerable<T> _enumerable;
            private readonly long? _total;

            public PagedEnumerable(IEnumerable<T> enumerable, long? total)
            {
                _enumerable = enumerable;
                _total = total;
            }

            public IEnumerator<T> GetEnumerator()
            {
                return _enumerable.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return ((IEnumerable) _enumerable).GetEnumerator();
            }

            public int Count => (int)(_total ?? throw new NotSupportedException());
        }

        public static IEnumerable<Item> GetAll(this ItemsPage<ItemListResponse> page)
        {
            IEnumerable<Item> GetEnumerable(ItemsPage<ItemListResponse> pg)
            {
                while (true)
                {
                    if (pg.Response?.Data == null || !pg.Response.Data.Data.Any())
                    {
                        yield break;
                    }

                    pg.Response.ThrowIfErrorResponse();

                    foreach (var item in pg.Response.Data.Data)
                    {
                        yield return item;
                    }

                    if (!pg.HasNext)
                        yield break;

                    pg = pg.GetNext();
                }
            }

            if (page == null) throw new ArgumentNullException(nameof(page));

            var enumerable = GetEnumerable(page);

            return page?.Response?.Data?.Total == null 
                ? enumerable
                : new PagedEnumerable<Item>(enumerable, page.Response.Data.Total.Value);
        }

        public static IEnumerable<DynamoItem> GetAll(this ItemsPage<DynamoItemListResponse> page)
        {
            IEnumerable<DynamoItem> GetEnumerable(ItemsPage<DynamoItemListResponse> pg)
            {
                while (true)
                {
                    if (pg.Response?.Data == null || !pg.Response.Data.Data.Any())
                    {
                        yield break;
                    }

                    pg.Response.ThrowIfErrorResponse();

                    foreach (var item in pg.Response.Data.Data)
                    {
                        yield return item;
                    }

                    if (!pg.HasNext)
                        yield break;

                    pg = pg.GetNext();
                }
            }

            if (page == null) throw new ArgumentNullException(nameof(page));

            var enumerable = GetEnumerable(page);

            return page?.Response?.Data?.Total == null
                ? enumerable
                : new PagedEnumerable<DynamoItem>(enumerable, page.Response.Data.Total.Value);
        }

        public static ItemsPage<T> ThrowIfErrorResponse<T>(this ItemsPage<T> page) where T : ApiResponse, new()
        {
            if (page == null) throw new ArgumentNullException(nameof(page));

            if(!(page.Response.IsSuccessful && page.Response.Data.Success))
                throw new ApiCallResponseException<T>(page.Response);

            return page;
        }
    }
}