using Microsoft.Azure; // Namespace for CloudConfigurationManager 
using Microsoft.WindowsAzure.Storage; // Namespace for CloudStorageAccount
using Microsoft.WindowsAzure.Storage.Table; // Namespace for Table storage types
using System;
using System.Collections.Generic;

namespace AzTableDemo
{
    class Program
    {
        static void Main(string[] args)
        {

            // Parse the connection string and return a reference to the storage account.
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
                CloudConfigurationManager.GetSetting("StorageConnectionString"));

            // Create the table client.
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();

            IEnumerable<CloudTable> list = tableClient.ListTables("WAD");
            foreach(var tab in list)
            {
                Console.WriteLine(tab.Name);
            }

            // Create the CloudTable object that represents the "people" table.
            CloudTable table = tableClient.GetTableReference("WADMetricsPT1HP10DV2S20160823");

            // Construct the query operation for all customer entities where PartitionKey="***".
            TableQuery<WADMerticsEntity> query = new TableQuery<WADMerticsEntity>(); //.Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, "Smith"));

            // Print the fields for each customer.
            foreach (WADMerticsEntity entity in table.ExecuteQuery(query))
            {
                Console.WriteLine("{0}, {1}\t{2}\t{3}", entity.PartitionKey, entity.RowKey,
                    entity.RoleInstance, entity.CounterName);
            }

            Console.ReadKey();
        }
    }
}
