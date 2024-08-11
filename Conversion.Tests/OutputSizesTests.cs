namespace Conversion.Tests;

using NUnit.Framework;
using Conversion.Models;
using Conversion.Enums;
using FFmpeg.NET.Enums;
using System;

[TestFixture]
public class OutputSizesTests
{
	[Test]
	public void Get_ValidPreset_ReturnsCorrectDimensionTuple()
	{
		// Arrange
		var presetName = RawOutputPresetNames.Hd720;

		// Act
		var result = OutputSizes.Get(presetName);

		// Assert
		Assert.That((1280, 720, VideoSize.Hd720), Is.EqualTo(result));
	}

	[Test]
	public void Get_InvalidPreset_ThrowsInvalidOperationException()
	{
		// Arrange
		var invalidPresetName = (RawOutputPresetNames)999;

		// Act & Assert
		Assert.Throws<InvalidOperationException>(() => OutputSizes.Get(invalidPresetName));
	}

	[Test]
	public void ForEach_IteratesOverAllPresets()
	{
		// Arrange
		var presetsCount = OutputSizes.Presets.Count;
		var iterationCount = 0;

		// Act
		foreach (var (name, preset) in OutputSizes.Presets)
		{
			iterationCount++;
			Assert.Multiple(() =>
			{
				Assert.That(OutputSizes.Presets.ContainsKey(name), Is.True);
				Assert.That(OutputSizes.Presets[name], Is.EqualTo(preset));
			});
		}

		// Assert
		Assert.That(presetsCount, Is.EqualTo(iterationCount));
	}
}
