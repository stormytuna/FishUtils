using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;

namespace FishUtils.Helpers;

public static class RenderHelpers
{
	/// <summary>
	/// Gets the common draw data for a <see cref="Projectile"/>.
	/// </summary>
	/// <param name="projectile">The projectile to get the draw data for.</param>
	/// <param name="lightColor">The light color to use when drawing.</param>
	/// <param name="horizontalFrames">The number of horizontal frames in the texture. Defaults to 1.</param>
	/// <returns>The draw data for the projectile.</returns>
	public static DrawData GetCommonDrawData(this Projectile projectile, Color lightColor, int horizontalFramesTotal = 1, int horizontalFrame = 0) {
		Texture2D texture = TextureAssets.Projectile[projectile.type].Value;
		
		Rectangle sourceRect = texture.Frame(horizontalFramesTotal, Main.projFrames[projectile.type], horizontalFrame, projectile.frame);
		
		return new DrawData {
			texture = texture,
			position = projectile.Center - Main.screenPosition + new Vector2(0f, projectile.gfxOffY),
			sourceRect = sourceRect,
			origin = sourceRect.Size() / 2f,
			color = projectile.GetAlpha(lightColor),
			rotation = projectile.rotation,
			scale = new Vector2(projectile.scale),
		};
	}

	/// <summary>
	/// Checks whether a vector is on screen.
	/// </summary>
	/// <param name="position">The vector.</param>
	/// <param name="padding">Extra padding around the screen's position to consider as on-screen.</param>
	/// <returns>Whether the vector is on screen.</returns>
	public static bool OnScreen(this Vector2 position, int padding = 480) {
		var screenBounds = new Rectangle((int)Main.screenPosition.X,  (int)Main.screenPosition.Y, Main.screenWidth, Main.screenHeight);
		screenBounds.Inflate(padding, padding);
		return screenBounds.Intersects(new Rectangle((int)position.X, (int)position.Y, 1, 1));
	}
}
