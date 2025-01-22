using Microsoft.Maui.Graphics;
using Microsoft.Maui.Graphics.Skia;
using TagsCloudVisualization;

namespace DrawingTagsCloudVisualization;

public class ImageSaver(ITagsCloudDrawer tagsCloudDrawer) : IImageSaver
{
    readonly ITagsCloudDrawer tagsCloudDrawer = tagsCloudDrawer;

    public Result<bool> SaveToFile(string filePath, int length, int width, string color, List<RectangleInformation> rectangleInformation)
    {
        if (string.IsNullOrWhiteSpace(filePath))
            return Result.Fail<bool>("File path cannot be null or empty.");

        if (length <= 0 || width <= 0)
            return Result.Fail<bool>("Canvas dimensions must be positive numbers.");

        if (rectangleInformation == null || rectangleInformation.Count == 0)
            return Result.Fail<bool>("Rectangle information cannot be null or empty.");

        using var bitmapContext = new SkiaBitmapExportContext(length, width, 2.0f);
        var canvas = bitmapContext.Canvas;

        var result = tagsCloudDrawer.Draw(canvas, color, rectangleInformation, length, width);
        if (!result.IsSuccess)
            return Result.Fail<bool>("Error during drawing");

        canvas = result.GetValueOrThrow();

        using var image = bitmapContext.Image;

        var resultFile = Result.Of(() =>
        {
            try
            {
                using var stream = File.OpenWrite(filePath);
                image.Save(stream);
            }
            catch (Exception)
            {
                throw new IOException("Error saving image to file");
            }
            return image;
        });
        if (!resultFile.IsSuccess)
            return Result.Fail<bool>("Error saving image to file");
        return Result.Ok(true);
    }
}