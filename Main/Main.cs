using Microsoft.Extensions.DependencyInjection;
using Conversion.Services;
using CommandLine;
using FoolProof.Core;
using MainArgs;
using Director;
using Microsoft.Extensions.Logging;

namespace Main;

public class Program
{
	private static Func<SearchOptions, GlobSearch> _globSearchFactory = opts => new GlobSearch(opts);
	private static IServiceProvider? _serviceProvider;

	public static void SetGlobSearchFactory(Func<SearchOptions, GlobSearch> factory)
	{
		_globSearchFactory = factory;
	}

	public static void SetServiceProvider(IServiceProvider serviceProvider)
	{
		_serviceProvider = serviceProvider;
	}

	public static void Main(string[] args)
	{
		var result = Parser.Default.ParseArguments<ConvertOptions, QueueOptions, SearchOptions>(args)
			.WithParsed<ConvertOptions>(Convert)
			.WithParsed<QueueOptions>(Queue)
			.WithParsed<SearchOptions>(Search);
	}

	public static void Convert(ConvertOptions opts)
	{
		DoConversion(new List<string> { opts.Video }, new List<string> { opts.Subtitle }).Wait();
	}

	public static void Queue(QueueOptions opts)
	{
		DoConversion(opts.Video, opts.Subtitle).Wait();
	}

	public static void Search(SearchOptions opts)
	{
		var globSearch = _globSearchFactory(opts);
		var (videoFiles, subtitleFiles) = globSearch.SearchFiles();
		DoConversion(videoFiles, subtitleFiles).Wait();
	}

	public static async Task DoConversion(IEnumerable<string> videoFilePaths, IEnumerable<string> subtitleFilePaths)
	{
		// Set up the service collection
		var serviceCollection = new ServiceCollection();
		ConfigureServices(serviceCollection);

		// Build the service provider if not set
		var serviceProvider = _serviceProvider ?? serviceCollection.BuildServiceProvider();

		// Your application logic here
		var director = serviceProvider.GetService<IConversionDirector>()
			?? throw new NullReferenceException("Director is null");

		await director.Import(videoFilePaths, subtitleFilePaths);

		// Dispose of the service provider
		if (serviceProvider is IDisposable disposable)
		{
			disposable.Dispose();
		}
	}

	public static void ConfigureServices(IServiceCollection services)
	{
		// Add FoolProof services if needed
		services.AddFoolProof();

		// Register logging services
		services.AddLogging(configure =>
		{
			configure.AddConsole(); // You can add other providers, like Debug, EventSource, etc.
		});

		// Register your own services
		services.AddTransient<IConverter, Converter>();
		services.AddTransient<IConversionDirector, ConversionDirector>();
		services.AddTransient<IConversionEngine, FfmpegConversionAdapter>();
	}
}
