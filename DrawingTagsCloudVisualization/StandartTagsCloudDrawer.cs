using Microsoft.Maui.Graphics;
using TagsCloudVisualization;

namespace DrawingTagsCloudVisualization;

public class StandartTagsCloudDrawer() : ITagsCloudDrawer
{
    private readonly Dictionary<string, Color> dictColors = new(){
            { "white", Colors.White },
            { "red", Colors.Red },
            { "green", Colors.Green },
            { "yellow", Colors.Yellow },
            { "blue", Colors.Blue },
            { "pink", Colors.Pink },
            { "black", Colors.Black },
    };

    public ICanvas Draw(ICanvas canvas, string color, List<RectangleInformation> rectangleInformation)
    {
        foreach (var rectInfo in rectangleInformation)
        {
            var rect = rectInfo.rectangle;
            var text = rectInfo.word;

            float fontSize = rect.Height;
            if (!dictColors.TryGetValue(color, out var colorGet))
                colorGet = Colors.Black;
            canvas.FontColor = colorGet;
            var textBounds = canvas.GetStringSize(text, Font.Default, fontSize);

            while ((textBounds.Width > rect.Width || textBounds.Height > rect.Height) && fontSize > 1)
            {
                fontSize -= 1;
                textBounds = canvas.GetStringSize(text, Font.Default, fontSize);
            }

            canvas.FontSize = fontSize;
            var textX = rect.X + (rect.Width - textBounds.Width) / 2;
            var textY = rect.Y + (rect.Height - textBounds.Height) / 2;

            canvas.DrawString(text, textX, textY, HorizontalAlignment.Left);
        }

        return canvas;
    }
}