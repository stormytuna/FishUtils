using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;

namespace FishUtils.DataStructures;

public record SpriteBatchParams
{
	public SpriteSortMode SortMode;
	public BlendState BlendState;
	public SamplerState SamplerState;
	public DepthStencilState DepthStencilState;
	public RasterizerState RasterizerState;
	public Matrix TransformMatrix;
	public Effect Effect;

	public static SpriteBatchParams Default {
		get => new() {
			SortMode = SpriteSortMode.Deferred,
			BlendState = BlendState.AlphaBlend,
			SamplerState = SamplerState.PointClamp,
			DepthStencilState = DepthStencilState.None,
			RasterizerState = RasterizerState.CullNone,
			TransformMatrix = Main.GameViewMatrix.TransformationMatrix,
			Effect = null,
		};
	}
}

public static class SpriteBatchParamsExtensions
{
	public static void TakeSnapshotAndEnd(this SpriteBatch sb, out SpriteBatchParams sbParams) {
		sbParams = new SpriteBatchParams {
			SortMode = GetSortMode(sb),
			BlendState = GetBlendState(sb),
			SamplerState = GetSamplerState(sb),
			DepthStencilState = GetDepthStencilState(sb),
			RasterizerState = GetRasterizerState(sb),
			TransformMatrix = GetTransformMatrix(sb),
			Effect = GetEffect(sb)
		};
		sb.End();
	}

	public static void Restart(this SpriteBatch sb, SpriteBatchParams sbParams) {
		sb.End();
		sb.Begin(sbParams.SortMode, sbParams.BlendState, sbParams.SamplerState, sbParams.DepthStencilState, sbParams.RasterizerState, sbParams.Effect, sbParams.TransformMatrix);
	}

	public static void Begin(this SpriteBatch sb, SpriteBatchParams sbParams) {
		sb.Begin(sbParams.SortMode, sbParams.BlendState, sbParams.SamplerState, sbParams.DepthStencilState, sbParams.RasterizerState, sbParams.Effect, sbParams.TransformMatrix);
	}

	[UnsafeAccessor(UnsafeAccessorKind.Field, Name = "sortMode")]
	extern static ref SpriteSortMode GetSortMode(SpriteBatch sb);

	[UnsafeAccessor(UnsafeAccessorKind.Field, Name = "blendState")]
	extern static ref BlendState GetBlendState(SpriteBatch sb);

	[UnsafeAccessor(UnsafeAccessorKind.Field, Name = "samplerState")]
	extern static ref SamplerState GetSamplerState(SpriteBatch sb);

	[UnsafeAccessor(UnsafeAccessorKind.Field, Name = "depthStencilState")]
	extern static ref DepthStencilState GetDepthStencilState(SpriteBatch sb);

	[UnsafeAccessor(UnsafeAccessorKind.Field, Name = "rasterizerState")]
	extern static ref RasterizerState GetRasterizerState(SpriteBatch sb);

	[UnsafeAccessor(UnsafeAccessorKind.Field, Name = "transformMatrix")]
	extern static ref Matrix GetTransformMatrix(SpriteBatch sb);

	[UnsafeAccessor(UnsafeAccessorKind.Field, Name = "customEffect")]
	extern static ref Effect GetEffect(SpriteBatch sb);
}
