// See https://aka.ms/new-console-template for more information
using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using Newtonsoft.Json;
using ServiceBusQueue;
using System.Text;

Console.WriteLine("Hello, World!");

string connectionString = "";
string queueName = "appqueue";

SendMessage(new Order { OrderId="Order 1",Quantity=12});
SendMessage(new Order { OrderId = "Order 2", Quantity = 245 });

//SendMessage1("Test Message 1");
//SendMessage1("Test Message 2");

await PeekMessages();
//ReceiveMessages();
Console.WriteLine($"The number of messages in the queue are {GetQueueLength()}");

void SendMessage(Order order)
{
    var client = new QueueClient(connectionString, queueName);
    if (client.Exists())
    {
        //client.SendMessage(message);
        client.SendMessage(Convert.ToBase64String(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(order))));
        Console.WriteLine("The message has been Sent");
    }
}

void SendMessage1(string message)
{
    var client = new QueueClient(connectionString, queueName);
    if (client.Exists())
    {
        //client.SendMessage(message);
        client.SendMessage(Convert.ToBase64String(Encoding.UTF8.GetBytes(message)));
        Console.WriteLine("The message has been Sent");
    }
}

async Task PeekMessages()
{
    var client = new QueueClient(connectionString, queueName);
    int maxMessages = 10;
    PeekedMessage[] messages = (await client.PeekMessagesAsync(maxMessages)).Value;
    Console.WriteLine("The messages in the queue are:");
    foreach(var message in messages)
    {
        Console.WriteLine(Encoding.UTF8.GetString(Convert.FromBase64String(message.Body.ToString())));
    }
}

void ReceiveMessages()
{
    var client = new QueueClient(connectionString, queueName);
    int maxMessages = 10;
    QueueMessage[] messages = client.ReceiveMessages(maxMessages).Value;
    Console.WriteLine("The messages in the queue received are:");
    foreach (var message in messages)
    {
        Console.WriteLine(message.Body);
        client.DeleteMessage(message.MessageId, message.PopReceipt);
    }
}

int GetQueueLength()
{
    var client = new QueueClient(connectionString, queueName);
    if (client.Exists())
    {
        QueueProperties properties = client.GetProperties();
        return properties.ApproximateMessagesCount;
    }
    return 0;
}
