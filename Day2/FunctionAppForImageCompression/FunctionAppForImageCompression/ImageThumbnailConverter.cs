using System;
using System.IO;
using System.Threading.Tasks;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;

namespace FunctionAppForImageCompression
{
    internal class ImageThumbnailConverter
    {
        public static async Task<byte[]> GenerateThumbnailAsync(byte[] imageBytes, int width, int height)
        {
            try
            {
                using (var image = await Image.LoadAsync(new MemoryStream(imageBytes)))
                {
                    // Resize the image to the specified width and height while preserving aspect ratio
                    image.Mutate(x => x.Resize(new ResizeOptions
                    {
                        Size = new Size(width, height),
                        Mode = ResizeMode.Max
                    }));

                    using (var outputStream = new MemoryStream())
                    {
                        // Save the thumbnail as a JPEG image (you can change the format as needed)
                        await image.SaveAsync(outputStream, new JpegEncoder());

                        // Convert the thumbnail to a byte array
                        return outputStream.ToArray();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error generating thumbnail: {ex.Message}");
                return null;
            }
        }
    }
}
