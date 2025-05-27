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
}
