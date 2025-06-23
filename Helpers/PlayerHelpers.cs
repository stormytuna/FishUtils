using System.Collections.Generic;
using Terraria;

namespace FishUtils.Helpers;

public static class PlayerHelpers
{
	/// <summary>
	/// Attempts to get the used ammo item id for the given <paramref name="item"/>. If you want to consume ammo to spawn a projectile, use <see cref="Player.PickAmmo"/>
	/// </summary>
	/// <param name="player">The player to check.</param>
	/// <param name="item">The item to find the ammo for.</param>
	/// <param name="usedAmmoItemId">The id of the used ammo item.</param>
	/// <returns>true if the ammo was found, false otherwise.</returns>
	public static bool TryGetWeaponAmmo(this Player player, Item item, out int usedAmmoItemId) {
		return player.PickAmmo(item, out _, out _, out _, out _, out usedAmmoItemId, true);
	}

	/// <summary>
	/// Gets the number of unique minion types the player has currently spawned. This is determined by counting the number of unique projectile types that have <see cref="Projectile.minion"/> and <see cref="Projectile.minionSlots"/> set.
	/// </summary>
	/// <param name="player">The player to count the minions for.</param>
	/// <returns>The number of unique minion types.</returns>
	public static int GetNumUniqueMinions(this Player player) {
		int count = 0;
		List<int> minionTypes = new();
		
		foreach (var proj in Main.ActiveProjectiles) {
			if (proj.active && proj.owner == player.whoAmI && proj.minion && proj.minionSlots > 0f && !minionTypes.Contains(proj.type)) {
				minionTypes.Add(proj.type);
				count++;
			}
		}
		
		return count;
	}

	/// <summary>
	/// Applies a standard invincibility time to the player, taking into account <see cref="Player.longInvince"/>.
	/// </summary>
	/// <param name="player">The player to apply the invincibility time to.</param>
	public static void ApplyStandardImmuneTime(this Player player) {
		player.SetImmuneTimeForAllTypes(player.longInvince ? 120 : 80);
	}
}
