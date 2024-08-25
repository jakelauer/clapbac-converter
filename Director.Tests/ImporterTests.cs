namespace Director.Tests;

using NUnit.Framework;
using Moq;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Conversion.Models;
using Conversion.Services;
using Director;
using Microsoft.Extensions.Logging;

[TestFixture]
public class ImporterTests
{
	private Mock<IConverter> _converterMock;
	private Mock<ILogger<IConversionDirector>> _loggerMock;
	private ConversionDirector _conversionDirector;

	[SetUp]
	public void SetUp()
	{
		_converterMock = new();
		_loggerMock = new();
		_conversionDirector = new ConversionDirector(_loggerMock.Object, _converterMock.Object);
	}

	[Test]
	public async Task Import_WithSingleFilePaths_CallsConvert()
	{
		// Arrange
		var videoFilePath = "video.mp4";
		var subtitleFilePath = "subtitle.srt";

		// Act
		await _conversionDirector.Import(videoFilePath, subtitleFilePath);

		// Assert
		_converterMock.Verify(c => c.Convert(It.IsAny<ConversionInput>()), Times.Once);
	}

	[Test]
	public async Task Import_WithMultipleFilePaths_CallsConvertForEach()
	{
		// Arrange
		var videoFilePaths = new List<string> { "video1.mp4", "video2.mp4" };
		var subtitleFilePaths = new List<string> { "subtitle1.srt", "subtitle2.srt" };

		// Act
		await _conversionDirector.Import(videoFilePaths, subtitleFilePaths);

		// Assert
		_converterMock.Verify(c => c.Convert(It.IsAny<ConversionInput>()), Times.Exactly(videoFilePaths.Count));
	}

	[Test]
	public async Task Import_WithSingleFilePath_CreatesCorrectConversionInput()
	{
		// Arrange
		var videoFilePath = "video.mp4";
		var subtitleFilePath = "subtitle.srt";

		// Act
		await _conversionDirector.Import(videoFilePath, subtitleFilePath);

		// Assert
		_converterMock.Verify(c => c.Convert(It.Is<ConversionInput>(input => input.InputFilePath.Contains(videoFilePath))), Times.Once);
	}

	[Test]
	public async Task Import_WithMultipleFilePaths_CreatesCorrectConversionInputs()
	{
		// Arrange
		var videoFilePaths = new List<string> { "video1.mp4", "video2.mp4" };
		var subtitleFilePaths = new List<string> { "subtitle1.srt", "subtitle2.srt" };

		// Act
		await _conversionDirector.Import(videoFilePaths, subtitleFilePaths);

		// Assert
		foreach (var videoFilePath in videoFilePaths)
		{
			_converterMock.Verify(c => c.Convert(It.Is<ConversionInput>(input => input.InputFilePath.Contains(videoFilePath))), Times.Once);
		}
	}
}
