using Microsoft.AspNetCore.Mvc;

namespace QAPDashboard.Views.Shared.Components.TableRowInput
{
  public class TableRowInputViewComponent : ViewComponent
  {
    public IViewComponentResult Invoke(TableRowInputViewModel tableRowInputViewModel)
    {
      return View("TableRowInput", tableRowInputViewModel);
    }
  }

}