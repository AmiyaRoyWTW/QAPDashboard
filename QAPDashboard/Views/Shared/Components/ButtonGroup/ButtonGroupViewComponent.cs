using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace QAPDashboard.Views.Shared.Components.ButtonGroup
{
    public class ButtonGroupViewComponent : ViewComponent
    {
        public Task<IViewComponentResult> InvokeAsync(ButtonGroupViewModel buttons)
        {
            return Task.FromResult<IViewComponentResult>(View("ButtonGroup", buttons));
        }

    }
}
