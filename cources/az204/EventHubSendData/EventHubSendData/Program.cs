// See https://aka.ms/new-console-template for more information
using Azure.Messaging.EventHubs.Producer;
using EventHubSendData;
using Microsoft.Extensions.Configuration;
using System.Text;
using System.Text.Json;

Console.WriteLine("Hello, World!");

IConfiguration configuration = new ConfigurationBuilder()
    .AddEnvironmentVariables()
    .Build();
string conectionString = configuration.GetValue<string>("azEventHubSendConnectionString");
string eventHubName = "apphub";


List<Device> deviceList = new List<Device>()
{
    new Device{deviceId="D1",temperature=50.0f},
    new Device{deviceId="D2",temperature=34.1f},
    new Device{deviceId="D2",temperature=36.9f},
    new Device{deviceId="D3",temperature=25.0f},
    new Device{deviceId="D4",temperature=40.1f},
    new Device{deviceId="D4",temperature=38.9f},
    new Device{deviceId="D4",temperature=35.4f},
};

await SendData();
async Task SendData()
{
    EventHubProducerClient eventHubProducerClient = new EventHubProducerClient(conectionString,eventHubName);
    EventDataBatch eventDataBatch = await eventHubProducerClient.CreateBatchAsync(new CreateBatchOptions { });
    foreach(var device in deviceList)
    {
        var eventData = new Azure.Messaging.EventHubs.EventData(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(device)));
        if (!eventDataBatch.TryAdd(eventData))
        {
            Console.WriteLine("Error occured in adding data to the batch");
        }
    }
    await eventHubProducerClient.SendAsync(eventDataBatch);
    Console.WriteLine("Events are sent");
    eventDataBatch.Dispose();
    await eventHubProducerClient.DisposeAsync();
}
