using Microsoft.AspNetCore.Mvc;

namespace QAPDashboard.Views.Shared.Components.Dropdown
{
    public class DropdownViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(DropdownViewModel dropdownViewModel)
        {
            return View("Dropdown", dropdownViewModel);
        }
    }

}
