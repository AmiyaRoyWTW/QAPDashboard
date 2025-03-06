namespace QAPDashboard.Views.Shared.Components.TableRowInput
{
  public class TableRowInputViewModel
  {
    public string? Id { get; set; }
    public string? Name { get; set; }
    public string? Value { get; set; }
    public bool Enabled { get; set; } = true;
    public bool Hidden { get; set; } = false;
  }
}
