namespace ImageUploadAzureStoreApp.Models
{
    public class ConfigModel
    {
        public string ContainerName { get; set; } = "";
        public string StorageConnectionString { get; set; } = string.Empty;

        public string ThumbnailImageContainerName { get; set; } = "";
    }
}
