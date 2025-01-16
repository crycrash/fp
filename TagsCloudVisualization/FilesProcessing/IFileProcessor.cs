namespace TagsCloudVisualization.FilesProcessing;
public interface IFileProcessor
{
    IEnumerable<string> ReadWords(string filePath);
}
