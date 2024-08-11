using Conversion.Enums;
using Conversion.Extensions;
using FFmpeg.NET;
using FFmpeg.NET.Enums;
using FoolProof.Core;

namespace Conversion.Models.OutputSettings;

public class OutputSetting : IOutputSetting
{
	public int? MaxBitrate { get; set; }
	[RequiredIfEmpty("MaxWidth")]
	public int? MaxHeight { get; set; }
	[RequiredIfEmpty("MaxHeight")]
	public int? MaxWidth { get; set; }
	public bool WithAudio { get; set; }
	public bool WithSubtitles { get; set; }
	public OutputFormats OutputFormat { get; set; }
	public RawOutputPresetNames? OutputPresetName { get; set; }
}
