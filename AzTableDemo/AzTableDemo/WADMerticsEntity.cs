using Microsoft.WindowsAzure.Storage.Table;

namespace AzTableDemo
{
    internal class WADMerticsEntity : TableEntity
    {
        public WADMerticsEntity(string partitionKey, string rowKey)
        {
            this.PartitionKey = partitionKey;
            this.RowKey = rowKey;
        }

        public WADMerticsEntity() { }

        public string DeploymentId { get; set; }
        public string Role { get; set; }
        public string RoleInstance { get; set; }
        public string CounterName { get; set; }
        public double Maximum { get; set; }
        public double Minimum { get; set; }
        public double Total { get; set; }

        public int Count { get; set; }

        public double Average { get; set; }
    }
}