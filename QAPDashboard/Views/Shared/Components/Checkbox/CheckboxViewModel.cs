namespace QAPDashboard.Views.Shared.Components.Checkbox
{
    public class CheckboxViewModel
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? Class { get; set; }
        public string? Label { get; set; }
        public string? Value { get; set; }
        public bool IsChecked { get; set; } = false;
    }
}
