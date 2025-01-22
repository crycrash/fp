namespace TagsCloudVisualization.FilesProcessing;
public class TxtFileProcessor : IFileProcessor
{
    public Result<IEnumerable<string>> ReadWords(string filePath)
    {
        var result = Result.Of(() =>
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException(filePath);

            return File.ReadLines(filePath)
                    .Select(line => line.Trim())
                    .Where(word => !string.IsNullOrEmpty(word));
        }, "File not found");
        return result;
    }
}