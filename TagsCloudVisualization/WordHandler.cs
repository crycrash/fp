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

    public Result<Dictionary<string, int>> ProcessFile(string filePath, string option)
    {
        var generalResult = Result.Of(() => {
            var result = _fileProcessor.ReadWords(filePath);
            if (!result.IsSuccess){
                Console.WriteLine(result.Error);
                throw new ArgumentException();
            }
            var words = result.GetValueOrThrow();
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
        });
        return generalResult;
    }
}