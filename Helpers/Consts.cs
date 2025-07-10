namespace FishUtils.Helpers;

public static class Consts
{
	public const float Pi = float.Pi;
	public const float PiOver2 = float.Pi / 2f;
	public const float PiOver4 = float.Pi / 4f;
	public const float TwoPi = float.Pi * 2f;

	// See https://terraria.wiki.gg/wiki/Stopwatch#Notes
	public const float UnitsPerFrameToTilesPerSecond = 11f / 15f; // TODO: this is wrong i think
	public const float UnitsPerFrameToMilesPerHour = 216000f / 42240f;
}
