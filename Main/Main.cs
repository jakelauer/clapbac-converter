using Microsoft.Extensions.DependencyInjection;
using Conversion.Services;
using CommandLine;
using FoolProof.Core;
using MainArgs;
using Director;

class Program
{
	static void Main(string[] args)
	{
		Parser.Default.ParseArguments<Options>(args)
			  .WithParsed(RunOptions);
	}

	static async void RunOptions(Options opts)
	{
		// Set up the service collection
		var serviceCollection = new ServiceCollection();
		ConfigureServices(serviceCollection);

		// Build the service provider
		var serviceProvider = serviceCollection.BuildServiceProvider();

		// Your application logic here
		var importer = serviceProvider.GetService<IImporter>();
		var converter = serviceProvider.GetService<IConverter>();

		if (importer == null || converter == null)
		{
			Console.WriteLine("Failed to get services");
			return;
		}

		await importer.Import(converter, opts.Input, opts.Output);

		// Use the input and output arguments
		Console.WriteLine($"Input: {opts.Input}");
		Console.WriteLine($"Output: {opts.Output}");

		Console.ReadLine();
	}

	static void ConfigureServices(IServiceCollection services)
	{
		// Add FoolProof services if needed
		services.AddFoolProof();

		// Register your own services
		services.AddTransient<Converter>();
	}
}
