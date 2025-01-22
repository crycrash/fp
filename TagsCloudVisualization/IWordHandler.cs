namespace TagsCloudVisualization;

public interface IWordHandler
{
    public Result<Dictionary<string, int>> ProcessFile(string filePath, string option);
}