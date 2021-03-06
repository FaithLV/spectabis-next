using System.Collections.Generic;
using System.Linq;
using SpectabisUI.Factories;
using SpectabisUI.Interfaces;

namespace SpectabisNext.Repositories
{
    public class PageRepository : IPageRepository
    {
        private readonly PageFactory _pageFactory;

        private List<IPage> Pages { get; }

        public PageRepository(PageFactory pageFactory)
        {
            Pages = new List<IPage>();
            _pageFactory = pageFactory;
        }

        public IEnumerable<IPage> All => Pages;

        /// <summary>
        /// Return an instance of a page from repository. If page does not exist yet, one is created.
        /// </summary>
        public IPage GetPage<T>()
        {
            var page = Pages.SingleOrDefault(x => x.GetType() == typeof(T));

            if (page != null)
            {
                if (page.ReloadOnNavigation)
                {
                    page = _pageFactory.Create<T>();
                }

                return page;
            }

            page = _pageFactory.Create<T>();
            Pages.Add(page);

            return page;
        }
    }
}