using Microsoft.Xna.Framework;
using Terraria;

namespace FishUtils.Helpers;

public static class ColorHelpers
{
	public static Color WithMouseTextPulsing(this Color color) {
		return color * (Main.mouseTextColor / 255f);
	}

	public static string ApplyColor(this string str, Color color) {
		return $"[c/{color.Hex3()}:{str}]";
	}
}
