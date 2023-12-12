using System;
using System.IO;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace FunctionApp1
{
    public class FunctionImageTriigger
    {
        [FunctionName("FunctionImageTriigger")]
        public void Run([BlobTrigger("imagesfolder/{name}", Connection = "day1storageaccount")]Stream myBlob, string name, ILogger log)
        {
            log.LogInformation($"C# Blob trigger function Processed blob\n Name:{name} \n Size: {myBlob.Length} Bytes");
        }
    }
}
