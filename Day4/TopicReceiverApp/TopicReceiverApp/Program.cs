using Azure.Messaging.ServiceBus;
using Newtonsoft.Json;
using TopicReceiverConsoleApp;


string connectionString = "Endpoint=sb://day4kannansmessages.servicebus.windows.net/;SharedAccessKeyName=receiverPolicy;SharedAccessKey=SwqhgZX/rRVUycPB8n4pGd88u4JANW6Np+ASbBTc2qw=;EntityPath=stocks";
string topicName = "stocks";
string subscriptionName = "subscriberB";//change to ConsumerA,ConsumerB,ConsumerC

await ReceiveMessages();

async Task ReceiveMessages()
{
    ServiceBusClient serviceBusClient = new ServiceBusClient(connectionString);
    ServiceBusReceiver serviceBusReceiver = serviceBusClient.CreateReceiver(topicName, subscriptionName,
        new ServiceBusReceiverOptions() { ReceiveMode = ServiceBusReceiveMode.ReceiveAndDelete });

    IAsyncEnumerable<ServiceBusReceivedMessage> messages = serviceBusReceiver.ReceiveMessagesAsync();

    await foreach (ServiceBusReceivedMessage message in messages)
    {

        Stock order = JsonConvert.DeserializeObject<Stock>(message.Body.ToString());
        Console.WriteLine("Order Id {0}", order.OrderID);
        Console.WriteLine("Quantity {0}", order.Quantity);
        Console.WriteLine("Unit Price {0}", order.UnitPrice);
        Console.WriteLine();
        //await Console.Out.WriteLineAsync();

    }
}
