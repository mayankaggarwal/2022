// See https://aka.ms/new-console-template for more information
using Azure.Messaging.ServiceBus;
using AzureServiceBusQueue;
using Newtonsoft.Json;

Console.WriteLine("Hello, World!");
string connectionString = "";
string queueName = "appqueue";

string deadLetter_connectionString = $"{connectionString}/$DeadLetterQueue";
string deadLetter_queueName = $"{queueName}/$DeadLetterQueue";

string[] Importance = new string[] { "High", "Medium", "Low" };

var orders = new List<Order>()
{
    new Order{OrderID="01",Quantity=100,UnitPrice=9.99F},
    new Order{OrderID="02",Quantity=200,UnitPrice=10.99F},
    new Order{OrderID="03",Quantity=300,UnitPrice=8.99F}
};
//await SendMessages(orders);
//await PeekMessages();
//await PeekMessages1();
//await ReceiveMessages();
//await GetProperties();
//await SendMessagesWithCustomProperties(orders);
//await ServiceBusProcessor();
//await PeekMessage();
//await DeadLetter_ReceiveMessages();

async Task SendMessages(List<Order> orders)
{
    ServiceBusClient serviceBusClient = new ServiceBusClient(connectionString);
    ServiceBusSender serviceBusSender = serviceBusClient.CreateSender(queueName);
    ServiceBusMessageBatch serviceBusMessageBatch = await serviceBusSender.CreateMessageBatchAsync();
    foreach(var order in orders)
    {
        var serviceBusMessage = new ServiceBusMessage(JsonConvert.SerializeObject(order));
        serviceBusMessage.ContentType = "application/json";
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
async Task PeekMessages()
{
    ServiceBusClient serviceBusClient = new ServiceBusClient(connectionString);
    ServiceBusReceiver serviceBusReceiver = serviceBusClient.CreateReceiver(queueName, new ServiceBusReceiverOptions { ReceiveMode = ServiceBusReceiveMode.PeekLock });

    IAsyncEnumerable<ServiceBusReceivedMessage> messages = serviceBusReceiver.ReceiveMessagesAsync();

    await foreach (var message in messages)
    {
        Order order = JsonConvert.DeserializeObject<Order>(message.Body.ToString());
        Console.WriteLine($"Order with values {order.OrderID}, {order.Quantity} and {order.UnitPrice}");
    }
    await serviceBusReceiver.DisposeAsync();
    await serviceBusClient.DisposeAsync();
}
async Task PeekMessages1()
{
    ServiceBusClient serviceBusClient = new ServiceBusClient(connectionString);
    ServiceBusReceiver serviceBusReceiver = serviceBusClient.CreateReceiver(queueName, new ServiceBusReceiverOptions { ReceiveMode = ServiceBusReceiveMode.PeekLock });

    IReadOnlyList<ServiceBusReceivedMessage> messages = await serviceBusReceiver.PeekMessagesAsync(10);

    foreach (var message in messages)
    {
        Order order = JsonConvert.DeserializeObject<Order>(message.Body.ToString());
        Console.WriteLine($"Order with values {order.OrderID}, {order.Quantity} and {order.UnitPrice}");
    }
    await serviceBusReceiver.DisposeAsync();
    await serviceBusClient.DisposeAsync();

}
async Task ReceiveMessages()
{
    ServiceBusClient serviceBusClient = new ServiceBusClient(connectionString);
    ServiceBusReceiver serviceBusReceiver = serviceBusClient.CreateReceiver(queueName, new ServiceBusReceiverOptions { ReceiveMode = ServiceBusReceiveMode.ReceiveAndDelete });

    IAsyncEnumerable<ServiceBusReceivedMessage> messages = serviceBusReceiver.ReceiveMessagesAsync();

    await foreach (var message in messages)
    {
        Order order = JsonConvert.DeserializeObject<Order>(message.Body.ToString());
        Console.WriteLine($"Order with values {order.OrderID}, {order.Quantity} and {order.UnitPrice}");
    }
    await serviceBusReceiver.DisposeAsync();
    await serviceBusClient.DisposeAsync();

}

async Task GetProperties()
{
    ServiceBusClient serviceBusClient = new ServiceBusClient(connectionString);
    ServiceBusReceiver serviceBusReceiver = serviceBusClient.CreateReceiver(queueName, new ServiceBusReceiverOptions { ReceiveMode = ServiceBusReceiveMode.PeekLock });

    IAsyncEnumerable<ServiceBusReceivedMessage> messages = serviceBusReceiver.ReceiveMessagesAsync();
    await foreach (var message in messages)
    {
        Console.WriteLine($"MessageId: {message.MessageId}, SequenceNumber : {message.SequenceNumber}, Importance: {message.ApplicationProperties["Importance"]}");
    }
    await serviceBusReceiver.DisposeAsync();
    await serviceBusClient.DisposeAsync();
}

async Task SendMessagesWithCustomProperties(List<Order> orders)
{
    ServiceBusClient serviceBusClient = new ServiceBusClient(connectionString);
    ServiceBusSender serviceBusSender = serviceBusClient.CreateSender(queueName);
    ServiceBusMessageBatch serviceBusMessageBatch = await serviceBusSender.CreateMessageBatchAsync();
    int i = 0;
    foreach (var order in orders)
    {
        var serviceBusMessage = new ServiceBusMessage(JsonConvert.SerializeObject(order));
        serviceBusMessage.ContentType = "application/json";
        serviceBusMessage.ApplicationProperties.Add("Importance", Importance[i%3]);
        serviceBusMessage.TimeToLive = TimeSpan.FromSeconds(30);
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

async Task ServiceBusProcessor()
{
    ServiceBusClient serviceBusClient = new ServiceBusClient(connectionString);
    ServiceBusProcessor serviceBusProcessor = serviceBusClient.CreateProcessor(queueName,new ServiceBusProcessorOptions { });

    serviceBusProcessor.ProcessMessageAsync += ServiceBusProcessor_ProcessMessageAsync;
    serviceBusProcessor.ProcessErrorAsync += ServiceBusProcessor_ProcessErrorAsync;

    await serviceBusProcessor.StartProcessingAsync();
    Console.WriteLine("Waiting for messages");
    Console.ReadKey();

    await serviceBusProcessor.StopProcessingAsync();

    await serviceBusProcessor.DisposeAsync();
    await serviceBusClient.DisposeAsync();
}

Task ServiceBusProcessor_ProcessErrorAsync(ProcessErrorEventArgs arg)
{
    Console.WriteLine(arg.Exception.ToString());
    return Task.CompletedTask;
}

async Task ServiceBusProcessor_ProcessMessageAsync(ProcessMessageEventArgs arg)
{
    Order order = JsonConvert.DeserializeObject<Order>(arg.Message.Body.ToString());
    Console.WriteLine($"Order with values {order.OrderID}, {order.Quantity} and {order.UnitPrice}");
}

async Task PeekMessage()
{
    ServiceBusClient serviceBusClient = new ServiceBusClient(connectionString);
    ServiceBusReceiver serviceBusReceiver = serviceBusClient.CreateReceiver(queueName, new ServiceBusReceiverOptions { ReceiveMode = ServiceBusReceiveMode.PeekLock });

    ServiceBusReceivedMessage message = await serviceBusReceiver.ReceiveMessageAsync();
    Order order = JsonConvert.DeserializeObject<Order>(message.Body.ToString());
    Console.WriteLine($"Order with values {order.OrderID}, {order.Quantity} and {order.UnitPrice}");

    serviceBusReceiver.CompleteMessageAsync(message);

    await serviceBusReceiver.DisposeAsync();
    await serviceBusClient.DisposeAsync();
    
    

}

async Task DeadLetter_ReceiveMessages()
{
    ServiceBusClient serviceBusClient = new ServiceBusClient(deadLetter_connectionString);
    ServiceBusReceiver serviceBusReceiver = serviceBusClient.CreateReceiver(deadLetter_queueName, new ServiceBusReceiverOptions { ReceiveMode = ServiceBusReceiveMode.ReceiveAndDelete });

    IAsyncEnumerable<ServiceBusReceivedMessage> messages = serviceBusReceiver.ReceiveMessagesAsync();

    await foreach (var message in messages)
    {
        Order order = JsonConvert.DeserializeObject<Order>(message.Body.ToString());
        Console.WriteLine($"Order with values {order.OrderID}, {order.Quantity} and {order.UnitPrice}");
    }
    await serviceBusReceiver.DisposeAsync();
    await serviceBusClient.DisposeAsync();

}