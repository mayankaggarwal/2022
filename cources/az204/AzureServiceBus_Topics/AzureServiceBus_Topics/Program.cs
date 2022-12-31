// See https://aka.ms/new-console-template for more information
using Azure.Messaging.ServiceBus;
using AzureServiceBus_Topics;
using Microsoft.Extensions.Configuration;
using System.Text.Json;

Console.WriteLine("Hello, World!");

    //var connectionString = Environment.GetEnvironmentVariable("azQueueConnectionString");
IConfiguration configuration = new ConfigurationBuilder()
.AddEnvironmentVariables().Build();

var connectionString = configuration.GetValue<string>("azTopicConnectionString");
string topicName = "apptopic";

var orders = new List<Order>()
{
new Order{OrderID="01",Quantity=100,UnitPrice=9.99F},
new Order{OrderID="02",Quantity=200,UnitPrice=10.99F},
new Order{OrderID="03",Quantity=300,UnitPrice=8.99F}
};
string[] Importance = new string[] { "High", "Medium", "Low" };
await SendMessages(orders);
//await ReceiveMessages("SubscriptionB");

async Task SendMessages(List<Order> orders)
{
    ServiceBusClient serviceBusClient = new ServiceBusClient(connectionString);
    ServiceBusSender serviceBusSender = serviceBusClient.CreateSender(topicName);
    ServiceBusMessageBatch serviceBusMessageBatch = await serviceBusSender.CreateMessageBatchAsync();
    int i = 0;
    int messageId = 1;
    foreach (var order in orders)
    {
        var serviceBusMessage = new ServiceBusMessage(JsonSerializer.Serialize(order));
        serviceBusMessage.ContentType = "application/json";
        serviceBusMessage.ApplicationProperties.Add("Importance", Importance[i % 3]);
        serviceBusMessage.MessageId = (messageId + i).ToString();
        i++;
        if (!serviceBusMessageBatch.TryAddMessage(serviceBusMessage))
        {
            throw new Exception("Error occured");
        }
    }

    Console.WriteLine("Sending Order Batch Messages");
    await serviceBusSender.SendMessagesAsync(serviceBusMessageBatch);
    await serviceBusSender.DisposeAsync();
    await serviceBusClient.DisposeAsync();
}

async Task ReceiveMessages(string subscriptionName)
{
    ServiceBusClient serviceBusClient = new ServiceBusClient(connectionString);
    ServiceBusReceiver serviceBusReceiver = serviceBusClient.CreateReceiver(topicName, subscriptionName,new ServiceBusReceiverOptions { ReceiveMode = ServiceBusReceiveMode.ReceiveAndDelete});
    IAsyncEnumerable<ServiceBusReceivedMessage> messages = serviceBusReceiver.ReceiveMessagesAsync();   
    await foreach(var message in messages)
    {
        Order order = JsonSerializer.Deserialize<Order>(message.Body.ToString());
        Console.WriteLine($"Order with values {order.OrderID}, {order.Quantity} and {order.UnitPrice}");
    }
    await serviceBusReceiver.DisposeAsync();
    await serviceBusClient.DisposeAsync();
}
