using FluentAssertions;
using ConsoleClient;
using DrawingTagsCloudVisualization;
using TagsCloudVisualization;
using System.Drawing;
using Microsoft.Maui.Graphics.Skia;
using Microsoft.Maui.Graphics;

namespace TagsCloudVisualizationTests;

[TestFixture]
public class TestErrorHandling
{
    private StandartTagsCloudDrawer Drawer;
    private ICanvas Canvas;

    [SetUp]
    public void SetUp()
    {
        Drawer = new StandartTagsCloudDrawer();
        using var bitmapContext = new SkiaBitmapExportContext(400, 400, 2.0f);
        Canvas = bitmapContext.Canvas;
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
        result.Error.Should().Be("Ошибка при рисовании облака тегов");
    }

    [Test]
    public void Draw_ShouldReturnFailure_WhenListEmpty()
    {
        var rectangleInformation = new List<RectangleInformation>();

        var result = Drawer.Draw(Canvas, "red", rectangleInformation, 400, 400);
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be("Ошибка при рисовании облака тегов");
    }
}
