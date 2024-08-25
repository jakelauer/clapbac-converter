using FFmpeg.NET;

namespace Conversion.Services;

public interface IConversionEngine
{
	public Task<MediaFile> ConvertAsync(IInputArgument input, OutputFile output, CancellationToken cancellationToken);
	public Task<MediaFile> ConvertAsync(IInputArgument input, OutputFile output, ConversionOptions options, CancellationToken cancellationToken);
}

public class FfmpegConversionAdapter : IConversionEngine
{
	private readonly Engine _engine = new("/opt/homebrew/bin/ffmpeg");

	public virtual Task<MediaFile> ConvertAsync(IInputArgument input, OutputFile output, CancellationToken cancellationToken)
	{
		return _engine.ConvertAsync(input, output, cancellationToken);
	}

	public virtual Task<MediaFile> ConvertAsync(IInputArgument input, OutputFile output, ConversionOptions options, CancellationToken cancellationToken)
	{
		return _engine.ConvertAsync(input, output, options, cancellationToken);
	}
}
