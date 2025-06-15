using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Utilities;

namespace FishUtils.Helpers;

public static class RandomHelpers
{
	// Adapted from https://bitbucket.org/Superbest/superbest-random/src/f067e1dc014c31be62c5280ee16544381e04e303/Superbest%20random/RandomExtensions.cs#lines-19
	/// <summary>
	/// Returns a normally distributed random float using the Box-Muller transform.
	/// </summary>
	/// <param name="rand">The UnifiedRandom to use.</param>
	/// <param name="mu">The mean of the distribution. Defaults to 0.</param>
	/// <param name="sigma">The standard deviation of the distribution. Defaults to 1.</param>
	/// <returns>A normally distributed random float.</returns>
	public static float NextGaussian(this UnifiedRandom rand, float mu = 0, float sigma = 1) {
		float u1 = rand.NextFloat();
		float u2 = rand.NextFloat();

		float stdNormal = float.Sqrt(-2f * float.Log(u1)) * float.Sin(2f * MathHelper.Pi * u2);

		float normal = mu + (sigma * stdNormal);

		return normal;
	}
	
	/// <summary>
	/// Generates a list of radian angles that are evenly spaced between 0 and 2Pi, with optional overlap and random offset.
	/// </summary>
	/// <param name="rand">The UnifiedRandom to use</param>
	/// <param name="numSegments">The number of evenly spaced angles to generate</param>
	/// <param name="overlap">The amount of radians to allow the angles to overlap by</param>
	/// <param name="randomOffset">Whether to add a random offset to the angles</param>
	/// <returns>A list of radian angles that are evenly spaced between 0 and 2Pi, with optional overlap and random offset</returns>
	public static List<float> NextSegmentedAngles(this UnifiedRandom rand, int numSegments, float overlap = 0f, bool randomOffset = true) {
		List<float> angles = new();

		// Build our list
		float offset = randomOffset ? rand.NextFloat(MathHelper.TwoPi) : 0f;
		for (int i = 0; i < numSegments; i++) {
			float angle = (i / (float)numSegments * MathHelper.TwoPi) + offset;
			angles.Add(angle);
		}

		// Randomly rotate our angles
		for (int i = 0; i < angles.Count; i++) {
			float rotationMax = (MathHelper.TwoPi / numSegments) + overlap;
			float rotation = rand.NextFloat(-rotationMax / 2f, rotationMax / 2f);
			angles[i] += rotation;
		}

		return angles;
	}
	
	/// <summary>
	/// Generates a random radian value between 0 and 2Pi
	/// </summary>
	/// <param name="rand">The UnifiedRandom to use</param>
	/// <returns>A random radian value between 0 and 2Pi</returns>
	public static float NextRadian(this UnifiedRandom rand) {
		return rand.NextFloat(MathHelper.TwoPi);
	}
	
	/// <summary>
	/// Generates a random point within the given rectangle
	/// </summary>
	/// <param name="rand">The UnifiedRandom to use</param>
	/// <param name="rect">The rectangle to generate a point within</param>
	/// <returns>A random point within the rectangle</returns>
	public static Vector2 NextVectorWithin(this UnifiedRandom rand, Rectangle rect) {
		return new Vector2(rect.Left + rand.Next(rect.Width + 1), rect.Top + rand.Next(rect.Height + 1));
	}
	
	/// <summary>
	/// Generates a random point within the given rectangle, normalized so that the rectangle's top-left corner is at (0,0)
	/// </summary>
	/// <param name="rand">The UnifiedRandom to use</param>
	/// <param name="rect">The rectangle to generate a point within</param>
	/// <returns>A random point within the rectangle, normalized so that the rectangle's top-left corner is at (0,0)</returns>
	public static Vector2 NextVectorWithinNormalized(this UnifiedRandom rand, Rectangle rect) {
		return NextVectorWithin(rand, rect with { X = 0, Y = 0 });
	}

	/// <summary>
	/// Generates a random count by recursively sampling from a given chance until either the chance is missed or the maximum count is reached.
	/// </summary>
	/// <param name="rand">The UnifiedRandom to use</param>
	/// <param name="chance">The chance to sample from</param>
	/// <param name="maxCount">The maximum count to return. Defaults to 100.</param>
	/// <returns>A random count between 0 and maxCount, inclusive</returns>
	public static int NextRecursiveCount(this UnifiedRandom rand, float chance, int maxCount = 100) {
		for (int i = 0; i < maxCount; i++) {
			if (rand.NextFloat() > chance) {
				return i;
			}	
		}
		
		return maxCount;
	}

	/// <summary>
	/// Gets a random element from the WeightedRandom, removes it from the WeightedRandom, and returns it.
	/// </summary>
	/// <typeparam name="T">The type of the elements in the WeightedRandom</typeparam>
	/// <param name="weightedRandom">The WeightedRandom to get from and remove from</param>
	/// <returns>The random element</returns>
	public static T GetAndRemove<T>(this WeightedRandom<T> weightedRandom) {
		var result = weightedRandom.Get();
		weightedRandom.elements.RemoveAll(x => x.Item1.Equals(result));
		return result;
	}
}
