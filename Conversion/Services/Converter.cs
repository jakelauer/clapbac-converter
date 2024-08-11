using System.Diagnostics;
using Conversion.Models;
using FFmpeg.NET;
using Microsoft.Extensions.Logging;

namespace Conversion.Services;

public class Converter(IConversionInput input, ILogger<Converter> logger)
{
	private readonly ILogger<Converter> _logger = logger;
	private readonly Engine _engine = new("/opt/homebrew/bin/ffmpeg");
	private readonly IConversionInput _input = input;
	private readonly InputFile _inputFile = new(input.InputFilePath);

	public async Task Convert()
	{
		foreach (var output in _input.Outputs)
		{
			await FFmpegConvert(_inputFile, output);
		}
	}

	private async Task FFmpegConvert(InputFile inputFile, IConversionOutput output)
	{
		var cancellationToken = new CancellationToken();
		var mediaFile = await _engine.ConvertAsync(inputFile, new OutputFile(output.OutputFilePath), cancellationToken);
		_logger.LogInformation("Output file: {filename}", mediaFile.FileInfo.FullName);
	}
}
