using Conversion.Models;
using Conversion.Models.OutputSettings;
using Conversion.Services;
using Microsoft.Extensions.Logging;

namespace Director;

public interface IConversionDirector
{
	public Task Import(string videoFilePath, string subtitleFilePath);
	public Task Import(IEnumerable<string> videoFilePaths, IEnumerable<string> subtitleFilePaths);
}

public class ConversionDirector(ILogger<IConversionDirector> logger, IConverter converter) : IConversionDirector
{
	private IConverter _converter { get; set; } = converter;
	private ILogger<IConversionDirector> _logger { get; set; } = logger;

	public async Task Import(string videoFilePath, string subtitleFilePath)
	{
		await Import([videoFilePath], [subtitleFilePath]);
	}

	public async Task Import(IEnumerable<string> videoFilePaths, IEnumerable<string> subtitleFilePaths)
	{
		_logger.LogInformation("Importing files: {videoFilePaths} and {subtitleFilePaths}", videoFilePaths, subtitleFilePaths);

		var videoFileInfos = GetVideoFileInfos(videoFilePaths);
		var subtitleFileInfos = GetSubtitleFileInfos(subtitleFilePaths);

		var inputs = videoFileInfos.Select(videoFileInfo => new ConversionInput(videoFileInfo.FullName));
		foreach (var input in inputs)
		{
			await _converter.Convert(input);
		}
	}

	private static IEnumerable<FileInfo> GetVideoFileInfos(IEnumerable<string> videoFilePaths)
	{
		return videoFilePaths.Select(path => new FileInfo(path));
	}

	private static IEnumerable<FileInfo> GetSubtitleFileInfos(IEnumerable<string> subtitleFilePaths)
	{
		return subtitleFilePaths.Select(path => new FileInfo(path));
	}
}
