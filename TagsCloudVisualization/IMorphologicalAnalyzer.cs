namespace TagsCloudVisualization;

public interface IMorphologicalAnalyzer
{
    bool IsExcludedWord(string word, string excludedPartOfSpeech);
}
