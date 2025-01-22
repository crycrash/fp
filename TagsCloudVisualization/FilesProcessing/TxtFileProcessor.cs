namespace TagsCloudVisualization.FilesProcessing;
public class TxtFileProcessor : IFileProcessor
{
    public Result<IEnumerable<string>> ReadWords(string filePath)
    {
        if (!File.Exists(filePath))
        {
            return Result.Fail<IEnumerable<string>>("File not found");
        }
        try
        {
            var lines = File.ReadLines(filePath)
                            .Select(line => line.Trim())
                            .Where(word => !string.IsNullOrEmpty(word));
            return Result.Ok(lines);
        }
        catch (Exception)
        {
            return Result.Fail<IEnumerable<string>>("Error reading file");
        }
    }
}