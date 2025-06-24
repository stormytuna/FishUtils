using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;

namespace FishUtils.Helpers;

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
	/// Smooth homing on a target, optionally taking into account the target's velocity or adding a buffer zone.
	/// </summary>
	/// <param name="actor">The entity doing the homing.</param>
	/// <param name="target">The target to home in on.</param>
	/// <param name="acceleration">The maximum acceleration to apply to the actor.</param>
	/// <param name="topSpeed">The maximum speed the actor will reach.</param>
	/// <param name="targetVelocity">The velocity of the target to take into account. Optional.</param>
	/// <param name="bufferDistance">The distance at which the actor will slow down. Optional.</param>
	/// <param name="bufferStrength">The strength of the slow-down. Optional.</param>
	public static void SmoothHoming(Entity actor, Vector2 target, float acceleration, float topSpeed, Vector2? targetVelocity = null, float bufferDistance = 0f, float bufferStrength = 0f) {
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

		if (bufferDistance > 0 && distanceToTarget < bufferDistance) {
			actor.velocity *= float.Pow(distanceToTarget / bufferDistance, bufferStrength);
		}
	}

	/// <summary>
	/// Smoothly rotates from <paramref name="currentAngle"/> to <paramref name="targetAngle"/>,
	/// but never moves more than <paramref name="maxStep"/> in a single step.
	/// </summary>
	/// <param name="currentAngle">The current angle.</param>
	/// <param name="targetAngle">The target angle.</param>
	/// <param name="maxStep">The maximum amount to step in a single step.</param>
	/// <returns>The new angle.</returns>
	public static float SmoothRotate(float currentAngle, float targetAngle, float maxStep) {
		currentAngle = MathHelper.WrapAngle(currentAngle);
		targetAngle = MathHelper.WrapAngle(targetAngle);
		float delta = MathHelper.WrapAngle(targetAngle - currentAngle);
		delta = float.Clamp(delta, -maxStep, maxStep);
		return MathHelper.WrapAngle(currentAngle + delta);
	}

	/// <summary>
	/// Normalizes a vector to length 1, or returns <see cref="Vector2.Zero"/> if the vector has NaNs.
	/// </summary>
	/// <param name="vector">The vector to normalize.</param>
	/// <returns>The normalized vector.</returns>
	public static Vector2 Normalized(this Vector2 vector) {
		return vector.SafeNormalize(Vector2.Zero);
	}

	/// <summary>
	/// Returns the midpoint of two vectors.
	/// </summary>
	/// <param name="vec1">The first vector.</param>
	/// <param name="vec2">The second vector.</param>
	/// <returns>The midpoint of the two vectors.</returns>
	public static Vector2 MidPoint(Vector2 vec1, Vector2 vec2) {
		return (vec1 + vec2) / 2f;
	}

	/// <summary>
	/// Clamps the given angle to be within the specified minimum and maximum range,
	/// taking into account angle wrapping at the boundaries.
	/// </summary>
	/// <param name="angle">The angle to clamp, in radians.</param>
	/// <param name="min">The minimum angle of the range, in radians.</param>
	/// <param name="max">The maximum angle of the range, in radians.</param>
	/// <returns>The clamped angle, within the specified range.</returns>
	public static float ClampAngle(float angle, float min, float max) {
		angle = MathHelper.WrapAngle(angle);
		min = MathHelper.WrapAngle(min);
		max = MathHelper.WrapAngle(max);

		if (min == max) {
			return min;
		}

		if (min < max) {
			return float.Clamp(angle, min, max);
		}

		if (angle > max && angle < min) {
			return (float.Abs(angle - min) < float.Abs(angle - max)) ? min : max;
		}
		
		return angle;
	}
}
