using Microsoft.Maui.Graphics;
using Microsoft.Maui.Graphics.Skia;
using TagsCloudVisualization;

namespace DrawingTagsCloudVisualization;

public class ImageSaver(ITagsCloudDrawer tagsCloudDrawer) : IImageSaver
{
    readonly ITagsCloudDrawer tagsCloudDrawer = tagsCloudDrawer;

    public void SaveToFile(string filePath, int lenght, int width, string color, List<RectangleInformation> rectangleInformation)
    {
        using var bitmapContext = new SkiaBitmapExportContext(lenght, width, 2.0f);
        var canvas = bitmapContext.Canvas;
        canvas.FontColor = Colors.Black;
        var result = tagsCloudDrawer.Draw(canvas, color, rectangleInformation, lenght, width);
        if (!result.IsSuccess){
            Console.WriteLine(result.Error);
            return;
        }
        canvas = result.GetValueOrThrow();
        using var image = bitmapContext.Image;
        using var stream = File.OpenWrite(filePath);
        image.Save(stream);
    }
}