using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace FishUtils.Helpers;

public static class NPCHelpers
{
	/// <summary>
	/// Finds all the NPCs within a certain range of a position.
	/// </summary>
	/// <param name="range">The range to search within.</param>
	/// <param name="worldPos">The position to search around.</param>
	/// <param name="careAboutCollision">Whether or not to care about collision between the NPC and the worldPos.</param>
	/// <param name="ignoredNPCs">A list of NPC IDs to ignore. If null, no NPCs are ignored.</param>
	/// <returns>A list of all the NPCs found.</returns>
	public static List<NPC> FindNearbyNPCs(float range, Vector2 worldPos, bool careAboutCollision = false, List<int> ignoredNPCs = null) {
		List<NPC> npcs = new(Main.maxNPCs);
		ignoredNPCs ??= [];

		foreach (NPC npc in Main.ActiveNPCs) {
			if (npc.WithinRange(worldPos, range) && !ignoredNPCs.Contains(npc.whoAmI) && npc.CanBeChasedBy()) {
				if (!careAboutCollision || CollisionHelpers.CanHit(npc, worldPos)) {
					npcs.Add(npc);
				}
			}
		}

		return npcs;
	}

	/// <summary>
	/// Finds a random NPC within a certain range of a position.
	/// </summary>
	/// <param name="range">The range to search within.</param>
	/// <param name="worldPos">The position to search around.</param>
	/// <param name="careAboutCollision">Whether or not to care about collision between the NPC and the worldPos.</param>
	/// <param name="ignoredNPCs">A list of NPC IDs to ignore. If null, no NPCs are ignored.</param>
	/// <returns>A random NPC found, or null if none exist.</returns>
	public static NPC FindRandomNearbyNPC(float range, Vector2 worldPos, bool careAboutCollision = false, List<int> ignoredNPCs = null) {
		ignoredNPCs ??= [];

		List<NPC> npcs = FindNearbyNPCs(range, worldPos, careAboutCollision, ignoredNPCs);
		if (npcs.Count > 0) {
			return Main.rand.Next(npcs);
		}

		return null;
	}

	/// <summary>
	/// Finds the closest NPC within a certain range of a position.
	/// </summary>
	/// <param name="range">The range to search within.</param>
	/// <param name="worldPos">The position to search around.</param>
	/// <param name="checkCollision">Whether or not to care about collision between the NPC and the worldPos.</param>
	/// <param name="ignoredNPCs">A list of NPC IDs to ignore. If null, no NPCs are ignored.</param>
	/// <returns>The closest NPC found, or null if none exist.</returns>
	public static NPC FindClosestNPC(float range, Vector2 worldPos, bool checkCollision = true, List<int> ignoredNPCs = null) {
		ignoredNPCs ??= [];
		NPC closestNpc = null;
		float closestNpcDistance = float.PositiveInfinity;

		foreach (NPC npc in Main.ActiveNPCs) {
			float distance = Vector2.Distance(npc.Center, worldPos);
			if (!npc.CanBeChasedBy() || distance > range || distance > closestNpcDistance || ignoredNPCs.Contains(npc.whoAmI)) {
				continue;
			}

			if (checkCollision && !CollisionHelpers.CanHit(npc, worldPos)) {
				continue;
			}

			closestNpc = npc;
			closestNpcDistance = distance;
		}

		return closestNpc;
	}

	/// <summary>
	/// Finds the furthest NPC within a certain range of a position.
	/// </summary>
	/// <param name="range">The range to search within.</param>
	/// <param name="worldPos">The position to search around.</param>
	/// <param name="checkCollision">Whether or not to care about collision between the NPC and the worldPos.</param>
	/// <param name="ignoredNPCs">A list of NPC IDs to ignore. If null, no NPCs are ignored.</param>
	/// <returns>The furthest NPC found, or null if none exist.</returns>
	public static NPC FindFurthestNPC(float range, Vector2 worldPos, bool checkCollision = true, List<int> ignoredNPCs = null) {
		ignoredNPCs ??= [];
		NPC furthestNpc = null;
		float furthestNpcDistance = float.NegativeInfinity;

		foreach (NPC npc in Main.ActiveNPCs) {
			float distance = Vector2.Distance(npc.Center, worldPos);
			if (!npc.CanBeChasedBy() || distance > range || distance < furthestNpcDistance || ignoredNPCs.Contains(npc.whoAmI)) {
				continue;
			}

			if (checkCollision && !CollisionHelpers.CanHit(npc, worldPos)) {
				continue;
			}

			furthestNpc = npc;
			furthestNpcDistance = distance;
		}
		
		return furthestNpc;
	}

