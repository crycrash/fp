using System.Drawing;

namespace TagsCloudVisualization;

public interface IRectangleGenerator
{
    public Result<List<RectangleInformation>> ExecuteRectangles(Dictionary<string, int> frequencyRectangles, Point center);
}