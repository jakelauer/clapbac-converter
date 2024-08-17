namespace Conversion.Models;

public interface IConversionInput
{
	public string InputFilePath { get; set; }
	public IEnumerable<IConversionOutput> Outputs { get; set; }
}
