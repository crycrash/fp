using Microsoft.Maui.Graphics;
using TagsCloudVisualization;

namespace DrawingTagsCloudVisualization;

public class AlternatingColorsTagsCloudDrawer() : ITagsCloudDrawer
{
    private readonly List<Color> colors = new List<Color>
        {
            Colors.White,
            Colors.Red,
            Colors.Green,
            Colors.Yellow,
            Colors.Blue,
            Colors.Pink,
            Colors.Black
        };

    public Result<ICanvas> Draw(ICanvas canvas, string color, List<RectangleInformation> rectangleInformation, int lenght, int width)
    {
        return Result.Of(() =>
        {
            if (rectangleInformation.Count == 0)
                throw new ArgumentException("Empty list of rectangles");
            for (int i = 0; i < rectangleInformation.Count; i++)
            {
                var rectInfo = rectangleInformation[i];
                var rect = rectInfo.rectangle;
                var text = rectInfo.word;

                var currentColor = colors[i % colors.Count];
                canvas.FontColor = currentColor;

                float fontSize = rect.Height;
                var textBounds = canvas.GetStringSize(text, Font.Default, fontSize);

                while ((textBounds.Width > rect.Width || textBounds.Height > rect.Height) && fontSize > 1)
                {
                    fontSize -= 1;
                    textBounds = canvas.GetStringSize(text, Font.Default, fontSize);
                }

                if (fontSize <= 1)
                {
                    throw new InvalidOperationException($"Cannot fit text '{text}' into the rectangle {rect}");
                }

                canvas.FontSize = fontSize;
                var textX = rect.X + (rect.Width - textBounds.Width) / 2;
                var textY = rect.Y + (rect.Height - textBounds.Height) / 2;
                if (textX < 0 || textY < 0 || textX + textBounds.Width > width || textY + textBounds.Height > lenght)
                    throw new InvalidOperationException("Text goes out of the canvas bounds");

                canvas.DrawString(text, textX, textY, HorizontalAlignment.Left);
            }

            return canvas;
        }, "Ошибка при рисовании облака тегов");
    }
}