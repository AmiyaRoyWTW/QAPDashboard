using Microsoft.AspNetCore.Mvc;

namespace QAPDashboard.Views.Shared.Components.Checkbox
{
    public class CheckboxViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(CheckboxViewModel checkboxViewModel)
        {
            return View("Checkbox", checkboxViewModel);
        }
    }
}
