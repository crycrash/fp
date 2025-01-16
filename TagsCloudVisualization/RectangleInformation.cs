using System.Drawing;

namespace TagsCloudVisualization;

public record RectangleInformation(Rectangle Rectangle, string Word)
{
    public readonly Rectangle rectangle = Rectangle;
    public readonly string word = Word;
}