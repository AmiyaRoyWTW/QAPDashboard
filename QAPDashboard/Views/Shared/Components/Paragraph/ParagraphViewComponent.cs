using Microsoft.AspNetCore.Mvc;

namespace QAPDashboard.Views.Shared.Components.Paragraph
{
    public class ParagraphViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(ParagraphViewModel paragraph)
        {
            return View("Paragraph", paragraph);
        }
    }
}
