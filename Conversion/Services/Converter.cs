using System.Diagnostics;
using Conversion.Models;
using FFmpeg.NET;
using Microsoft.Extensions.Logging;

namespace Conversion.Services;

public interface IConverter
{
	Task Convert(IConversionInput input);
}

public class Converter(ILogger<IConverter> logger, IConversionEngine? engine = null) : IConverter
{
	private readonly ILogger<IConverter> _logger = logger;
	private readonly IConversionEngine _engine = engine ?? new FfmpegConversionAdapter();

	public readonly Dictionary<int, string> OutputFileMap = [];
	public readonly Dictionary<int, Exception> OutputExceptions = [];

	public static Engine GetFFmpegEngine()
	{
		return new Engine("/opt/homebrew/bin/ffmpeg");
	}

	public async Task Convert(IConversionInput input)
	{
		InputFile inputFile = new(input.InputFilePath);

		await Task.WhenAll(input.Outputs.Select((output, index) => FFmpegConvert(index, inputFile, output)));
	}

	private async Task FFmpegConvert(int index, InputFile inputFile, IConversionOutput output)
	{
		try
		{
			var cancellationToken = new CancellationToken();
			var mediaFile = await _engine.ConvertAsync(inputFile, new OutputFile(output.OutputFilePath), cancellationToken);
			OutputFileMap[index] = mediaFile.FileInfo.FullName;

			_logger.LogInformation("Output file: {filename}", mediaFile.FileInfo.FullName);
		}
		catch (Exception e)
		{
			_logger.LogError("Conversion failed for output {index}: {message}", index, e.Message);
			OutputExceptions[index] = e;
		}
	}
}
