﻿namespace QAPDashboard.Views.Shared.Components.Button
{
    public class ButtonViewModel
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? Label { get; set; }
        public string? Text { get; set; }
        public string? Svg { get; set; }
        public string? Type { get; set; }
        public string? TailwindClasses { get; set; }
        public string Event { get; set; } = string.Empty;
        public bool IsDisabled { get; set; } = false;
        public bool IsHidden { get; set; } = false;
    }
}
