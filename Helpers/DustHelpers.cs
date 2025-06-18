using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace FishUtils.Helpers;

public class DustHelpers
{
	/// <summary>
	/// Creates an explosion effect by spawning a specified number of dust particles 
	/// at a given position with various customizable parameters.
	/// </summary>
	/// <param name="position">The center position where the dust explosion will originate.</param>
	/// <param name="spawnRadius">The radius within which the dust particles will spawn.</param>
	/// <param name="dustType">The type of dust to spawn.</param>
	/// <param name="amount">The number of dust particles to create.</param>
	/// <param name="minSpeed">The minimum speed of the dust particles.</param>
	/// <param name="maxSpeed">The maximum speed of the dust particles.</param>
	/// <param name="minAlpha">The minimum transparency level for the dust particles.</param>
	/// <param name="maxAlpha">The maximum transparency level for the dust particles.</param>
	/// <param name="minScale">The minimum scale for the dust particles.</param>
	/// <param name="maxScale">The maximum scale for the dust particles.</param>
	/// <param name="noGravity">If true, dust particles will not be affected by gravity.</param>
	/// <param name="noLight">If true, dust particles will not emit light.</param>
	/// <param name="noLightEmittance">If true, dust particles will not emit light.</param>
	public static void MakeDustExplosion(Vector2 position, float spawnRadius, int dustType, int amount, float minSpeed, float maxSpeed, int minAlpha, int maxAlpha, float minScale, float maxScale, bool noGravity = false, bool noLight = false, bool noLightEmittance = false) {
		for (int i = 0; i < amount; i++) {
			Vector2 spawnPosition = position + Main.rand.NextVector2Circular(spawnRadius, spawnRadius);
			Dust newDust = Dust.NewDustPerfect(spawnPosition, dustType);
			newDust.velocity = Main.rand.NextVector2Unit() * Main.rand.NextFloat(minSpeed, maxSpeed);
			newDust.alpha = Main.rand.Next(minAlpha, maxAlpha);
			newDust.scale = Main.rand.NextFloat(minScale, maxScale);
			newDust.noGravity = noGravity;
			newDust.noLight = noLight;
			newDust.noLightEmittence = noLightEmittance;
		}
	}


	/// <inheritdoc cref="MakeDustExplosion(Microsoft.Xna.Framework.Vector2,float,int,int,float,float,int,int,float,float,bool,bool,bool)"/>
	public static void MakeDustExplosion(Vector2 position, float spawnRadius, int dustType, int amount, float minSpeed,
		float maxSpeed, int alpha = 0, float scale = 1f, bool noGravity = false, bool noLight = false,
		bool noLightEmmittance = false) {
		MakeDustExplosion(position, spawnRadius, dustType, amount, minSpeed, maxSpeed, alpha, alpha, scale, scale,
			noGravity, noLight, noLightEmmittance);
	}

	/// <inheritdoc cref="MakeDustExplosion(Microsoft.Xna.Framework.Vector2,float,int,int,float,float,int,int,float,float,bool,bool,bool)"/>
	public static void MakeDustExplosion(Vector2 position, float spawnRadius, int dustType, int amount, float minSpeed,
		float maxSpeed, int minAlpha, int maxAlpha, float scale = 1f, bool noGravity = false, bool noLight = false,
		bool noLightEmmittance = false) {
		MakeDustExplosion(position, spawnRadius, dustType, amount, minSpeed, maxSpeed, minAlpha, maxAlpha, scale, scale,
			noGravity, noLight, noLightEmmittance);
	}

	/// <summary>
	/// Creates an explosion effect by spawning a specified number of dust particles at a given position.
	/// </summary>
	/// <param name="position">The center of the explosion.</param>
	/// <param name="spawnRadius">The radius of the explosion.</param>
	/// <param name="dustType">The type of dust to create.</param>
	/// <param name="amount">The number of dust particles to create.</param>
	/// <returns>A list of the created dust particles.</returns>
	public static List<Dust> MakeDustExplosion(Vector2 position, float spawnRadius, int dustType, int amount) {
		List<Dust> dusts = new(amount);

		for (int i = 0; i < amount; i++) {
			Vector2 spawnPosition = position + Main.rand.NextVector2Circular(spawnRadius, spawnRadius);
			dusts.Add(Dust.NewDustPerfect(spawnPosition, dustType));
		}

		return dusts;
	}

	/// <inheritdoc cref="MakeDustExplosion(Vector2, float, int, int)"/>
	public static List<Dust> MakeDustExplosion<TModDust>(Vector2 position, float spawnRadius, int amount)
		where TModDust : ModDust {
		return MakeDustExplosion(position, spawnRadius, ModContent.DustType<TModDust>(), amount);
	}

	/// <summary>
	/// Creates a visual effect of a lightning bolt by spawning dust particles along a path
	/// between the source and destination points.
	/// </summary>
	/// <param name="source">The starting point of the lightning bolt effect.</param>
	/// <param name="dest">The ending point of the lightning bolt effect.</param>
	/// <param name="dustId">The dust type to use for the effect.</param>
	/// <param name="scale">The scale of the dust particles.</param>
	/// <param name="sway">The amount of randomness in the path of the lightning bolt.</param>
	/// <param name="jagednessNumerator">A value controlling the jaggedness of the path.
	/// Higher values result in a more jagged appearance. [0..2] is advised</param>
	public static List<Dust> MakeLightningDust(Vector2 source, Vector2 dest, int dustId, float scale, float sway = 80f,
		float jagednessNumerator = 1f) {
		List<Vector2> dustPoints = MathHelpers.CreateLightningBolt(source, dest, sway, jagednessNumerator);

		List<Dust> dustList = new(dustPoints.Count);

		for (int i = 1; i < dustPoints.Count; i++) {
			Vector2 start = dustPoints[i - 1];
			Vector2 end = dustPoints[i];
			float numDust = (end - start).Length() * 0.4f;

			for (int j = 0; j < numDust; j++) {
				float lerp = j / numDust;
				Vector2 dustPosition = Vector2.Lerp(start, end, lerp);

				Dust d = Dust.NewDustPerfect(dustPosition, dustId, Scale: scale);
				d.noGravity = true;
				d.velocity = Main.rand.NextVector2Circular(0.3f, 0.3f);
				dustList.Add(d);
			}
		}

		return dustList;
	}

	/// <summary>
	/// Returns a rectangle representing the frame of the specified vanilla dust type in the dust texture. Use this with <see cref="ModTexturedType.Texture" /> returning null
	/// </summary>
	/// <param name="vanillaDustType">The type of vanilla dust to get the frame for.</param>
	/// <returns>A rectangle representing the frame of the specified vanilla dust type.</returns>
	public static Rectangle FrameVanillaDust(int vanillaDustType) {
		int frameX = vanillaDustType * 10 % 1000;
		int frameY = (vanillaDustType * 10 / 1000 * 30) + (Main.rand.Next(3) * 10);
		return new Rectangle(frameX, frameY, 8, 8);
	}
}
