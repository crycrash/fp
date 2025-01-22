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

    public Result<ICanvas> Draw(ICanvas canvas, string color, List<RectangleInformation> rectangleInformation, int lenght, int width)
    {
        if (rectangleInformation.Count == 0)
            return Result.Fail<ICanvas>("Empty list of rectangles");
        foreach (var rectInfo in rectangleInformation)
        {
            var rect = rectInfo.Rectangle;
            var text = rectInfo.Word;

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
            if (fontSize <= 1)
                return Result.Fail<ICanvas>($"Cannot fit text '{text}' into the rectangle {rect}");
            canvas.FontSize = fontSize;
            var textX = rect.X + (rect.Width - textBounds.Width) / 2;
            var textY = rect.Y + (rect.Height - textBounds.Height) / 2;
            if (textX < 0 || textY < 0 || textX + textBounds.Width > width || textY + textBounds.Height > lenght)
                return Result.Fail<ICanvas>("Text goes out of the canvas bounds");

            canvas.DrawString(text, textX, textY, HorizontalAlignment.Left);
        }
        
        return Result.Ok(canvas);
    }
}