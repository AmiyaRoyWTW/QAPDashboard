using Microsoft.AspNetCore.Mvc;

namespace QAPDashboard.Views.Shared.Components.Spinner
{
    public class SpinnerViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(SpinnerViewModel spinnerViewModel)
        {
            return View("Spinner", spinnerViewModel);
        }
    }

}