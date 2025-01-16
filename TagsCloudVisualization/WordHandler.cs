using TagsCloudVisualization.FilesProcessing;
namespace TagsCloudVisualization;

public class WordHandler : IWordHandler
{
    public WordHandler(IMorphologicalAnalyzer morphologicalAnalyzer, IFileProcessor fileProcessor)
    {
        _morphologicalAnalyzer = morphologicalAnalyzer;
        _fileProcessor = fileProcessor;
    }
    private readonly Dictionary<string, int> keyValueWords = [];
    private readonly IMorphologicalAnalyzer _morphologicalAnalyzer;
    private readonly IFileProcessor _fileProcessor;

    public Dictionary<string, int> ProcessFile(string filePath, string option)
    {
        var words = _fileProcessor.ReadWords(filePath);

        foreach (var word in words)
        {
            var normalizedWord = word.ToLower();
            if (!_morphologicalAnalyzer.IsExcludedWord(word, option))
            {
                if (keyValueWords.ContainsKey(normalizedWord))
                    keyValueWords[normalizedWord]++;
                else
                    keyValueWords[normalizedWord] = 1;
            }
        }
        return keyValueWords;
    }
}