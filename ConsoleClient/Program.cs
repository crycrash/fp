using Autofac;
using CommandLine;

namespace ConsoleClient;


public class Program
{
    static void Main(string[] args)
    {
        Parser.Default.ParseArguments<Options>(args)
            .WithParsed(RunApplication)
            .WithNotParsed(HandleErrors);
    }
    readonly static private HashSet<string> validPartsOfSpeech =
        [
            "S", "V", "A", "ADV", "NUM", "SPRO", "ADVPRO", "ANUM"
        ];

    private static void RunApplication(Options options)
    {
        if (options.AlgorithmForming != "Circle" && options.AlgorithmForming != "Fermat")
        {
            Console.WriteLine($"Ошибка: Неизвестный алгоритм '{options.AlgorithmForming}'. Допустимые значения: 'Circle', 'Fermat'.");
            return;
        }
        if (!string.IsNullOrEmpty(options.ExcludedPartOfSpeech))
        {
            var excludedParts = options.ExcludedPartOfSpeech.Split(',', StringSplitOptions.RemoveEmptyEntries)
                                                            .Select(part => part.Trim().ToUpper());

            var invalidParts = excludedParts.Where(part => !validPartsOfSpeech.Contains(part)).ToList();

            if (invalidParts.Count != 0)
            {
                Console.WriteLine($"Ошибка: Неизвестные части речи '{string.Join(", ", invalidParts)}'");
                return;
            }
        }
        if (options.AlgorithmDrawing != "Standart" && options.AlgorithmDrawing != "Altering")
        {
            Console.WriteLine($"Ошибка: Неизвестный алгоритм рассказки '{options.AlgorithmDrawing}'");
            return;
        }
        var container = DependencyInjectionConfig.BuildContainer(options);
        if (!container.IsSuccess)
        {
            Console.WriteLine($"Ошибка при создании DI-контейнера: {container.Error}");
            return;
        }

        using var scope = container.GetValueOrThrow().BeginLifetimeScope();
        var drawingFacade = scope.Resolve<ITagsCloudDrawingFacade>();
        drawingFacade.DrawRectangle(options);
        Console.WriteLine($"Облако тегов успешно сохранено в файл: {options.OutputFilePath}");
    }

    private static void HandleErrors(IEnumerable<Error> errors)
    {
        Console.WriteLine("Ошибка при обработке аргументов командной строки.");
        foreach (var error in errors)
        {
            switch (error)
            {
                case UnknownOptionError unknownOptionError:
                    Console.WriteLine($"- Неизвестный параметр: {unknownOptionError.Token}");
                    break;
                case SetValueExceptionError setValueExceptionError:
                    Console.WriteLine($"- Ошибка установки значения: {setValueExceptionError.Exception.Message}");
                    break;
                case MissingValueOptionError missingValueOptionError:
                    Console.WriteLine($"- Отсутствует значение: {missingValueOptionError.NameInfo}");
                    break;
                default:
                    Console.WriteLine($"- Неизвестная ошибка: {error.GetType().Name}");
                    break;
            }
        }
    }
}
