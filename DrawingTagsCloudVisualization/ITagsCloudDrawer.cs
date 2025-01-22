using Microsoft.Maui.Graphics;
using TagsCloudVisualization;

namespace DrawingTagsCloudVisualization;

public interface ITagsCloudDrawer
{
    public Result<ICanvas> Draw(ICanvas canvas, string color, List<RectangleInformation> rectangleInformation, int lenght, int width);
}