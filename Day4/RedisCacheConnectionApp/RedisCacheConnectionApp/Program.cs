

using StackExchange.Redis;

string connectionString = "day3kannanredis.redis.cache.windows.net:6380,password=uArr2F6hDbMWzy8QioDGbw2whVoV9Z7etAzCaHPKCQ8=,ssl=True,abortConnect=False";

ConnectionMultiplexer redis = ConnectionMultiplexer.Connect(connectionString);



void GetCacheData()
{
    IDatabase database = redis.GetDatabase();
    if (database.KeyExists("message1"))
        Console.WriteLine(database.StringGet("message1"));
    else
        Console.WriteLine("key does not exist");

}
void SetCacheData(string key,string value)
{
    IDatabase database = redis.GetDatabase();

    database.StringSet(key,value);

    Console.WriteLine("Cache data set");
}
//GetCacheData();
SetCacheData("message2", "in day4");
Console.WriteLine("End");