	/// <summary>
	/// Checks if there is an active boss NPC or an ongoing invasion.
	/// </summary>
	/// <returns>true if there is an active boss NPC or an ongoing invasion, false otherwise.</returns>
	public static bool AnyActiveBossOrInvasion() {
		if (Main.CurrentFrameFlags.AnyActiveBossNPC) {
			return true;
		}

		if (Main.invasionType != InvasionID.None) {
			return true;
		}

		return false;
	}

	/// <summary>
	/// Determines whether the specified NPC is considered a boss.
	/// </summary>
	/// <param name="npc">The NPC to check.</param>
	/// <returns>true if the NPC is a boss; otherwise, false.</returns>
	public static bool CountsAsBoss(this NPC npc) {
		return npc.boss || NPCID.Sets.DangerThatPreventsOtherDangers[npc.type] || npc.type is NPCID.EaterofWorldsHead or NPCID.EaterofWorldsBody or NPCID.EaterofWorldsTail;
	}

	/// <summary>
	/// Determines whether the specified NPC is a worm.
	/// </summary>
	/// <param name="npc">The NPC to check.</param>
	/// <returns>true if the NPC is a worm; otherwise, false.</returns>
	public static bool IsWorm(this NPC npc) {
		return npc.type switch {
			NPCID.EaterofWorldsHead => true,
			NPCID.EaterofWorldsBody => true,
			NPCID.EaterofWorldsTail => true,
			NPCID.LeechHead => true,
			NPCID.LeechBody => true,
			NPCID.LeechTail => true,
			NPCID.DiggerHead => true,
			NPCID.DiggerBody => true,
			NPCID.DiggerTail => true,
			NPCID.DevourerHead => true,
			NPCID.DevourerBody => true,
			NPCID.DevourerTail => true,
			NPCID.DuneSplicerHead => true,
			NPCID.DuneSplicerBody => true,
			NPCID.DuneSplicerTail => true,
			NPCID.GiantWormHead => true,
			NPCID.GiantWormBody => true,
			NPCID.GiantWormTail => true,
			NPCID.TombCrawlerHead => true,
			NPCID.TombCrawlerBody => true,
			NPCID.TombCrawlerTail => true,
			NPCID.SeekerHead => true,
			NPCID.SeekerBody => true,
			NPCID.SeekerTail => true,
			NPCID.SolarCrawltipedeHead => true,
			NPCID.SolarCrawltipedeBody => true,
			NPCID.SolarCrawltipedeTail => true,
			NPCID.WyvernHead => true,
			NPCID.WyvernBody => true,
			NPCID.WyvernBody2 => true,
			NPCID.WyvernBody3 => true,
			NPCID.WyvernTail => true,
			NPCID.BoneSerpentHead => true,
			NPCID.BoneSerpentBody => true,
			NPCID.BoneSerpentTail => true,
			NPCID.BloodEelHead => true,
			NPCID.BloodEelBody => true,
			NPCID.BloodEelTail => true,
			NPCID.CultistDragonHead => true,
			NPCID.CultistDragonBody1 => true,
			NPCID.CultistDragonBody2 => true,
			NPCID.CultistDragonBody3 => true,
			NPCID.CultistDragonBody4 => true,
			NPCID.CultistDragonTail => true,
			_ => false,
		};
	}

	/// <summary>
	/// Scales the given damage value up or down depending on the current difficulty.
	/// </summary>
	/// <param name="damage">The amount of damage to scale.</param>
	/// <returns>The scaled damage value.</returns>
	/// <remarks>
	/// In Master Mode, the damage is tripled. In Expert Mode, the damage is doubled.
	/// Otherwise, the original damage value is returned.
	/// </remarks>
	public static int ScaleDamageForDifficulty(int damage) {
		if (Main.masterMode) {
			return damage * 3;
		}

		if (Main.expertMode) {
			return damage * 2;
		}

		return damage;
	}
}
