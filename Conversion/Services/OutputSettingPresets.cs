using Conversion.Enums;
using Conversion.Models;
using Conversion.Models.OutputSettings;
using Microsoft.Extensions.Logging;

namespace Conversion.Services;

public class OutputSettingPresets : Dictionary<RawOutputPresetNames, IEnumerable<IOutputSetting>>
{
	public struct PresetContext
	{
		public RawOutputPresetNames Name;
		public Dimension Dimension;
		public OutputFormats OutputFormat;
		public bool WithAudio;
		public bool WithSubtitles;
	}

	private readonly ILogger<OutputSettingPresets> _logger;

	public OutputSettingPresets(ILogger<OutputSettingPresets> logger)
	{
		_logger = logger;

		GeneratePresets();
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

		_logger.LogInformation("[{Name}] Added preset: {Width}x{Height} {OutputFormat} {WithAudio} {WithSubtitles}",
			context.Name,
			context.Dimension.Width,
			context.Dimension.Height,
			context.OutputFormat,
			context.WithAudio ? "with audio" : "without audio",
			context.WithSubtitles ? "with subtitles" : "without subtitles");
	}
}
