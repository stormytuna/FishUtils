using System;
using System.Linq;
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
				spriteBatch.Draw(_asset.Value, drawPos, rect, Color, 0f, rect.Size() / 2f, scale, SpriteEffects.None, 0f);
			}

			size = rect.Size() * scale;
			return true;
		}

		public override float GetStringLength(DynamicSpriteFont font) {
			return _asset.Width() * Scale;
		}
	}

	TextSnippet ITagHandler.Parse(string text, Color baseColor, string options) {
		Color color = Color.White;
		
		// TODO: Make this not suck
		if (options is not null) {
			var optionStrings = options.Split(';');
			foreach (var optionString in optionStrings) {
				if (optionString == "") {
					continue;
				}

				switch (optionString[0]) {
					case 'c':
						var colorInts = optionString[1..].Split(',');
						if (colorInts.Length is not 3 and not 4) {
							// TODO: Logging?
							continue;
						}
						
						var colors = colorInts.Select(x => int.Parse(x)).ToArray();
						int alpha = colors.Length == 3 ? 255 : colors[3];
						color = new Color(colors[0], colors[1], colors[2], alpha);
						break;
					default:
						// TODO: Logging?
						break;
				}
			}
		}
		
		return new TextureSnippet(text) {
			Text = CreateTag(text),
			Color = color,
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
