using Conversion.Enums;
using FFmpeg.NET.Enums;

namespace Conversion.Models;

using DimensionTuple = (int Width, int Height, VideoSize FFmpegClosestMatch);

public readonly struct OutputSizes
{
	public static readonly Dictionary<RawOutputPresetNames, DimensionTuple> Presets = new()
	{
		[RawOutputPresetNames.Thumb] = (160, 90, VideoSize.Qqvga),
		[RawOutputPresetNames.LowSd] = (320, 180, VideoSize.Qvga),
		[RawOutputPresetNames.MedSd] = (640, 360, VideoSize.Vga),
		[RawOutputPresetNames.HighSd] = (960, 540, VideoSize.Qhd),
		[RawOutputPresetNames.Hd720] = (1280, 720, VideoSize.Hd720),
		[RawOutputPresetNames.Hd1080] = (1920, 1080, VideoSize.Hd1080),
		[RawOutputPresetNames.Hd2k] = (2560, 1440, VideoSize._2K),
		[RawOutputPresetNames.Hd4k] = (3840, 2160, VideoSize._4K),
	};

	public static DimensionTuple Get(RawOutputPresetNames presetName)
	{
		if (Presets.TryGetValue(presetName, out DimensionTuple value))
		{
			return value;
		}

		throw new InvalidOperationException($"Output preset {presetName} not found.");
	}

}
