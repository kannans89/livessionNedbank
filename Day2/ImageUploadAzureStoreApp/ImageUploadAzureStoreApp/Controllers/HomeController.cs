using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs;
using ImageUploadAzureStoreApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ImageUploadAzureStoreApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ConfigModel _configOptions;

        public HomeController(ILogger<HomeController> logger, ConfigModel options)
        {
            _logger = logger;
            _configOptions = options;
        }
        public IActionResult UploadImageView()
        {

            return View();
        }


        public async Task<IActionResult> ViewThumbNails()
        {

            BlobContainerClient containerClient = await GetCloudBlobContainer(_configOptions.ThumbnailImageContainerName);
            List<string> results = new List<string>();
            await foreach (BlobItem blobItem in containerClient.GetBlobsAsync())
            {
                results.Add(
                    Flurl.Url.Combine(
                        containerClient.Uri.AbsoluteUri,
                        blobItem.Name
                    )
                );
            }

            ViewBag.ImageUrls = results;
            return View();
        }


        [HttpPost]

        public async Task<IActionResult> UploadImage(ImageUploadVM vm)
        {
            Stream image = vm.Image.OpenReadStream();
            BlobContainerClient containerClient = await GetCloudBlobContainer(_configOptions.ContainerName);
            // string blobName = Guid.NewGuid().ToString().ToLower().Replace("-", String.Empty);
            string blobName = vm.Image.FileName;
            BlobClient blobClient = containerClient.GetBlobClient(blobName);

            BlobHttpHeaders blobHeaders = new BlobHttpHeaders
            {
                ContentType = "image/jpeg"
                //ContentType = "application/pdf"
            };
            await blobClient.UploadAsync(image, blobHeaders);

            return RedirectToAction("Index");
        }

        private async Task<BlobContainerClient> GetCloudBlobContainer(string containerName)
        {
            BlobServiceClient serviceClient = new BlobServiceClient(_configOptions.StorageConnectionString);
            BlobContainerClient containerClient = serviceClient.GetBlobContainerClient(containerName);
            await containerClient.CreateIfNotExistsAsync();
            return containerClient;
        }


        public async Task<IActionResult> ViewImages()
        {
            BlobContainerClient containerClient = await GetCloudBlobContainer(_configOptions.ContainerName);
            List<string> results = new List<string>();
            await foreach (BlobItem blobItem in containerClient.GetBlobsAsync())
            {
                results.Add(
                    Flurl.Url.Combine(containerClient.Uri.AbsoluteUri, blobItem.Name)
                );
            }

            ViewBag.ImageUrls = results;
            return View();
        }


        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
