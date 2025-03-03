using Microsoft.AspNetCore.Mvc;

namespace QAPDashboard.Views.Shared.Components.FilterDropdown
{
    public class FilterDropdownViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(FilterDropdownViewModel filterDropdownViewModel)
        {
            return View("FilterDropdown", filterDropdownViewModel);
        }
    }

}