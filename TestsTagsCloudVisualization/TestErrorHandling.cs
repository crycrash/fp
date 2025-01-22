using FluentAssertions;
using ConsoleClient;
using DrawingTagsCloudVisualization;
using TagsCloudVisualization;
using System.Drawing;
using Microsoft.Maui.Graphics.Skia;
using Microsoft.Maui.Graphics;
using TagsCloudVisualization.ManagingRendering;

namespace TagsCloudVisualizationTests;

[TestFixture]
public class TestErrorHandling
{
    private StandartTagsCloudDrawer Drawer;
    private ICanvas Canvas;
    private IImageSaver ImageSaver;
    private List<RectangleInformation> RectangleInformation;
    private CircularCloudLayouter Layouter;

    [SetUp]
    public void SetUp()
    {
        Drawer = new StandartTagsCloudDrawer();
        using var bitmapContext = new SkiaBitmapExportContext(400, 400, 2.0f);
        Canvas = bitmapContext.Canvas;
        ImageSaver = new ImageSaver(Drawer);
        RectangleInformation = new List<RectangleInformation>
        {
            new RectangleInformation(new Rectangle(0, 0, 100, 50), "Test")
        };
        var point = new System.Drawing.Point(100, 100);
        Layouter = new CircularCloudLayouter(new ArchimedeanSpiral(point), point);
    }
    [Test]
    public void BuildContainer_ShouldReturnFailure_WhenMystemPathIsInvalid()
    {
        var result = DependencyInjectionConfig.FindMystemPath("nomystem");

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be("Не удалось найти файл");
    }

    [Test]
    public void RegisterMystem_ShouldRegisterSuccessfully_WhenFileExists()
    {
        var validPath = "/Users/milana/fp/ConsoleClient/mystem";
        var result = DependencyInjectionConfig.FindMystemPath(validPath);
        result.IsSuccess.Should().BeTrue();
    }

    [Test]
    public void Draw_ShouldBeSuccessfull_WhenGoingBeyond()
    {
        var rectangleInformation = new List<RectangleInformation>
        {
            new RectangleInformation(new Rectangle(700, 100, 10, 15), "Test1"),
            new RectangleInformation(new Rectangle(2000, 40, 10, 10), "Test2")
        };

        var result = Drawer.Draw(Canvas, "red", rectangleInformation, 400, 400);

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be("Text goes out of the canvas bounds");
    }

    [Test]
    public void Draw_ShouldReturnFailure_WhenListEmpty()
    {
        var rectangleInformation = new List<RectangleInformation>();

        var result = Drawer.Draw(Canvas, "red", rectangleInformation, 400, 400);
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be("Empty list of rectangles");
    }

    [Test]
    public void SaveToFile_ShouldReturnSuccess_WhenParametersAreValid()
    {
        var filePath = "output.png";
        var length = 800;
        var width = 600;
        var color = "red";

        var result = ImageSaver.SaveToFile(filePath, length, width, color, RectangleInformation);

        result.IsSuccess.Should().BeTrue();
        result.GetValueOrThrow().Should().BeTrue();
        File.Exists(filePath).Should().BeTrue();
        File.Delete(filePath);
    }

    [Test]
    public void SaveToFile_ShouldFail_WhenFilePathIsEmpty()
    {
        var filePath = "";
        var length = 800;
        var width = 600;
        var color = "red";

        var result = ImageSaver.SaveToFile(filePath, length, width, color, RectangleInformation);

        result.IsSuccess.Should().BeFalse();
    }

    [Test]
    public void SaveToFile_ShouldFail_WhenCanvasDimensionsAreInvalid()
    {
        var filePath = "output.png";
        var length = -800;
        var width = 600;
        var color = "red";

        var result = ImageSaver.SaveToFile(filePath, length, width, color, RectangleInformation);
        result.IsSuccess.Should().BeFalse();
    }

    [Test]
    public void SaveToFile_ShouldFail_WhenFileCannotBeSaved()
    {
        var filePath = "/invalid_path/output.png";
        var length = 800;
        var width = 600;
        var color = "red";

        var result = ImageSaver.SaveToFile(filePath, length, width, color, RectangleInformation);

        result.IsSuccess.Should().BeFalse();
    }
    [Test]
    public void PutNextRectangle_ShouldReturnRectangle_WhenSizeIsValid()
    {
        var rectangleSize = new System.Drawing.Size(50, 30);
        var result = Layouter.PutNextRectangle(rectangleSize);

        result.IsSuccess.Should().BeTrue();
        result.GetValueOrThrow().Size.Should().Be(rectangleSize);
    }

    [Test]
    public void PutNextRectangle_ShouldFail_WhenSizeIsEmpty()
    {
        var rectangleSize = System.Drawing.Size.Empty;

        var result = Layouter.PutNextRectangle(rectangleSize);

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be("Invalid rectangle");
    }

    [Test]
    public void PutNextRectangle_ShouldFail_WhenSizeHasNegativeWidth()
    {
        var rectangleSize = new System.Drawing.Size(-10, 30);

        var result = Layouter.PutNextRectangle(rectangleSize);

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be("Invalid rectangle");
    }

    [Test]
    public void PutNextRectangle_ShouldPlaceRectanglesWithoutIntersection()
    {
        var rectangleSize1 = new System.Drawing.Size(50, 30);
        var rectangleSize2 = new System.Drawing.Size(60, 40);

        var result1 = Layouter.PutNextRectangle(rectangleSize1);
        var result2 = Layouter.PutNextRectangle(rectangleSize2);

        result1.IsSuccess.Should().BeTrue();
        result2.IsSuccess.Should().BeTrue();

        var rect1 = result1.GetValueOrThrow();
        var rect2 = result2.GetValueOrThrow();

        rect1.IntersectsWith(rect2).Should().BeFalse();
    }

    [Test]
    public void PutNextRectangle_ShouldMoveRectangleCloserToCenter_WhenPossible()
    {
        var rectangleSize = new System.Drawing.Size(50, 30);

        var result = Layouter.PutNextRectangle(rectangleSize);

        result.IsSuccess.Should().BeTrue();
        var placedRectangle = result.GetValueOrThrow();

        var distanceToCenter = Math.Sqrt(Math.Pow(placedRectangle.X, 2) + Math.Pow(placedRectangle.Y, 2));
        distanceToCenter.Should().BeGreaterOrEqualTo(0);
    }
}
