using System;
using Microsoft.Extensions.DependencyInjection;
using FoolProof.Core;
using Conversion.Services;
using Director;

// Set up the service collection
var serviceCollection = new ServiceCollection();
ConfigureServices(serviceCollection);

// Build the service provider
var serviceProvider = serviceCollection.BuildServiceProvider();

// Your application logic here
var ClipBuilder = serviceProvider.GetService<ClipBuilder>();
ClipBuilder?.Build();

Console.ReadLine();

static void ConfigureServices(IServiceCollection services)
{
	// Add FoolProof services
	services.AddFoolProof();

	// Register your own services
	services.AddTransient<Converter>();
}
