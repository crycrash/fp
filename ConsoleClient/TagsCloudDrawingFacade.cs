using System.Drawing;
using DrawingTagsCloudVisualization;
using TagsCloudVisualization;
namespace ConsoleClient;

public class TagsCloudDrawingFacade(
    IWordHandler wordHandler,
    IRectangleGenerator rectangleGenerator, IImageSaver imageSaver) : ITagsCloudDrawingFacade
{
    private readonly IWordHandler _wordHandler = wordHandler;
    private readonly IRectangleGenerator _rectangleGenerator = rectangleGenerator;
    private readonly IImageSaver _imageSaver = imageSaver;

    public void DrawRectangle(Options options)
    {
        var frequencyRectangles = _wordHandler.ProcessFile(options.InputFilePath, options.ExcludedPartOfSpeech);
        var arrRect = _rectangleGenerator.ExecuteRectangles(frequencyRectangles, new Point(options.CenterX, options.CenterY));
        _imageSaver.SaveToFile(options.OutputFilePath, options.Length, options.Width, options.Color, arrRect);
    }
}
