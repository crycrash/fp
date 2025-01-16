using System.Text.RegularExpressions;
using MyStemWrapper;

namespace TagsCloudVisualization;

public class MorphologicalProcessing(MyStem mystem) : IMorphologicalAnalyzer
{
    private readonly MyStem _mystem = mystem;

    public bool IsExcludedWord(string word, string excludedPartOfSpeech)
    {
        var analysisResult = _mystem.Analysis(word);

        if (string.IsNullOrWhiteSpace(excludedPartOfSpeech))
        {
            return ContainsPartOfSpeech(analysisResult, ["CONJ", "INTJ", "PART", "PR", "SPRO", "COM"]);
        }
        var excludedParts = excludedPartOfSpeech.Split(',', StringSplitOptions.RemoveEmptyEntries)
                                                .Select(part => part.Trim().ToUpper())
                                                .ToArray();

        return ContainsPartOfSpeech(analysisResult, [.. excludedParts, "CONJ", "INTJ", "PART", "PR", "SPRO", "COM"]);
    }

    private static bool ContainsPartOfSpeech(string analysisResult, string[] partsOfSpeech)
    {
        foreach (var part in partsOfSpeech)
        {
            var regex = $@"\b.*?={Regex.Escape(part)}\W";
            if (Regex.IsMatch(analysisResult, regex, RegexOptions.IgnoreCase))
            {
                return true;
            }
        }
        return false;
    }
}
