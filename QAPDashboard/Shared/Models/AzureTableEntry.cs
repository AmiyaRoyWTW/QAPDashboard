namespace QAPDashboard.Shared.Models
{
    public class AzureTableEntry
    {
        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public string Url { get; set; }
    }
}
