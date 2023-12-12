using System;
using System.IO;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage;
using System.Threading.Tasks;

namespace FunctionAppForImageCompression
{
    public class ProcessImageFunction
    {
        [FunctionName("ProcessImageFunction")]
        public async Task RunAsync([BlobTrigger("documents/{name}", Connection = "AzureWebJobsStorage")]Stream imageStream, string name, ILogger log)
        {

            try
            {
                log.LogInformation($"Processing image: {name}");

                // Read the uploaded image into a byte array
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    await imageStream.CopyToAsync(memoryStream);
                    byte[] imageBytes = memoryStream.ToArray();

                    // Generate the thumbnail
                    byte[] thumbnailBytes = await ImageThumbnailConverter.GenerateThumbnailAsync(imageBytes, 100, 100);

                    string storageConnectionString = Environment.GetEnvironmentVariable("AzureWebJobsStorage");

                    // Create a CloudStorageAccount instance from the connection string
                    CloudStorageAccount storageAccount = CloudStorageAccount.Parse(storageConnectionString);

                    // Create a CloudBlobClient to interact with Blob storage
                    CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
                    CloudBlobContainer container = blobClient.GetContainerReference("thumbnails");
                    CloudBlockBlob thumbnailBlob = container.GetBlockBlobReference(name);
                    thumbnailBlob.Properties.ContentType = "image/jpeg";
                    using (MemoryStream thumbnailStream = new MemoryStream(thumbnailBytes))
                    {
                        await thumbnailBlob.UploadFromStreamAsync(thumbnailStream);
                    }
                    // Write the thumbnail to the output blob
                    //  await thumbnailStream.WriteAsync(thumbnailBytes, 0, thumbnailBytes.Length);


                    log.LogInformation($"Thumbnail generated and uploaded: {name}");
                }
            }
            catch (Exception ex)
            {
                log.LogError($"Error processing image: {ex.Message}");
            }

        }
    }
}
