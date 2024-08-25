using Conversion.Enums;
using Conversion.Models;
using Microsoft.Extensions.Logging;

namespace Conversion.Models.OutputSettings;

public class OutputSettingPresets : Dictionary<RawOutputPresetNames, IEnumerable<IOutputSetting>>
{
	public IEnumerable<IOutputSetting> GetPreset(RawOutputPresetNames presetName)
	{
		return this[presetName];
	}

	internal OutputSettingPresets()
	{
		GeneratePresets();
	}

	public struct PresetContext
	{
		public RawOutputPresetNames Name;
		public Dimension Dimension;
		public OutputFormats OutputFormat;
		public bool WithAudio;
		public bool WithSubtitles;
	}

	/// <summary>
	/// Adds presets for each size and output format.
	/// </summary>
	private void GeneratePresets()
	{
		foreach (var (name, preset) in OutputSizes.Presets)
		{
			var outputSettings = new List<OutputSetting>();

			foreach (var outputFormat in Enum.GetValues<OutputFormats>())
			{
				var context = new PresetContext
				{
					Name = name,
					Dimension = preset,
					OutputFormat = outputFormat
				};

				AddFormatPresets(outputSettings, context);
			}

			this[name] = outputSettings;
		}
	}

	private void AddFormatPresets(List<OutputSetting> outputSettings, PresetContext context)
	{
		bool[] array = [true, false];

		var combinations = from withSubtitles in array
						   from withAudio in array
						   select new { withAudio, withSubtitles };

		foreach (var combination in combinations)
		{
			context.WithAudio = combination.withAudio;
			context.WithSubtitles = combination.withSubtitles;

			AddOutputSetting(outputSettings, context);
		}
	}

	private void AddOutputSetting(List<OutputSetting> outputSettings, PresetContext context)
	{
		outputSettings.Add(new OutputSetting
		{
			MaxWidth = context.Dimension.Width,
			MaxHeight = context.Dimension.Height,
			MaxBitrate = 1000,
			WithAudio = context.WithAudio,
			WithSubtitles = context.WithSubtitles,
			OutputFormat = context.OutputFormat
		});
	}
}
