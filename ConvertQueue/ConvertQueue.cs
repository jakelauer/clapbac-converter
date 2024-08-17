using Conversion.Models;
using Conversion.Services;
using Microsoft.Extensions.Logging;

namespace ConvertQueue;

/// <summary>
/// Processes an enumerable of IConversionInput requests as a queue via the Converter service.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="ConvertQueue"/> class.
/// </remarks>
/// <param name="queue">The queue of conversion requests.</param>
/// <param name="logger">The logger.</param>
/// <param name="engine">The conversion engine.</param>
public class ConvertQueue(
	IConverter converter,
	IEnumerable<IConversionInput> queue,
	ILogger<ConvertQueue> logger)
{
	private readonly IConverter _converter = converter;
	private readonly ILogger<ConvertQueue> _logger = logger;
	private readonly IEnumerable<IConversionInput> _queue = queue;

	/// <summary>
	/// Processes the queue of conversion requests.
	/// </summary>
	/// <returns>A task representing the asynchronous operation.</returns>
	public async Task ProcessQueue()
	{
		_logger.LogInformation("Processing queue of {count} items in parallel", _queue.Count());

		// Process in parallel and log before and after each conversion, and any exceptions
		await Task.WhenAll(_queue.Select(async (input, index) =>
		{
			try
			{
				_logger.LogInformation("Processing item {index}", index);

				await _converter.Convert(input);

				_logger.LogInformation("Processed item {index}", index);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error processing item {index}", index);
			}
		}));

		_logger.LogInformation("Queue processed in parallel");
	}
}
