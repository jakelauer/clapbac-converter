using CommandLine;
using Conversion.Enums;

namespace MainArgs;

public class Options
{
	[Option('i', "input", Required = true, HelpText = "Input file path.")]
	public string Input { get; set; } = string.Empty;

	[Option('o', "output", Required = true, HelpText = "Output file path.")]
	public string Output { get; set; } = string.Empty;

	[Option('f', "formats", Required = false, HelpText = "Output formats.", Separator = ',')]
	public OutputFormats[] Formats { get; set; } = [
		OutputFormats.Mp4,
		OutputFormats.Webm,
		OutputFormats.Gif,
		OutputFormats.Webp
	];
}
