using Microsoft.Xna.Framework;
using Terraria;

namespace FishUtils.Helpers;

public static class CollisionHelpers
{

	public static bool CanHit(Entity source, Vector2 targetPos, int targetWidth = 1, int targetHeight = 1) {
		return Collision.CanHit(source.position, source.width, source.height, targetPos, targetWidth, targetHeight);
	}

	public static bool CanHit(Vector2 source, Vector2 target) {
		return Collision.CanHit(source, 1, 1, target, 1, 1);
	}

	/// <summary>
	/// Bounces off the tiles, given the current and old velocities.
	/// </summary>
	/// <param name="currentVelocity">The current velocity of the entity.</param>
	/// <param name="oldVelocity">The old velocity of the entity before it touched the tile.</param>
	/// <param name="xMult">An optional float that multiplies the X component of the velocity when bouncing off the tile. Defaults to 1f.</param>
	/// <param name="yMult">An optional float that multiplies the Y component of the velocity when bouncing off the tile. Defaults to 1f.</param>
	/// <returns>The new velocity of the entity after bouncing off the tile.</returns>
	public static Vector2 BounceOffTiles(this Vector2 currentVelocity, Vector2 oldVelocity, float xMult = 1f, float yMult = 1f) {
		Vector2 result = currentVelocity;
		
		if (currentVelocity.X != oldVelocity.X) {
			result.X = -oldVelocity.X * xMult;	
		}

		if (currentVelocity.Y != oldVelocity.Y) {
			result.Y = -oldVelocity.Y * yMult;	
		}

		return result;
	}
}
