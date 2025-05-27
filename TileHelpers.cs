namespace FishUtils;

public static class TileHelpers
{
	/// <summary>
	/// Retrieves the brightness level at a specified tile coordinate.
	/// </summary>
	/// <param name="point">The tile coordinates to get the brightness from.</param>
	/// <returns>The brightness level at the specified tile coordinate.</returns>
	public static float GetBrightness(Point point) {
		return Lighting.Brightness(point.X, point.Y);
	}

	/// <summary>
	/// Retrieves the brightness level at a specified world position.
	/// </summary>
	/// <param name="vector">The world position to get the brightness from.</param>
	/// <returns>The brightness level at the specified world position.</returns>
	public static float GetBrightness(Vector2 vector) {
		return GetBrightness(vector.ToTileCoordinates());
	}
}
