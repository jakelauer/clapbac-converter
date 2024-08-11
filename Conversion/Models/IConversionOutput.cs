using Conversion.Enums;
using Conversion.Models.OutputSettings;

namespace Conversion.Models;

public interface IConversionOutput
{
	public string OutputFilePath { get; set; }
	public IOutputSetting OutputSettings { get; set; }
}
