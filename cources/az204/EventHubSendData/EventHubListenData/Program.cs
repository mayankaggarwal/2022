using Azure.Messaging.EventHubs.Consumer;
using Azure.Messaging.EventHubs.Producer;
using Microsoft.Extensions.Configuration;
using System.Text;
using System.Text.Json;

Console.WriteLine("Hello, World!");

IConfiguration configuration = new ConfigurationBuilder()
    .AddEnvironmentVariables()
    .Build();
string conectionString = configuration.GetValue<string>("azEventHubListenConnectionString");
string consumerGroup = "$Default";
//await GetPartitionIds();
//await ReadEvents();
await ReadEventsFromPartition();

async Task GetPartitionIds()
{
    EventHubConsumerClient eventHubConsumerClient = new EventHubConsumerClient(consumerGroup,conectionString);
    string[] partitionIds = await eventHubConsumerClient.GetPartitionIdsAsync();
    foreach(string partitionId in partitionIds)
    {
        Console.WriteLine("Partition Id is {0}", partitionId);
    }
}

async Task ReadEvents()
{
    EventHubConsumerClient eventHubConsumerClient = new EventHubConsumerClient(consumerGroup, conectionString);
    var cancellationSource = new CancellationTokenSource();
    cancellationSource.CancelAfter(TimeSpan.FromSeconds(300));

    await foreach(PartitionEvent partitionEvent in eventHubConsumerClient.ReadEventsAsync(cancellationSource.Token))
    {
        Console.WriteLine($"Partition ID {partitionEvent.Partition.PartitionId}");
        Console.WriteLine($"Data Offset {partitionEvent.Data.Offset}");
        Console.WriteLine($"Sequence Number {partitionEvent.Data.SequenceNumber}");
        Console.WriteLine($"Partition Key {partitionEvent.Data.PartitionKey}");
        Console.WriteLine($"{Encoding.UTF8.GetString(partitionEvent.Data.EventBody)}");
    }
}

async Task ReadEventsFromPartition()
{
    EventHubConsumerClient eventHubConsumerClient = new EventHubConsumerClient(consumerGroup, conectionString);
    string partitionId = (await eventHubConsumerClient.GetPartitionIdsAsync()).First();
    var cancellationSource = new CancellationTokenSource();
    cancellationSource.CancelAfter(TimeSpan.FromSeconds(300));

    await foreach (PartitionEvent partitionEvent in eventHubConsumerClient.ReadEventsFromPartitionAsync(partitionId,EventPosition.Latest))
    {
        Console.WriteLine($"Partition ID {partitionEvent.Partition.PartitionId}");
        Console.WriteLine($"Data Offset {partitionEvent.Data.Offset}");
        Console.WriteLine($"Sequence Number {partitionEvent.Data.SequenceNumber}");
        Console.WriteLine($"Partition Key {partitionEvent.Data.PartitionKey}");
        Console.WriteLine($"{Encoding.UTF8.GetString(partitionEvent.Data.EventBody)}");
    }
}


