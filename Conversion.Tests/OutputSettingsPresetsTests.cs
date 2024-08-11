using Conversion.Enums;
using Conversion.Models;
using Conversion.Models.OutputSettings;
using Conversion.Services;
using Microsoft.Extensions.Logging;
using Moq;

namespace Conversion.Tests;

[TestFixture]
public class OutputSettingPresetsTests
{
	private Mock<ILogger<OutputSettingPresets>> _mockLogger;
	private OutputSettingPresets _outputSettingPresets;

	[SetUp]
	public void SetUp()
	{
		_mockLogger = new Mock<ILogger<OutputSettingPresets>>();
		_outputSettingPresets = new OutputSettingPresets(_mockLogger.Object);
	}

	[Test]
	public void GeneratePresets_AddsPresetsForEachSizeAndFormat()
	{
		// Assert
		foreach (var size in OutputSizes.Presets)
		{
			Assert.That(_outputSettingPresets.ContainsKey(size.Key), Is.True);

			var presets = _outputSettingPresets[size.Key];
			Assert.Multiple(() =>
			{
				Assert.That(presets.Count(), Is.EqualTo(Enum.GetValues<OutputFormats>().Length * 4));
				Assert.That(presets.Any(os => os.WithAudio && os.WithSubtitles), Is.True);
				Assert.That(presets.Any(os => os.WithAudio && !os.WithSubtitles), Is.True);
				Assert.That(presets.Any(os => !os.WithAudio && os.WithSubtitles), Is.True);
				Assert.That(presets.Any(os => !os.WithAudio && !os.WithSubtitles), Is.True);
			});

		}
	}


}
