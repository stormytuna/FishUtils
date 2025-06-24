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
	
	/// Checks if the given item is a mana pickup.
	/// <returns>true if the item is a mana pickup, false otherwise.</returns>
	public static bool IsManaPickup(this Item item) {
		return item.type is ItemID.Star or ItemID.SoulCake or ItemID.SugarPlum;	
	}	
}
