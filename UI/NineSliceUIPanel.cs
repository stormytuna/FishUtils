using System;
using FishUtils.DataStructures;
using ReLogic.Content;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;

namespace FishUtils.UI;
/// <summary>
/// A UI panel that can be used to display a 9-sliced image for a panel with fixed size corners. <br/>
/// Panel size must always be equal to or greater than the texture size. <br/>
/// NOTE: Panel corners are static, edge and center slices will be scaled to fit the panel <br/>
/// NOTE: Panel must be divisible by 3, and have equal width and height
/// </summary>
public class NineSliceUIPanel : UIElement
{
	private Asset<Texture2D> _panelTexture;
	private int _sliceWidth;
	private Color _color;
	private Color? _hoverColor;
	private Color? _clickColor;
	private bool _clicking;
	
	public NineSliceUIPanel(Asset<Texture2D> panelTexture, Color color, Color? hoverColor = null, Color? clickColor = null) {
		_panelTexture = panelTexture;
		panelTexture.Wait();

		if (panelTexture.Width() != panelTexture.Height()) {
			throw new ArgumentException("Panel width must be equal to panel height", nameof(panelTexture));
		}

		if (panelTexture.Width() % 3 != 0) {
			throw new ArgumentException("Panel width must be divisible by 3", nameof(panelTexture));
		}
		
		_sliceWidth = panelTexture.Width() / 3;
		_color = color;
		_hoverColor = hoverColor;
		_clickColor = clickColor;

		OnLeftMouseDown += (_, _) => _clicking = true;
		OnLeftMouseUp += (_, _) => _clicking = false;
	}

	#region Source Rects
	
	public Rectangle TopLeftCornerSource {
		get => new Rectangle(0, 0, _sliceWidth, _sliceWidth);		
	}
	
	public Rectangle TopEdgeSource {
		get => new Rectangle(_sliceWidth, 0, _sliceWidth, _sliceWidth);
	}
	
	public Rectangle TopRightCornerSource {
		get => new Rectangle(_sliceWidth * 2, 0, _sliceWidth, _sliceWidth);
	}
	
	public Rectangle LeftEdgeSource {
		get => new Rectangle(0, _sliceWidth, _sliceWidth, _sliceWidth);
	}
	
	public Rectangle InsideSource {
		get => new Rectangle(_sliceWidth, _sliceWidth, _sliceWidth, _sliceWidth);
	}
	
	public Rectangle RightEdgeSource {
		get => new Rectangle(_sliceWidth * 2, _sliceWidth, _sliceWidth, _sliceWidth);
	}
	
	public Rectangle BottomLeftCornerSource {
		get => new Rectangle(0, _sliceWidth * 2, _sliceWidth, _sliceWidth);
	}
	
	public Rectangle BottomEdgeSource {
		get => new Rectangle(_sliceWidth, _sliceWidth * 2, _sliceWidth, _sliceWidth);
	}
	
	public Rectangle BottomRightCornerSource {
		get => new Rectangle(_sliceWidth * 2, _sliceWidth * 2, _sliceWidth, _sliceWidth);
	}
	
	#endregion

	#region Slice Rects
	
	public Rectangle GetTopLeft(Rectangle dims) {
		return new Rectangle(dims.X, dims.Y, _sliceWidth, _sliceWidth);		
	}

	public Rectangle GetTopEdge(Rectangle dims) {
		return new Rectangle(dims.X + _sliceWidth, dims.Y, dims.Width - _sliceWidth * 2, _sliceWidth);
	}
	
	public Rectangle GetTopRight(Rectangle dims) {
		return new Rectangle(dims.X + dims.Width - _sliceWidth, dims.Y, _sliceWidth, _sliceWidth);
	}
	
	public Rectangle GetLeftEdge(Rectangle dims) {
		return new Rectangle(dims.X, dims.Y + _sliceWidth, _sliceWidth, dims.Height - _sliceWidth * 2);
	}
	
	public Rectangle GetInside(Rectangle dims) {
		return new Rectangle(dims.X + _sliceWidth, dims.Y + _sliceWidth, dims.Width - _sliceWidth * 2, dims.Height - _sliceWidth * 2);
	}
	
	public Rectangle GetRightEdge(Rectangle dims) {
		return new Rectangle(dims.X + dims.Width - _sliceWidth, dims.Y + _sliceWidth, _sliceWidth, dims.Height - _sliceWidth * 2);
	}
	
	public Rectangle GetBottomLeft(Rectangle dims) {
		return new Rectangle(dims.X, dims.Y + dims.Height - _sliceWidth, _sliceWidth, _sliceWidth);
	}
	
	public Rectangle GetBottomEdge(Rectangle dims) {
		return new Rectangle(dims.X + _sliceWidth, dims.Y + dims.Height - _sliceWidth, dims.Width - _sliceWidth * 2, _sliceWidth);
	}
	
	public Rectangle GetBottomRight(Rectangle dims) {
		return new Rectangle(dims.X + dims.Width - _sliceWidth, dims.Y + dims.Height - _sliceWidth, _sliceWidth, _sliceWidth);
	}
	
	#endregion
	
	protected override void DrawSelf(SpriteBatch spriteBatch) {
		base.DrawSelf(spriteBatch);
		
		spriteBatch.TakeSnapshotAndEnd(out var sbParams);
		
		spriteBatch.Begin(sbParams with {
			BlendState = BlendState.AlphaBlend,
			SamplerState = SamplerState.PointClamp,
		});
		
		var dims = GetDimensions().ToRectangle();
		var texture = _panelTexture.Value;
		var drawColor = _clicking 
			? (_clickColor ?? _color) 
			: IsMouseHovering ? (_hoverColor ?? _color) : _color;
		
		spriteBatch.Draw(texture, GetInside(dims), InsideSource, drawColor);
		spriteBatch.Draw(texture, GetTopLeft(dims), TopLeftCornerSource, drawColor);
		spriteBatch.Draw(texture, GetTopEdge(dims), TopEdgeSource, drawColor);
		spriteBatch.Draw(texture, GetTopRight(dims), TopRightCornerSource, drawColor);
		spriteBatch.Draw(texture, GetLeftEdge(dims), LeftEdgeSource, drawColor);
		spriteBatch.Draw(texture, GetRightEdge(dims), RightEdgeSource, drawColor);
		spriteBatch.Draw(texture, GetBottomLeft(dims), BottomLeftCornerSource, drawColor);
		spriteBatch.Draw(texture, GetBottomEdge(dims), BottomEdgeSource, drawColor);
		spriteBatch.Draw(texture, GetBottomRight(dims), BottomRightCornerSource, drawColor);
		
		spriteBatch.Restart(sbParams);
	}
}
