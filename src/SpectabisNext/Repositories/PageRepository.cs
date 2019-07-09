using System.Collections.Generic;
using System.Linq;
using SpectabisNext.Views;
using SpectabisUI.Controls;

namespace SpectabisNext.Repositories
{
    public class PageRepository
    {
        List<Page> Pages { get; set; }

        public PageRepository(GameLibrary gameLibrary, Settings settingsPage, FirstTimeWizard firstTimeWizard)
        {
            Pages = new List<Page>();

            Pages.Add(gameLibrary);
            Pages.Add(settingsPage);
            Pages.Add(firstTimeWizard);
        }

        public void Add(Page page)
        {
            Pages.Add(page);
        }

        public void Add(IEnumerable<Page> pages)
        {
            Pages.AddRange(pages);
        }

        public IEnumerable<Page> GetAll()
        {
            return Pages;
        }

        public Page GetPage<T>()
        {
            return Pages.Single(x => x.GetType() == typeof(T));
        }
    }
}