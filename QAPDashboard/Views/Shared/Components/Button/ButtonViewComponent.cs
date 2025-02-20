using Microsoft.AspNetCore.Mvc;

namespace QAPDashboard.Views.Shared.Components.Button
{
    public class ButtonViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(ButtonViewModel buttonViewModel)
        {
            return View("Button", buttonViewModel);
        }
    }
}
