using Azure.Messaging.ServiceBus;
using Newtonsoft.Json;
using TopicSenderConsoleApp;



string connectionString = "Endpoint=sb://day4kannansmessages.servicebus.windows.net/;SharedAccessKeyName=senderPolicy;SharedAccessKey=oZ/t6umGnr+gCp3I140mPz3AGS003e487+ASbIpJ1Ik=;EntityPath=stocks";
string topicName = "stocks";

List<Stock> orders = new List<Stock>()
{
        new Stock(){OrderID="01",Quantity=100,UnitPrice=9.99F},
        new Stock(){OrderID="02",Quantity=200,UnitPrice=10.99F},
        new Stock(){OrderID="03",Quantity=300,UnitPrice=8.99F}

};

await SendMessage(orders);
async Task SendMessage(List<Stock> orders)
{
    ServiceBusClient serviceBusClient = new ServiceBusClient(connectionString);
    ServiceBusSender serviceBusSender = serviceBusClient.CreateSender(topicName);

    ServiceBusMessageBatch serviceBusMessageBatch = await serviceBusSender.CreateMessageBatchAsync();
    int messageId = 1;
    foreach (Stock order in orders)
    {

        ServiceBusMessage serviceBusMessage = new ServiceBusMessage(JsonConvert.SerializeObject(order));
        serviceBusMessage.ContentType = "application/json";
        serviceBusMessage.MessageId = messageId.ToString();
        messageId++;

        if (!serviceBusMessageBatch.TryAddMessage(
            serviceBusMessage))
        {
            throw new Exception("Error occured");
        }

    }
    Console.WriteLine("Sending messages");
    await serviceBusSender.SendMessagesAsync(serviceBusMessageBatch);

    await serviceBusSender.DisposeAsync();
    await serviceBusClient.DisposeAsync();

}
