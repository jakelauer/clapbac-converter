using System.Diagnostics;
using Conversion.Models;
using FFmpeg.NET;
using Microsoft.Extensions.Logging;

namespace Conversion.Services;

public class Converter(IConversionInput input, ILogger<Converter> logger, IConversionEngine? engine = null)
{
	private readonly ILogger<Converter> _logger = logger;
	private readonly IConversionEngine _engine = engine ?? new FfmpegConversionAdapter();
	private readonly IConversionInput _input = input;
	private readonly InputFile _inputFile = new(input.InputFilePath);

	public readonly Dictionary<int, string> OutputFileMap = [];
	public readonly Dictionary<int, Exception> OutputExceptions = [];

	public static Engine GetFFmpegEngine()
	{
		return new Engine("/opt/homebrew/bin/ffmpeg");
	}

	public async Task Convert()
	{
		try
		{
			await Task.WhenAll(_input.Outputs.Select((output, index) => FFmpegConvert(index, _inputFile, output)));
		}
		catch (Exception e)
		{
			_logger.LogError("Conversion failed: {message}", e.Message);
		}
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
