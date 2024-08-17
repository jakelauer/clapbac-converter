using Conversion.Models;
using Conversion.Services;

namespace Director;

public interface IImporter
{
	public Task Import(IConverter converter, string videoFilePath, string subtitleFilePath);
	public Task Import(IConverter converter, IEnumerable<string> videoFilePaths, IEnumerable<string> subtitleFilePaths);
}

public class Importer : IImporter
{
	public async Task Import(IConverter converter, string videoFilePath, string subtitleFilePath)
	{
		await Import(converter, [videoFilePath], [subtitleFilePath]);
	}

	public async Task Import(IConverter converter, IEnumerable<string> videoFilePaths, IEnumerable<string> subtitleFilePaths)
	{
		var videoFileInfos = GetVideoFileInfos(videoFilePaths);
		var subtitleFileInfos = GetSubtitleFileInfos(subtitleFilePaths);
		var presets = new OutputSettingPresets();

		var inputs = videoFileInfos.Select(videoFileInfo => new ConversionInput(videoFileInfo.FullName, presets));
		foreach (var input in inputs)
		{
			await converter.Convert(input);
		}
	}

	private IEnumerable<FileInfo> GetVideoFileInfos(IEnumerable<string> videoFilePaths)
	{
		return videoFilePaths.Select(path => new FileInfo(path));
	}

	private IEnumerable<FileInfo> GetSubtitleFileInfos(IEnumerable<string> subtitleFilePaths)
	{
		return subtitleFilePaths.Select(path => new FileInfo(path));
	}
}
