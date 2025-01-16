using System.Drawing;

namespace TagsCloudVisualization.ManagingRendering;

public class FermatSpiral : ISpiral
{
    private readonly Point startPoint;
    private readonly double scaleFactor;
    private double currentAngle;

    public FermatSpiral(Point startPoint, double scaleFactor = 1)
    {
        if (scaleFactor <= 0)
            throw new ArgumentOutOfRangeException(nameof(scaleFactor), "Scale factor must be positive");

        this.startPoint = startPoint;
        this.scaleFactor = scaleFactor;
        this.currentAngle = 0;
    }

    public Point GetNextPoint()
    {
        var radius = scaleFactor * Math.Sqrt(currentAngle);
        var x = (int)(startPoint.X + radius * Math.Cos(currentAngle));
        var y = (int)(startPoint.Y + radius * Math.Sin(currentAngle));
        
        currentAngle += Math.PI / 36;

        return new Point(x, y);
    }
}
