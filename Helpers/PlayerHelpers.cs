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
}
