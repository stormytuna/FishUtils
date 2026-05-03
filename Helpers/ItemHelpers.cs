using Terraria;
using Terraria.ID;

namespace FishUtils.Helpers;

public static class ItemHelpers
{
	/// <summary>
	/// Checks if the given item is a life pickup.
	/// </summary>
	/// <param name="item">The item to check.</param>
	/// <returns>true if the item is a life pickup, false otherwise.</returns>
	public static bool IsLifePickup(this Item item) {
		return item.type is ItemID.Heart or ItemID.CandyApple or ItemID.CandyCane;
	}
	
	/// <summary>
	/// Determines if the given item is a weapon (i.e. has more than 0 damage but is not a tool).
	/// </summary>
	/// <param name="item">The item to check.</param>
	/// <returns>true if the item is a weapon; otherwise, false.</returns>
	public static bool IsWeapon(this Item item) {
		bool isTool = item.pick > 0 || item.axe > 0 || item.hammer > 0;
		return item.damage > 0 && !isTool;
	}
	
	/// <summary>
	/// Checks if the given item is a mana pickup.
	/// </summary>
	/// <returns>true if the item is a mana pickup, false otherwise.</returns>
	public static bool IsManaPickup(this Item item) {
		return item.type is ItemID.Star or ItemID.SoulCake or ItemID.SugarPlum;	
	}

	/// <summary>
	/// Sets the given item to have default values for a yoyo item.
	/// </summary>
	/// <param name="item">The item to set the defaults for.</param>
	/// <param name="damage">The damage of the yoyo.</param>
	/// <param name="knockback">The knockback of the yoyo.</param>
	/// <param name="crit">The critical strike chance of the yoyo.</param>
	/// <param name="projectileType">The yoyo projectile.</param>
	/// <param name="shootSpeed">The item shoot speed.</param>
	public static void DefaultToYoyo(this Item item, int damage, float knockback, int crit, int projectileType, float shootSpeed) {
		item.width = 24;
		item.height = 24;

		item.useStyle = ItemUseStyleID.Shoot;
		item.useTime = item.useAnimation = 25;
		item.channel = true;
		item.noMelee = true;
		item.noUseGraphic = true;
		item.UseSound = SoundID.Item1;

		item.damage = damage;
		item.DamageType = DamageClass.MeleeNoSpeed;
		item.knockBack = knockback;
		item.crit = crit;
		item.shoot = projectileType;
		item.shootSpeed = shootSpeed;
	}
}
