using Microsoft.Maui.Graphics;
using TagsCloudVisualization;

namespace DrawingTagsCloudVisualization;

public interface ITagsCloudDrawer
{
    public ICanvas Draw(ICanvas canvas, string color, List<RectangleInformation> rectangleInformation);
}