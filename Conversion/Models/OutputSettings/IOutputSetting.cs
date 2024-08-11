using Conversion.Enums;

namespace Conversion.Models.OutputSettings;

public interface IOutputSetting
{
	public int? MaxWidth
	{
		get
		{
			return MaxWidth;
		}

		set
		{
			if (value < 0 || value > 3840)
			{
				throw new ArgumentOutOfRangeException("MaxWidth");
			}
		}
	}
	public int? MaxHeight { get; set; }
	public int? MaxBitrate { get; set; }
	public bool WithAudio { get; set; }
	public bool WithSubtitles { get; set; }
	public OutputFormats OutputFormat { get; set; }
}
