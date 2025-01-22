using Microsoft.Maui.Graphics;
using Microsoft.Maui.Graphics.Skia;
using TagsCloudVisualization;

namespace DrawingTagsCloudVisualization;

public class ImageSaver(ITagsCloudDrawer tagsCloudDrawer) : IImageSaver
{
    readonly ITagsCloudDrawer tagsCloudDrawer = tagsCloudDrawer;

    public Result<bool> SaveToFile(string filePath, int length, int width, string color, List<RectangleInformation> rectangleInformation)
    {
        return Result.Of(() =>
        {
            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentException("File path cannot be null or empty.");

            if (length <= 0 || width <= 0)
                throw new ArgumentException("Canvas dimensions must be positive numbers.");

            if (rectangleInformation == null || rectangleInformation.Count == 0)
                throw new ArgumentException("Rectangle information cannot be null or empty.");

            using var bitmapContext = new SkiaBitmapExportContext(length, width, 2.0f);
            var canvas = bitmapContext.Canvas;

            var result = tagsCloudDrawer.Draw(canvas, color, rectangleInformation, length, width);
            if (!result.IsSuccess)
                throw new InvalidOperationException("Error during drawing");

            canvas = result.GetValueOrThrow();

            using var image = bitmapContext.Image;

            try
            {
                using var stream = File.OpenWrite(filePath);
                image.Save(stream);
            }
            catch (Exception)
            {
                throw new IOException("Error saving image to file");
            }
            return true;
        }, "Error occurred while saving the image to file");
    }
}