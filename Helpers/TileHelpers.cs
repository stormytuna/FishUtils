using Microsoft.Xna.Framework;
using Terraria;

namespace FishUtils.Helpers;

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

	/// <summary>
	/// Clamps two tile coordinates to within the world bounds.
	/// </summary>
	public static void ClampWithinWorld(ref int minX, ref int maxX, ref int minY, ref int maxY) {
		if (minX < 0) {
			minX = 0;
		}

		if (minX > Main.maxTilesX) {
			minX = Main.maxTilesX;
		}

		if (maxX < 0) {
			maxX = 0;
		}

		if (maxX > Main.maxTilesX) {
			maxX = Main.maxTilesX;
		}

		if (minY < 0) {
			minY = 0;
		}

		if (minY > Main.maxTilesY) {
			minY = Main.maxTilesY;
		}

		if (maxY < 0) {
			maxY = 0;
		}

		if (maxY > Main.maxTilesY) {
			maxY = Main.maxTilesY;
		}
	}
}
