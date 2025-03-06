namespace QAPDashboard.Views.Shared.Components.Paragraph
{
    public class ParagraphViewModel
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? Label { get; set; }
        public string? Value { get; set; }
        public bool Enabled { get; set; } = true;
        public bool HiddenInput { get; set; } = false;
        public bool Editable { get; set; } = false;
    }
}
