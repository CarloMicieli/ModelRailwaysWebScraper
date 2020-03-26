using System;
using System.Collections.Immutable;
using System.Linq;

namespace WebScraper.Scraping.Model
{
    public sealed class Pagination
    {
        public Page CurrentPage { get; }
        public Page NextPage { get; }
        public Page PreviousPage { get; }

        public Pagination(Page currentPage, Page nextPage, Page previousPage)
        {
            CurrentPage = currentPage;
            NextPage = nextPage;
            PreviousPage = previousPage;
        }

        public static Pagination Of(ImmutableList<Page> pages, int pageNumber)
        {
            Page current = pages.Where(it => it.PageNumber == pageNumber).FirstOrDefault();
            Page prev = pages.TakeWhile(it => it.PageNumber < pageNumber).LastOrDefault();
            Page next = pages.SkipWhile(it => it.PageNumber <= pageNumber).FirstOrDefault();
            return new Pagination(current, next, prev);
        }
    }

    public sealed class Page
    {
        public int PageNumber { set; get; }
        public Uri PageLink { set; get; }
        public bool IsActive { set; get; }

        private Page()
        {
        }

        public static bool TryCreate(string sPage, string sUri, bool isActive, out Page result)
        {
            if (int.TryParse(sPage, out var page))
            {
                if (Uri.TryCreate(sUri, UriKind.RelativeOrAbsolute, out var link))
                {
                    result = new Page
                    {
                        PageNumber = page,
                        PageLink = link,
                        IsActive = isActive
                    };
                    return true;
                }
            }

            result = default;
            return false;
        }
    }
}
