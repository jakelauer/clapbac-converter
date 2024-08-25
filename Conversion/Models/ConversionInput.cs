using Conversion.Models.OutputSettings;

namespace Conversion.Models;

public class ConversionInput(string inputPath) : IConversionInput
{
	public string InputFilePath { get; set; } = inputPath;
	public IEnumerable<IConversionOutput> Outputs { get; set; } = GenerateOutputs(inputPath);

	private static IEnumerable<IConversionOutput> GenerateOutputs(string inputPath)
	{
		// Generate the output file paths
		return OutputSetting.Presets.Values.Select(preset => new ConversionOutput(inputPath, preset));
	}
}
