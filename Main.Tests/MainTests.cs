using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using MainArgs;
using Director;
using Conversion.Services;

namespace Main.Tests;

[TestFixture]
public class MainTests
{
	private Mock<IConversionDirector> _mockDirector;
	private ServiceCollection _services;

	[SetUp]
	public void Setup()
	{
		_mockDirector = new Mock<IConversionDirector>();
		_services = new ServiceCollection();
		_services.AddTransient(_ => _mockDirector.Object);
	}

	[Test]
	public void Convert_CallsDoConversionWithCorrectParameters()
	{
		var opts = new ConvertOptions { Video = "video.mp4", Subtitle = "subtitle.srt" };
		// Ensure the Program class uses the service provider to get the IConversionDirector
		var serviceProvider = _services.BuildServiceProvider();
		Program.SetServiceProvider(serviceProvider);

		Program.Convert(opts);
		_mockDirector.Verify(d => d.Import(It.Is<IEnumerable<string>>(v => v.Contains("video.mp4")), It.Is<IEnumerable<string>>(s => s.Contains("subtitle.srt"))), Times.Once);
	}

	[Test]
	public void Queue_CallsDoConversionWithCorrectParameters()
	{
		var opts = new QueueOptions
		{
			Video = ["video.mp4"],
			Subtitle = ["subtitle.srt"]
		};
		// Ensure the Program class uses the service provider to get the IConversionDirector
		var serviceProvider = _services.BuildServiceProvider();
		Program.SetServiceProvider(serviceProvider);

		Program.Queue(opts);

		_mockDirector.Verify(d => d.Import(
			It.Is<IEnumerable<string>>(v => v.Contains("video.mp4")),
			It.Is<IEnumerable<string>>(s => s.Contains("subtitle.srt"))
		), Times.Once);
	}

	[Test]
	public void Search_CallsDoConversionWithCorrectParameters()
	{
		var opts = new SearchOptions { SearchGlob = "*.mp4" };
		var mockGlobSearch = new Mock<GlobSearch>();
		mockGlobSearch.Setup(g => g.SearchFiles()).Returns((new List<string> { "video.mp4" }, new List<string> { "subtitle.srt" }));

		Program.SetGlobSearchFactory(_ => mockGlobSearch.Object);

		// Ensure the Program class uses the service provider to get the IConversionDirector
		var serviceProvider = _services.BuildServiceProvider();
		Program.SetServiceProvider(serviceProvider);

		Program.Search(opts);

		_mockDirector.Verify(d => d.Import(
			It.Is<IEnumerable<string>>(v => v.Contains("video.mp4")),
			It.Is<IEnumerable<string>>(s => s.Contains("subtitle.srt"))
		), Times.Once);
	}

	[Test]
	public async Task DoConversion_SetsUpServicesAndCallsImport()
	{
		var videoFiles = new List<string> { "video.mp4" };
		var subtitleFiles = new List<string> { "subtitle.srt" };
		// Ensure the Program class uses the service provider to get the IConversionDirector
		var serviceProvider = _services.BuildServiceProvider();
		Program.SetServiceProvider(serviceProvider);

		await Program.DoConversion(videoFiles, subtitleFiles);
		_mockDirector.Verify(d => d.Import(It.Is<IEnumerable<string>>(v => v.Contains("video.mp4")), It.Is<IEnumerable<string>>(s => s.Contains("subtitle.srt"))), Times.Once);
	}

	[Test]
	public void ConfigureServices_RegistersServicesCorrectly()
	{
		Program.ConfigureServices(_services);
		var serviceProvider = _services.BuildServiceProvider();
		Assert.Multiple(() =>
		{
			Assert.That(serviceProvider.GetService<IConverter>(), Is.Not.Null);
			Assert.That(serviceProvider.GetService<IConversionDirector>(), Is.Not.Null);
			Assert.That(serviceProvider.GetService<IConversionEngine>(), Is.Not.Null);
			Assert.That(serviceProvider.GetService<ILogger<Program>>(), Is.Not.Null);
		});
	}
}
