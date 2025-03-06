using Microsoft.AspNetCore.Mvc;

namespace QAPDashboard.Views.Shared.Components.TextArea
{
    public class TextAreaViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(TextAreaViewModel textArea)
        {
            return View("TextArea", textArea);
        }
    }
}
