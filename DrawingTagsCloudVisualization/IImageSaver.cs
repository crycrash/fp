using TagsCloudVisualization;

namespace DrawingTagsCloudVisualization;

public interface IImageSaver
{
    public void SaveToFile(string filePath, int lenght, int width, string color, List<RectangleInformation> rectangleInformation);
}