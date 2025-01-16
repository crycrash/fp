using System.Drawing;

namespace TagsCloudVisualization;

public interface IRectangleGenerator
{
    public List<RectangleInformation> ExecuteRectangles(Dictionary<string, int> frequencyRectangles, Point center);
}