using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using ReLogic.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI.Chat;

namespace FishUtils.UI;

public class TextureTagHandler : ITagHandler, ILoadable
{
	private class TextureSnippet(string assetPath) : TextSnippet
	{
		private readonly Asset<Texture2D> _asset = ModContent.Request<Texture2D>(assetPath);

		public override bool UniqueDraw(bool justCheckingString, out Vector2 size, SpriteBatch spriteBatch, Vector2 position = new(), Color color = new(), float scale = 1) {
			float textureSize = scale * 24f;
			Rectangle rect = new(0, 0, _asset.Width(), _asset.Height());
			if (rect.Width * scale > textureSize || rect.Height * scale > textureSize) {
				scale = rect.Width <= rect.Height ? textureSize / rect.Height : textureSize / rect.Width;
			}

			Vector2 drawPos = position + (rect.Size() * scale * 0.5f);
			if (!justCheckingString && color != Color.Black) {
				spriteBatch.Draw(_asset.Value, drawPos, rect, Color.White, 0f, rect.Size() / 2f, scale, SpriteEffects.None, 0f);
			}

			size = rect.Size() * scale;
			return true;
		}

		public override float GetStringLength(DynamicSpriteFont font) {
			return _asset.Width() * Scale;
		}
	}

	TextSnippet ITagHandler.Parse(string text, Color baseColor, string options) {
		return new TextureSnippet(text) {
			Text = CreateTag(text),
			CheckForHover = false,
			DeleteWhole = true,
		};
	}

	public static string CreateTag(string assetPath) {
		return $"[tunatex:{assetPath}]";
	}

	public void Load(Mod mod) {
		if (Environment.GetEnvironmentVariable("FISHUTILS_REGISTERED_TEXTURE_TAG_HANDLER") is not null) {
			return;
		};

		Environment.SetEnvironmentVariable("FISHUTILS_REGISTERED_TEXTURE_TAG_HANDLER", "true");
		ChatManager.Register<TextureTagHandler>("tunatex");
	}

	public void Unload() {
		Environment.SetEnvironmentVariable("FISHUTILS_REGISTERED_TEXTURE_TAG_HANDLER", null);
	}
}
