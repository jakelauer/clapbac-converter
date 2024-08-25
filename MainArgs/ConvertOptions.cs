using CommandLine;
using Conversion.Enums;

namespace MainArgs;

[Verb("convert", HelpText = "Convert video with subtitle.")]
public class ConvertOptions
{
	[Option('v', "video", Required = false, HelpText = "Input video path.")]
	public string Video { get; set; } = string.Empty;

	[Option('s', "subtitle", Required = true, HelpText = "Subtitle file path.")]
	public string Subtitle { get; set; } = string.Empty;

	[Option('f', "formats", Required = false, HelpText = "Output formats.", Separator = ',')]
	public OutputFormats[] Formats { get; set; } = [
		OutputFormats.Mp4,
		OutputFormats.Webm,
		OutputFormats.Gif,
		OutputFormats.Webp
	];
}
