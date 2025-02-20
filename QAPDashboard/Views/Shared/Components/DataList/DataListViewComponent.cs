using Microsoft.AspNetCore.Mvc;

namespace QAPDashboard.Views.Shared.Components.DataList
{
    public class DataListViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(DataListViewModel dataListViewModel)
        {
            return View("DataList", dataListViewModel);
        }
    }
}
