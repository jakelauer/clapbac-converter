using Conversion.Enums;
using Conversion.Models.OutputSettings;

namespace Conversion.Models;

public interface IConversionOutput
{
	public string OutputFilePath { get; set; }
	public IEnumerable<IOutputSetting> OutputSettings { get; set; }
}
