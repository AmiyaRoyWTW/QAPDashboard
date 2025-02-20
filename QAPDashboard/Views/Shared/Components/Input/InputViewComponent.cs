using Microsoft.AspNetCore.Mvc;

namespace QAPDashboard.Views.Shared.Components.Input
{
    public class InputViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(InputViewModel input)
        {
            return View("Input", input);
        }
    }
}
