using System;
using System.Text;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Consumer;
using Azure.Messaging.EventHubs.Processor;
class Program
{


    private const string ehubNamespaceConnectionString = "Endpoint=sb://day5rg.servicebus.windows.net/;SharedAccessKeyName=ReceivePolicy;SharedAccessKey=rZECD0LxVfYOyKvJjzP5YrdwGv3ScoInQ+AEhJTUmXs=;EntityPath=apphub";
    private const string eventHubName = "apphub";

    private const string blobStorageConnectionString = "DefaultEndpointsProtocol=https;AccountName=day5rgstorage;AccountKey=bQxxvzdGPiDDVCqHR4L9r+nu0cntNlDoTh63lKMOcR8DaGLcYtnsjq7s6Lrir0zqnJEGDPaSOBpD+ASt6+b7xw==;EndpointSuffix=core.windows.net";
    private const string blobContainerName = "eventhubcheckpoint";


    static async Task Main()
    {
        // Read from the default consumer group: $Default
        string consumerGroup = "consumer2"; //EventHubConsumerClient.DefaultConsumerGroupName;
        // Create a blob container client that the event processor will use
        BlobContainerClient storageClient = new BlobContainerClient(blobStorageConnectionString, blobContainerName);
        // Create an event processor client to process events in the event hub
        EventProcessorClient processor = new EventProcessorClient(storageClient,
            consumerGroup, ehubNamespaceConnectionString, eventHubName);
        // Register handlers for processing events and handling errors
        processor.ProcessEventAsync += ProcessEventHandler;
        processor.ProcessErrorAsync += ProcessErrorHandler;
        // Start the processing
        await processor.StartProcessingAsync();
        // Wait for 10 seconds for the events to be processed
        //await Task.Delay(TimeSpan.FromSeconds(10));
        Console.WriteLine("Press Enter to stop...");
        Console.ReadLine();
        // Stop the processing
        await processor.StopProcessingAsync();
    }
    static async Task ProcessEventHandler(ProcessEventArgs eventArgs)
    {
        // Write the body of the event to the console window
        Console.WriteLine("\tRecevied event: {0}", Encoding.UTF8.GetString(eventArgs.Data.Body.ToArray()));
        // Update checkpoint in the blob storage so that the app receives only new events the next time it's run
        await eventArgs.UpdateCheckpointAsync(eventArgs.CancellationToken);
    }
    static Task ProcessErrorHandler(ProcessErrorEventArgs eventArgs)
    {
        // Write details about the error to the console window
        Console.WriteLine($"\tPartition '{eventArgs.PartitionId}': " +
            $"an unhandled exception was encountered. This was not expected to happen.");
        Console.WriteLine(eventArgs.Exception.Message);
        return Task.CompletedTask;
    }
}