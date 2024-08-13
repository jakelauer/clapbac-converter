using Conversion.Models;
using Conversion.Services;
using FFmpeg.NET;
using Microsoft.Extensions.Logging;
using Moq;

namespace Conversion.Tests;

class MockEngineAdapter : IConversionEngine
{
	public Task<MediaFile> ConvertAsync(IInputArgument input, OutputFile output, CancellationToken cancellationToken)
	{
		return Task.FromResult(It.IsAny<MediaFile>());
	}

	public Task<MediaFile> ConvertAsync(IInputArgument input, OutputFile output, ConversionOptions options, CancellationToken cancellationToken)
	{
		return Task.FromResult(It.IsAny<MediaFile>());
	}
}

[TestFixture]
public class ConverterTests
{
	private Mock<ILogger<Converter>> _mockLogger;
	private Mock<IConversionInput> _mockInput;
	private Converter _converter;

	[SetUp]
	public void SetUp()
	{
		_mockLogger = new Mock<ILogger<Converter>>();
		_mockInput = new Mock<IConversionInput>();
		_mockInput.Setup(input => input.InputFilePath).Returns("input.mp4");
		_mockInput.Setup(input => input.Outputs).Returns([
			new Mock<IConversionOutput>().Object,
			new Mock<IConversionOutput>().Object
		]);

		_converter = new Converter(_mockInput.Object, _mockLogger.Object);
	}

	[Test]
	public async Task Convert_CallsFFmpegConvertForEachOutput()
	{
		var mockEngine = new MockEngineAdapter();
		var ffmpegConvertMock = new Mock<FfmpegConversionAdapter>();

		var converter = new Converter(_mockInput.Object, _mockLogger.Object, ffmpegConvertMock.Object);

		await converter.Convert();

		ffmpegConvertMock.Verify(ffmpeg => ffmpeg.ConvertAsync(
			It.IsAny<IInputArgument>(),
			It.IsAny<OutputFile>(),
			It.IsAny<CancellationToken>()
		), Times.Exactly(converter.OutputFileMap.Count));
	}

	[Test]
	public async Task FFmpegConvert_LogsCorrectInformation()
	{
		var outputMock = new Mock<IConversionOutput>();
		outputMock.Setup(output => output.OutputFilePath).Returns("output.mp4");

		var inputFile = new InputFile("input.mp4");
		var outputFile = new OutputFile("output.mp4");

		var convertSpy = new Mock<Converter>(_mockInput.Object, _mockLogger.Object) { CallBase = true };

		var converter = new Converter(_mockInput.Object, _mockLogger.Object);
		await converter.Convert();

		Assert.That(_mockInput.Object.Outputs, Has.Length.EqualTo(converter.OutputFileMap.Count + converter.OutputExceptions.Count));
	}
}
