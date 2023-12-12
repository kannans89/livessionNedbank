namespace ImageUploadAzureStoreApp.Models
{
    public class ImageUploadVM
    {
        public string? ImageCaption { set; get; }
        public string? ImageDescription { set; get; }
        public IFormFile? Image { set; get; }
    }
}
