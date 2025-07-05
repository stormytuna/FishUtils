using Terraria;
using Microsoft.Xna.Framework;

namespace FishUtils.Helpers;

public static class ProjectileHelpers
{
/// <summary>
/// Determines the index of the projectile within a group of identical projectiles owned by the same player.
/// </summary>
/// <param name="projectile">The projectile to find the index for.</param>
/// <param name="index">The index of the projectile in the group.</param>
/// <param name="totalMembersInGroup">The total number of projectiles in the group.</param>
	public static void GetIndexInGroup(this Projectile projectile, out int index, out int totalMembersInGroup) {
		index = 0;
		totalMembersInGroup = 0;

		foreach (Projectile otherProj in Main.ActiveProjectiles) {
			if (otherProj.owner == projectile.owner && otherProj.type == projectile.type) {
				if (projectile.whoAmI > otherProj.whoAmI) {
					index++;
				}

				totalMembersInGroup++;
			}
		}
	}

	/// <summary>
	/// Creates an explosion using the passed projectile by resizing it, then striking nearby enemies, then returning it back to its original state.
	/// Performs client side check.
	/// </summary>
	/// <param name="width">The new width of the projectile.</param>
	/// <param name="height">The new height of the projectile.</param>
	/// <param name="damage">The new damage of the projectile. Defaults to the projectile's current damage.</param>
	/// <param name="knockback">The new knockback of the projectile. Defaults to the projectile's current knockback.</param>
	public static void Explode(this Projectile projectile, int width, int height, int? damage = null, float? knockback = null) {
		if (Main.myPlayer != projectile.owner) {
			return;
		}

		int oldPenetrate = projectile.penetrate;
		bool oldTileCollide = projectile.tileCollide;
		int oldDamage = projectile.damage;
		float oldKnockback = projectile.knockBack;
		Point oldSize = projectile.Size.ToPoint();

		projectile.penetrate = -1;
		projectile.tileCollide = false;
		projectile.damage = damage ?? projectile.damage;
		projectile.knockBack = knockback ?? projectile.knockBack;
		projectile.Resize(width, height);

		projectile.Damage();

		projectile.penetrate = oldPenetrate;
		projectile.tileCollide = oldTileCollide;
		projectile.damage = oldDamage;
		projectile.knockBack = oldKnockback;
		projectile.Resize(oldSize.X, oldSize.Y);
	}
}
