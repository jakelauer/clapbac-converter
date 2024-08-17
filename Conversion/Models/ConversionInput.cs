using Conversion.Services;

namespace Conversion.Models;

public class ConversionInput(string inputPath, OutputSettingPresets presets) : IConversionInput
{
	public string InputFilePath { get; set; } = inputPath;
	public IEnumerable<IConversionOutput> Outputs { get; set; } = GenerateOutputs(inputPath, presets);

	private static IEnumerable<IConversionOutput> GenerateOutputs(string inputPath, OutputSettingPresets presets)
	{
		// Generate the output file paths
		return presets.Values.Select(preset => new ConversionOutput(inputPath, preset));
	}
}
