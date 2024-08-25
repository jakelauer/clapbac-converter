using CommandLine;
using Conversion.Enums;

namespace MainArgs;

[Verb("search", HelpText = "Search pattern for videos.")]
public class SearchOptions
{
	[Option('g', "glob", Required = true, HelpText = "Search glob.")]
	public string SearchGlob { get; set; } = string.Empty;

	[Option('v', "ext-video", Required = true, HelpText = "Extension to search.")]
	public string[] VideoExtensions { get; set; } = [];

	[Option('s', "ext-subtitle", Required = true, HelpText = "Extension to search.")]
	public string[] SubtitleExtensions { get; set; } = [];

	[Option('p', "path", Required = false, HelpText = "Search path. Default is current directory.")]
	public string SearchPath { get; set; } = Directory.GetCurrentDirectory();

	[Option('f', "formats", Required = false, HelpText = "Output formats.", Separator = ',')]
	public OutputFormats[] Formats { get; set; } = [
		OutputFormats.Mp4,
		OutputFormats.Webm,
		OutputFormats.Gif,
		OutputFormats.Webp
	];
}
