using Microsoft.AspNetCore.Mvc;

namespace QAPDashboard.Views.Shared.Components.SearchBar
{
    public class SearchBarViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(SearchBarViewModel searchBarViewModel)
        {
            return View("SearchBar", searchBarViewModel);
        }
    }
}
