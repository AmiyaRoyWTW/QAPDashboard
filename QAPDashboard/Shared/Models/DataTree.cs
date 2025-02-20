namespace QAPDashboard.Shared.Models
{
    public class DataTree
    {
        public Dictionary<string, object> Content { get; set; }
        public string DataType { get; set; }
        public List<DataTree> Children { get; set; }
    }
}
