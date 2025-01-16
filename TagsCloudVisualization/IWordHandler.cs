namespace TagsCloudVisualization;

public interface IWordHandler
{
    public Dictionary<string, int> ProcessFile(string filePath, string option);
}