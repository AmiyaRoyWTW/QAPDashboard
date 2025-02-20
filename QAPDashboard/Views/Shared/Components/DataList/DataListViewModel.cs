using System.Collections.Generic;

namespace QAPDashboard.Views.Shared.Components.DataList
{
    public class DataListViewModel
    {
        public string Id { get; set; } = "";
        public string Name { get; set; }
        public string ListId { get; set; }
        public List<string> Options { get; set; } = [];

    }
}
