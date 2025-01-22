namespace TagsCloudVisualization.FilesProcessing;
public interface IFileProcessor
{
    Result<IEnumerable<string>> ReadWords(string filePath);
}
