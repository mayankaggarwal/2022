using System;
using System.Text;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace ServiceBusQueueFunction
{
    public class ServiceBusQueue
    {
        //[FunctionName("GetMessages")]
        //public void Run([ServiceBusTrigger("appqueue", Connection = "azQueueConnectionString")]string myQueueItem, ILogger log)
        //{
        //    log.LogInformation($"C# ServiceBus queue trigger function processed message: {myQueueItem}");
        //}

        [FunctionName("GetMessages")]
        public void Run([ServiceBusTrigger("appqueue", Connection = "azQueueConnectionString")] Message myQueueItem, ILogger log)
        {
            log.LogInformation("MessageBody: {0}", Encoding.UTF8.GetString(myQueueItem.Body));
            log.LogInformation("SequenceNumber: {0}", myQueueItem.SystemProperties.SequenceNumber);
        }

        [FunctionName("GetMessagesFromTopic")]
        public void Run2([ServiceBusTrigger("apptopic","SubscriptionA", Connection = "azTopicConnectionString")] Message myQueueItem, ILogger log)
        {
            log.LogInformation("MessageBody: {0}", Encoding.UTF8.GetString(myQueueItem.Body));
            log.LogInformation("SequenceNumber: {0}", myQueueItem.SystemProperties.SequenceNumber);
        }
    }
}
