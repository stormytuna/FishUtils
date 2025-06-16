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
	/// <returns>The draw data for the projectile.</returns>
	public static DrawData GetCommonDrawData(this Projectile projectile, Color lightColor) {
		var texture = TextureAssets.Projectile[projectile.type].Value;
		var sourceRect = texture.Frame(1, Main.projFrames[projectile.type], 0, projectile.frame);
		return new DrawData {
			texture = texture,
			position = (projectile.Center - Main.screenPosition + new Vector2(0f, projectile.gfxOffY)).Floor(),
			sourceRect = texture.Frame(1, Main.projFrames[projectile.type], 0, projectile.frame),
			origin = sourceRect.Size() / 2f,
			color = projectile.GetAlpha(lightColor),
			rotation = projectile.rotation,
			scale = new Vector2(projectile.scale),
		};
	}
}
