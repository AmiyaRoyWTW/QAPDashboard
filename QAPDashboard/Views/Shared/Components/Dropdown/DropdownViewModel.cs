using System.Collections.Generic;

namespace QAPDashboard.Views.Shared.Components.Dropdown
{
    public class DropdownViewModel
    {
        public List<string>? Options { get; set; }
        public string? Id { get; set; }
        public string? Label { get; set; }
        public string? Name { get; set; }
        public string? SelectedItem { get; set; }
        public string? FirstItem { get; set; }
        public bool IsDisabled { get; set; } = false;
    }
}
