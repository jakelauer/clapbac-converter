using CommandLine;
using Conversion.Enums;

namespace MainArgs;

[Verb("queue", HelpText = "Queue videos with subtitles.")]
public class QueueOptions
{
	[Option('v', "videos", Required = true, HelpText = "Input file path.")]
	public string[] Video { get; set; } = [];

	[Option('s', "subtitle", Required = true, HelpText = "Input file path.")]
	public string[] Subtitle { get; set; } = [];

	[Option('f', "formats", Required = false, HelpText = "Output formats.", Separator = ',')]
	public OutputFormats[] Formats { get; set; } = [
		OutputFormats.Mp4,
		OutputFormats.Webm,
		OutputFormats.Gif,
		OutputFormats.Webp
	];
}
