using System.IO;
using System.Linq;
using GlobExpressions;
using MainArgs;
using SubtitlesParser.Classes.Parsers;

namespace Main;

public class GlobSearch
{
	private SearchOptions _opts { get; set; }

	public GlobSearch(SearchOptions opts)
	{
		_opts = opts;
	}

	public GlobSearch()
	{
		_opts = new SearchOptions();
	}

	public virtual (IEnumerable<string>, IEnumerable<string>) SearchFiles()
	{
		// Get all files in the search path
		var allFiles = Directory.EnumerateFiles(_opts.SearchPath, "*", SearchOption.AllDirectories);

		// Use Glob to filter files based on the glob pattern
		var globMatcher = new Glob(_opts.SearchGlob);
		var matchedFiles = allFiles.Where(file => globMatcher.IsMatch(file));

		// Filter files by video and subtitle extensions
		var videoFiles = matchedFiles.Where(file => _opts.VideoExtensions.Contains(Path.GetExtension(file).ToLower()));
		var subtitleFiles = matchedFiles.Where(file => _opts.SubtitleExtensions.Contains(Path.GetExtension(file).ToLower()));

		// Combine video and subtitle files
		return (videoFiles, subtitleFiles);
	}
}
