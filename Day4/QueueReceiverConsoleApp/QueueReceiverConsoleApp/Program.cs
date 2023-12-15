
using Azure.Messaging.ServiceBus;

string connectionString = "Endpoint=sb://day4kannansmessages.servicebus.windows.net/;SharedAccessKeyName=receiverPolicy;SharedAccessKey=gOZCFJYNSbnJXMFGPD6rtDfvLlrfXJJ1e+ASbJLWPNU=;EntityPath=myorders";
string queueName = "myorders";
ServiceBusClient client = default!;
ServiceBusProcessor processor = default!;

client = new ServiceBusClient(connectionString);
processor = client.CreateProcessor(queueName, new ServiceBusProcessorOptions());

try
{
    processor.ProcessMessageAsync += MessageHandler;
    processor.ProcessErrorAsync += ErrorHandler;

    await processor.StartProcessingAsync();
    Console.WriteLine("Wait for a minute and then press any key to end the processing");
    Console.ReadKey();

    Console.WriteLine("\nStopping the receiver...");
    await processor.StopProcessingAsync();
    Console.WriteLine("Stopped receiving messages");
}
finally
{
    await processor.DisposeAsync();
    await client.DisposeAsync();
}
static async Task MessageHandler(ProcessMessageEventArgs args)
{
    string body = args.Message.Body.ToString();
    Console.WriteLine($"Received: {body}");
    await args.CompleteMessageAsync(args.Message);
}

static Task ErrorHandler(ProcessErrorEventArgs args)
{
    Console.WriteLine(args.Exception.ToString());
    return Task.CompletedTask;
}
