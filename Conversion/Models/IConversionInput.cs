namespace Conversion.Models;

public interface IConversionInput
{
	public string InputFilePath { get; set; }
	public IConversionOutput[] Outputs { get; set; }
}
