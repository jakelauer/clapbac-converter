using Conversion.Models.OutputSettings;
using FFmpeg.NET;
using Conversion.Enums;
using Conversion.Models;
using Conversion.Extensions;

namespace Conversion.Tests;

[TestFixture]
public class OutputSettingTests
{
	[Test]
	public void ToFFmpegOptions_WithMaxBitrate_SetsVideoBitRate()
	{
		var outputSetting = new OutputSetting { MaxBitrate = 1000 };
		var options = outputSetting.ToFFmpegOptions();

		Assert.That(options.VideoBitRate, Is.EqualTo(1000));
	}

	[Test]
	public void ToFFmpegOptions_WithOutputPresetName_SetsVideoSize()
	{
		var outputSetting = new OutputSetting { OutputPresetName = RawOutputPresetNames.Hd2k };
		var options = outputSetting.ToFFmpegOptions();

		Assert.That(options.VideoSize, Is.EqualTo(OutputSizes.Get(RawOutputPresetNames.Hd2k).FFmpegClosestMatch));
	}

	[Test]
	public void ToFFmpegOptions_WithExtendedOptions_ExtendsOptions()
	{
		var extendedOptions = new ConversionOptions { VideoBitRate = 2000 };
		var outputSetting = new OutputSetting { MaxBitrate = 1000 };
		var options = outputSetting.ToFFmpegOptions(extendedOptions);

		Assert.That(options.VideoBitRate, Is.EqualTo(2000));
	}

	[Test]
	public void ToFFmpegOptions_WithoutExtendedOptions_DoesNotExtendOptions()
	{
		var outputSetting = new OutputSetting { MaxBitrate = 1000 };
		var options = outputSetting.ToFFmpegOptions();

		Assert.That(options.VideoBitRate, Is.EqualTo(1000));
	}

	[Test]
	public void ToFFmpegOptions_WithAudio_RemovesAudioFalse()
	{
		var outputSetting = new OutputSetting { WithAudio = true };
		var options = outputSetting.ToFFmpegOptions();

		Assert.IsFalse(options.RemoveAudio);
	}

	[Test]
	public void ToFFmpegOptions_WithoutAudio_RemovesAudioTrue()
	{
		var outputSetting = new OutputSetting { WithAudio = false };
		var options = outputSetting.ToFFmpegOptions();

		Assert.That(options.RemoveAudio, Is.True);
	}
}
