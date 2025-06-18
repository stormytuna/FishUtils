using Terraria;

namespace FishUtils.Helpers;

public static class ProjectileHelpers
{
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
}
