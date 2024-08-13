using FFmpeg.NET.Enums;

namespace Conversion.Models;

using DimensionTuple = (int Width, int Height, VideoSize FFmpegClosestMatch);

public struct Dimension
{
	public int Width { get; set; }
	public int Height { get; set; }
	public VideoSize FFmpegClosestMatch { get; set; }

	public static implicit operator Dimension(DimensionTuple value)
	{
		return new Dimension
		{
			Width = value.Width,
			Height = value.Height,
			FFmpegClosestMatch = value.FFmpegClosestMatch,
		};
	}
}
