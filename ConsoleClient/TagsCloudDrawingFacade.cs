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
        var resultProcessFile = _wordHandler.ProcessFile(options.InputFilePath, options.ExcludedPartOfSpeech);
        if (!resultProcessFile.IsSuccess){
            Console.WriteLine(resultProcessFile.Error);
            return;
        }
        var frequencyRectangles = resultProcessFile.GetValueOrThrow();

        var resultExecute = _rectangleGenerator.ExecuteRectangles(frequencyRectangles, new Point(options.CenterX, options.CenterY));
        if (!resultExecute.IsSuccess){
            Console.WriteLine(resultProcessFile.Error);
            return;
        }
        var arrRect = resultExecute.GetValueOrThrow();

        var resultSave = _imageSaver.SaveToFile(options.OutputFilePath, options.Length, options.Width, options.Color, arrRect);

        if (!resultSave.IsSuccess){
            Console.WriteLine(resultSave.Error);
            return;
        }
    }
}
