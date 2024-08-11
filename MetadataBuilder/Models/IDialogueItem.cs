namespace MetadataBuilder.Models;

public interface IDialogueItem
{
	public string RawText { get; set; }
	public TimeSpan Duration { get; }
	public TimeSpan StartTime { get; set; }
	public TimeSpan EndTime { get; set; }
	public string? SpokenText { get; set; }
	public string? DescriptiveText { get; set; }
	public string? CharacterName { get; set; }
	public string? UnclassifiedText { get; set; }
	public (int, int) PositionXY { get; set; }
}
