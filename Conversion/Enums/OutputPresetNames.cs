namespace Conversion.Enums;

/// <summary>
/// Outputs that are used as a base for the other presets.
/// </summary>
public enum RawOutputPresetNames
{
	Thumb,
	LowSd,
	MedSd,
	HighSd,
	Hd720,
	Hd1080,
	Hd2k,
	Hd4k,
}

/// <summary>
/// Outputs which are presented to the user.
/// </summary>
public enum OutputPresetNames
{
	Thumb = RawOutputPresetNames.Thumb,
	LowSd = RawOutputPresetNames.LowSd,
	MedSd = RawOutputPresetNames.MedSd,
	HighSd = RawOutputPresetNames.HighSd,
	Hd720 = RawOutputPresetNames.Hd720,
}
