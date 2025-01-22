using System.Drawing;
using System.Runtime.InteropServices;
using Autofac;
using DrawingTagsCloudVisualization;
using TagsCloudVisualization;
using TagsCloudVisualization.FilesProcessing;
using TagsCloudVisualization.ManagingRendering;

namespace ConsoleClient;

public static class DependencyInjectionConfig
{
    public static Result<IContainer> BuildContainer(Options options)
    {
        var builder = new ContainerBuilder();
        var resultFile = FindMystemPath("mystem");
        if (!resultFile.IsSuccess)
        {
            return Result.Fail<IContainer>(resultFile.Error);
        }
        var pathToMyStem = resultFile.GetValueOrThrow();
        RegisterMystem(builder, pathToMyStem);
        RegisterProcessingComponents(builder);
        RegisterSpiral(builder, options);
        RegisterDrawingComponents(builder, options);
        return Result.Ok<IContainer>(builder.Build());
    }

    public static Result<String> FindMystemPath(string path)
    {
        var pathToMystem = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "mystem.exe" : path;

        if (!File.Exists(pathToMystem))
        {
            return Result.Fail<string>($"Не удалось найти файл");
        }

        return Result.Ok(pathToMystem);
    }

    private static ContainerBuilder RegisterMystem(ContainerBuilder builder, string pathToMystem)
    {
        builder.RegisterInstance(new MyStemWrapper.MyStem
        {
            PathToMyStem = pathToMystem,
            Parameters = "-ni"
        }).As<MyStemWrapper.MyStem>().SingleInstance();

        return builder;
    }

    private static ContainerBuilder RegisterProcessingComponents(ContainerBuilder builder)
    {
        builder.RegisterType<MorphologicalProcessing>()
                .As<IMorphologicalAnalyzer>()
                .InstancePerDependency();

        builder.RegisterType<TxtFileProcessor>()
            .As<IFileProcessor>()
            .InstancePerDependency();
        return builder;
    }

    private static ContainerBuilder RegisterSpiral(ContainerBuilder builder, Options options)
    {
        builder.Register<ISpiral>(c =>
            {
                var centerPoint = new Point(options.CenterX, options.CenterY);
                return options.AlgorithmForming switch
                {
                    "Circle" => new ArchimedeanSpiral(centerPoint, 1),
                    _ => new FermatSpiral(centerPoint, 20),
                };
            }).As<ISpiral>().InstancePerDependency();
        return builder;
    }

    private static ContainerBuilder RegisterDrawingComponents(ContainerBuilder builder, Options options)
    {
        builder.RegisterType<WordHandler>()
                .As<IWordHandler>()
                .InstancePerDependency();

        builder.RegisterType<RectangleGenerator>()
            .As<IRectangleGenerator>()
            .InstancePerDependency();

        builder.RegisterType<TagsCloudDrawingFacade>()
            .As<ITagsCloudDrawingFacade>()
            .InstancePerDependency();

        builder.RegisterType<ImageSaver>()
            .As<IImageSaver>()
            .InstancePerDependency();

        switch (options.AlgorithmDrawing)
        {
            case "Altering":
                builder.RegisterType<AlternatingColorsTagsCloudDrawer>().As<ITagsCloudDrawer>()
                    .InstancePerDependency();
                break;
            default:
                builder.RegisterType<StandartTagsCloudDrawer>().As<ITagsCloudDrawer>()
                    .InstancePerDependency();
                break;
        }
        return builder;
    }
}