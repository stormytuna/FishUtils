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
	/// Checks if the given item is a mana pickup.
	/// </summary>
	/// <param name="item">The item to check.</param>
	/// <returns>true if the item is a mana pickup, false otherwise.</returns>
	public static bool IsManaPickup(this Item item) {
		return item.type is ItemID.Star or ItemID.SoulCake or ItemID.SugarPlum;	
	}	
}
