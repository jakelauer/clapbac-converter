using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Conversion.Models;
using Conversion.Services;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;

namespace ConvertQueue.Tests;

[TestFixture]
public class ConvertQueueTests
{
	private Mock<IConverter> _mockConverter;
	private Mock<ILogger<ConvertQueue>> _mockLogger;
	private List<IConversionInput> _mockQueue;
	private ConvertQueue _convertQueue;

	[SetUp]
	public void SetUp()
	{
		_mockConverter = new Mock<IConverter>();
		_mockLogger = new Mock<ILogger<ConvertQueue>>();
		_mockQueue = new List<IConversionInput> { new Mock<IConversionInput>().Object, new Mock<IConversionInput>().Object };
		_convertQueue = new ConvertQueue(_mockConverter.Object, _mockQueue, _mockLogger.Object);
	}

	[Test]
	public void ConvertQueue_InitializesCorrectly()
	{
		Assert.IsNotNull(_convertQueue);
	}

	[Test]
	public async Task ProcessQueue_LogsCorrectInformation()
	{
		await _convertQueue.ProcessQueue();

		_mockLogger.Verify(logger => logger.Log(
			LogLevel.Information,
			It.IsAny<EventId>(),
			It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Processing queue of")),
			It.IsAny<Exception>(),
			It.IsAny<Func<It.IsAnyType, Exception?, string>>()), Times.Once);

		_mockLogger.Verify(logger => logger.Log(
			LogLevel.Information,
			It.IsAny<EventId>(),
			It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Processing item")),
			It.IsAny<Exception>(),
			It.IsAny<Func<It.IsAnyType, Exception?, string>>()), Times.Exactly(_mockQueue.Count));

		_mockLogger.Verify(logger => logger.Log(
			LogLevel.Information,
			It.IsAny<EventId>(),
			It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Processed item")),
			It.IsAny<Exception>(),
			It.IsAny<Func<It.IsAnyType, Exception?, string>>()), Times.Exactly(_mockQueue.Count));

		_mockLogger.Verify(logger => logger.Log(
			LogLevel.Information,
			It.IsAny<EventId>(),
			It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Queue processed in parallel")),
			It.IsAny<Exception>(),
			It.IsAny<Func<It.IsAnyType, Exception?, string>>()), Times.Once);
	}

	[Test]
	public async Task ProcessQueue_CallsConvertOnEachItem()
	{
		await _convertQueue.ProcessQueue();

		_mockConverter.Verify(converter => converter.Convert(It.IsAny<IConversionInput>()), Times.Exactly(_mockQueue.Count));
	}

	[Test]
	public async Task ProcessQueue_HandlesExceptionsCorrectly()
	{
		_mockConverter.Setup(converter => converter.Convert(It.IsAny<IConversionInput>())).ThrowsAsync(new System.Exception("Conversion failed"));

		await _convertQueue.ProcessQueue();

		_mockLogger.Verify(logger => logger.Log(
			LogLevel.Error,
			It.IsAny<EventId>(),
			It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Error processing item")),
			It.IsAny<Exception>(),
			It.IsAny<Func<It.IsAnyType, Exception?, string>>()), Times.Exactly(_mockQueue.Count));
	}
}
