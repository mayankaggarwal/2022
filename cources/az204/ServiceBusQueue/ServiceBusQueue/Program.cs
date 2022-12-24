// See https://aka.ms/new-console-template for more information
using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;

Console.WriteLine("Hello, World!");

string connectionString = "";
string queueName = "appqueue";

//SendMessage("Test Message 1");
//SendMessage("Test Message 2");

//PeekMessages();
ReceiveMessages();
Console.WriteLine($"The number of messages in the queue are {GetQueueLength()}");
void SendMessage(string message)
{
    var client = new QueueClient(connectionString, queueName);
    if (client.Exists())
    {
        client.SendMessage(message);
        Console.WriteLine("The message has been Sent");
    }
}

void PeekMessages()
{
    var client = new QueueClient(connectionString, queueName);
    int maxMessages = 10;
    PeekedMessage[] messages = client.PeekMessages(maxMessages).Value;
    Console.WriteLine("The messages in the queue are:");
    foreach(var message in messages)
    {
        Console.WriteLine(message.Body);
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
