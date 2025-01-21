using FluentAssertions;
using ConsoleClient;

namespace TagsCloudVisualizationTests;

[TestFixture]
public class TestErrorHandling
{
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
}
