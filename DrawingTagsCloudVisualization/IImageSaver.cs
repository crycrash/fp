using TagsCloudVisualization;

namespace DrawingTagsCloudVisualization;

public interface IImageSaver
{
    public Result<bool> SaveToFile(string filePath, int lenght, int width, string color, List<RectangleInformation> rectangleInformation);
}