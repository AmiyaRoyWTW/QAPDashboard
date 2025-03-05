namespace QAPDashboard.Views.Shared.Components.Input
{
    public class InputViewModel
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? Label { get; set; }
        public int Value { get; set; }
        public int Min { get; set; }
        public int Max { get; set; }
        public bool Enabled { get; set; } = true;
    }

}
