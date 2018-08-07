using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace DynamoApiClient.Extensions
{
    public static class ItemsPageExtensions
    {
        private class PagedEnumerable: IReadOnlyCollection<IDictionary<string, object>>
        {
            private readonly IEnumerable<IDictionary<string, object>> _enumerable;
            private readonly long? _total;

            public PagedEnumerable(IEnumerable<IDictionary<string, object>> enumerable, long? total)
            {
                _enumerable = enumerable;
                _total = total;
            }

            public IEnumerator<IDictionary<string, object>> GetEnumerator()
            {
                return _enumerable.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return ((IEnumerable) _enumerable).GetEnumerator();
            }

            public int Count => (int)(_total ?? throw new NotSupportedException());
        }

        public static IEnumerable<IDictionary<string, object>> GetAll(this ItemsPage page)
        {
            IEnumerable<IDictionary<string, object>> GetEnumerable(ItemsPage pg)
            {
                while (true)
                {
                    if (pg.Response?.Data == null || !pg.Response.Data.Any())
                    {
                        yield break;
                    }

                    pg.Response.ThrowIfErrorResponse();

                    foreach (var item in pg.Response.Data)
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

            return page?.Response?.Total == null 
                ? enumerable
                : new PagedEnumerable(enumerable, page.Response.Total.Value);
        }

        public static ItemsPage ThrowIfErrorResponse(this ItemsPage page)
        {
            if (page == null) throw new ArgumentNullException(nameof(page));

            if (page.Response == null) throw new ArgumentException("Page has no response data", nameof(page));

            if (!page.Response.Success) throw new Exception(page.Response.Error);

            return page;
        }
    }
}