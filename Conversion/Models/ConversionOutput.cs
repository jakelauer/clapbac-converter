using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Conversion.Models.OutputSettings;

namespace Conversion.Models;

public class ConversionOutput(string outputFilePath, IEnumerable<IOutputSetting> outputSettings) : IConversionOutput
{
	public string OutputFilePath { get; set; } = outputFilePath;
	public IEnumerable<IOutputSetting> OutputSettings { get; set; } = outputSettings;
}
