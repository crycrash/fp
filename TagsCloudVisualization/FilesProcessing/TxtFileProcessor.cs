namespace TagsCloudVisualization.FilesProcessing;
public class TxtFileProcessor : IFileProcessor
{
    public IEnumerable<string> ReadWords(string filePath)
    {
        if (!File.Exists(filePath))
            throw new FileNotFoundException(filePath);

        return File.ReadLines(filePath)
                   .Select(line => line.Trim())
                   .Where(word => !string.IsNullOrEmpty(word));
    }
}