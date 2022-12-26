using System;
using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.WebJobs.Host;

namespace QueueFunction
{
    //https://learn.microsoft.com/en-us/azure/azure-functions/functions-bindings-storage-queue-trigger?tabs=in-process%2Cextensionv5&pivots=programming-language-csharp
    public class Queue
    {
        //Message needs to be encoded to base64 string for getting received at receiver in Function
        //[FunctionName("GetMessages")]
        //public void Run([QueueTrigger("appqueue", Connection = "connectionString")] string myQueueItem, ILogger log)
        //{
        //    log.LogInformation($"C# Queue trigger function processed: {myQueueItem}");
        //}

        //[FunctionName("GetMessages")]
        //public void Run([QueueTrigger("appqueue", Connection = "connectionString")] Order myQueueItem, ILogger log)
        //{
        //    log.LogInformation($"C# Queue trigger function processed: {myQueueItem.OrderId} and {myQueueItem.Quantity}");
        //}

        //[FunctionName("GetMessages")]
        //[return: Table("Orders",Connection = "connectionString")]
        //public TableOrder Run([QueueTrigger("appqueue", Connection = "connectionString")] Order myQueueItem, ILogger log)
        //{
        //    TableOrder tableOrder = new TableOrder
        //    {
        //        PartitionKey = myQueueItem.OrderId,
        //        RowKey = myQueueItem.Quantity.ToString()
        //    };
        //    log.LogInformation($"Order Information has been written to the table");
        //    return tableOrder;
        //}

        [FunctionName("GetMessages")]
        public void Run([QueueTrigger("appqueue", Connection = "connectionString")] Order myQueueItem, ILogger log, [Table("Orders", Connection = "connectionString")]ICollector<TableOrder> tableorder)
        {
            TableOrder tableOrder = new TableOrder
            {
                PartitionKey = myQueueItem.OrderId,
                RowKey = myQueueItem.Quantity.ToString()
            };
            tableorder.Add(tableOrder);
            log.LogInformation($"Order Information has been written to the table");
        }
    }
}
