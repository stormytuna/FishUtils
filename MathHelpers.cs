using System.Collections.Generic;

namespace FishUtils;

public static class MathHelpers
{
	/// <summary>
	/// Creates a list of points that form a lightning bolt shape between source and dest.
	/// </summary>
	/// <param name="source">The starting point of the lightning bolt.</param>
	/// <param name="dest">The ending point of the lightning bolt.</param>
	/// <param name="sway">The amount of randomness in the lightning bolt's path.</param>
	/// <param name="jaggednessNumerator">A number that controls how jagged the lightning bolt is. Higher values mean more jagged. [0..2] is advised</param>
	/// <returns>A list of points that form the lightning bolt.</returns>
	public static List<Vector2> CreateLightningBolt(Vector2 source, Vector2 dest, float sway = 80f,
		float jaggednessNumerator = 1f) {
		List<Vector2> results = new();
		Vector2 tangent = dest - source;
		Vector2 normal = Vector2.Normalize(new Vector2(tangent.Y, -tangent.X));
		float length = tangent.Length();

		List<float> positions = new() { 0 };

		for (int i = 0; i < length / 16f; i++) {
			positions.Add(Main.rand.NextFloat());
		}

		positions.Sort();

		float jaggedness = jaggednessNumerator / sway;

		Vector2 prevPoint = source;
		float prevDisplacement = 0f;
		for (int i = 1; i < positions.Count; i++) {
			float pos = positions[i];

			// used to prevent sharp angles by ensuring very close positions also have small perpendicular variation.
			float scale = length * jaggedness * (pos - positions[i - 1]);

			// defines an envelope. Points near the middle of the bolt can be further from the central line.
			float envelope = pos > 0.95f ? 20 * (1 - pos) : 1;

			float displacement = Main.rand.NextFloat(-sway, sway);
			displacement -= (displacement - prevDisplacement) * (1 - scale);
			displacement *= envelope;

			Vector2 point = source + (pos * tangent) + (displacement * normal);
			results.Add(point);
			prevPoint = point;
			prevDisplacement = displacement;
		}

		results.Add(prevPoint);
		results.Add(dest);
		results.Insert(0, source);

		return results;
	}
	
	// Adapted from The Story of Red Cloud https://github.com/timhjersted/tsorcRevamp/blob/aa4dc019218f94757f616dd88b9e2e57af539011/tsorcRevampUtils.cs#L957
	/// <summary>
	/// Smooth homing on a target, optionally taking into account the target's velocity.
	/// </summary>
	/// <param name="actor">The entity doing the homing.</param>
	/// <param name="target">The target to home in on.</param>
	/// <param name="acceleration">The maximum acceleration to apply to the actor.</param>
	/// <param name="topSpeed">The maximum speed the actor will reach.</param>
	/// <param name="targetVelocity">The velocity of the target to take into account. Optional.</param>
	public static void SmoothHoming(Entity actor, Vector2 target, float acceleration, float topSpeed, Vector2? targetVelocity = null) {
		Vector2 targetVel = targetVelocity ?? Vector2.Zero;
		
		Vector2 toTarget = actor.DirectionTo(target);
		float distanceToTarget = actor.Distance(target);
		Vector2 velocityTarget = targetVel - actor.velocity;

		float speed = Vector2.Dot(-velocityTarget, toTarget);

		float eta = (-speed / acceleration) + float.Sqrt((speed * speed / (acceleration * acceleration)) + (2 * distanceToTarget / acceleration));

		Vector2 impactPos = target + (velocityTarget * eta);

		Vector2 fixedAcceleration = actor.DirectionTo(impactPos) * acceleration;
		
		actor.velocity += fixedAcceleration;

		if (actor.velocity.Length() > topSpeed) {
			actor.velocity.Normalize();
			actor.velocity *= topSpeed;
		}
	}
}
