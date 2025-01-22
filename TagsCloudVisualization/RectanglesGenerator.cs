using System.Drawing;
using TagsCloudVisualization.ManagingRendering;

namespace TagsCloudVisualization;

public class RectangleGenerator(ISpiral spiral) : IRectangleGenerator
{
    readonly ISpiral spiral = spiral;
    private static readonly List<RectangleInformation> rectangleInformation = [];
    private static readonly Dictionary<string, Size> rectangleData = [];
    private static void GenerateRectangles(Dictionary<string, int> frequencyRectangles)
    {
        var totalCountWords = frequencyRectangles.Sum(x => x.Value);
        var sortedWords = frequencyRectangles.OrderByDescending(word => word.Value);
        foreach (var word in sortedWords)
        {
            var width = word.Value * 500 / totalCountWords;
            var height = word.Value * 300 / totalCountWords;
            var rectangleSize = new Size(Math.Max(width, 1), Math.Max(height, 1));
            rectangleData.Add(word.Key, rectangleSize);
        }
    }
    private Result<List<RectangleInformation>> PutRectangles(Point center)
    {
        var layouter = new CircularCloudLayouter(spiral, center);
        foreach (var rect in rectangleData)
        {
            var result = layouter.PutNextRectangle(rect.Value);
            if (!result.IsSuccess)
                return Result.Fail<List<RectangleInformation>>("Invalid rectangles");
            var tempRect = result.GetValueOrThrow();
            rectangleInformation.Add(new RectangleInformation(tempRect, rect.Key));
        }
        return Result.Ok(rectangleInformation);
    }
    public Result<List<RectangleInformation>> ExecuteRectangles(Dictionary<string, int> frequencyRectangles, Point center)
    {
        GenerateRectangles(frequencyRectangles);
        var result = PutRectangles(center);
        if (!result.IsSuccess)
            return Result.Fail<List<RectangleInformation>>("Invalid rectangles");
        return Result.Ok(result.GetValueOrThrow());
    }
}