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
        var resultFile = FindMystemPath();
        if (!resultFile.IsSuccess)
        {
            return Result.Fail<IContainer>(resultFile.Error);
        }
        var pathToMyStem = resultFile.GetValueOrThrow();
        var result = RegisterMystem(builder, pathToMyStem);
        if (!result.IsSuccess)
        {
            return Result.Fail<IContainer>(result.Error);
        }

        result = RegisterProcessingComponents(builder);
        if (!result.IsSuccess)
        {
            return Result.Fail<IContainer>(result.Error);
        }

        result = RegisterSpiral(builder, options);
        if (!result.IsSuccess)
        {
            return Result.Fail<IContainer>(result.Error);
        }

        result = RegisterDrawingComponents(builder, options);
        if (!result.IsSuccess)
        {
            return Result.Fail<IContainer>(result.Error);
        }

        return Result.Ok<IContainer>(builder.Build());
    }

    private static Result<String> FindMystemPath()
    {
        var containerResultFile = Result.Of(() =>
        {
            var pathToMystem = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "mystem.exe" : "mysem";

            if (!File.Exists(pathToMystem))
            {
                throw new FileNotFoundException();
            }
            return pathToMystem;
        }, "Не удалось найти файл");
        return containerResultFile;
    }

    private static Result<ContainerBuilder> RegisterMystem(ContainerBuilder builder, string pathToMystem)
    {
        var containerResult = Result.Of(() =>
        {
            builder.RegisterInstance(new MyStemWrapper.MyStem
            {
                PathToMyStem = pathToMystem,
                Parameters = "-ni"
            }).As<MyStemWrapper.MyStem>().SingleInstance();

            return builder;
        }, "Ошибка при регистрации MyStem");

        return containerResult;
    }

    private static Result<ContainerBuilder> RegisterProcessingComponents(ContainerBuilder builder)
    {
        var containerResult = Result.Of(() =>
        {
            builder.RegisterType<MorphologicalProcessing>()
                .As<IMorphologicalAnalyzer>()
                .InstancePerDependency();

            builder.RegisterType<TxtFileProcessor>()
                .As<IFileProcessor>()
                .InstancePerDependency();
            return builder;
        }, "Ошибка при регистрации компонентов обработки");

        return containerResult;
    }

    private static Result<ContainerBuilder> RegisterSpiral(ContainerBuilder builder, Options options)
    {
        var containerResult = Result.Of(() =>
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
        }, "Ошибка при регистрации спирали");

        return containerResult;
    }

    private static Result<ContainerBuilder> RegisterDrawingComponents(ContainerBuilder builder, Options options)
    {
        var containerResult = Result.Of(() =>
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
        }, "Ошибка при регистрации компонентов рисования");

        return containerResult;
    }
}