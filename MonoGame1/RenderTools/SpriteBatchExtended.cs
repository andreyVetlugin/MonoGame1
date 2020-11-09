using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace MonoGame1.RenderTools
{
    public class SpriteBatchExtended : SpriteBatch
    {
        private readonly Texture2D pixel;

        public SpriteBatchExtended(GraphicsDevice graphicsDevice) : base(graphicsDevice)
        {
            pixel = new Texture2D(this.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            pixel.SetData(new[] { Color.White });
        }

        public void DrawPixel(Vector2 startPoint,
            Vector2 scale,
            Color color)
        {
            this.Draw(pixel,
                startPoint,
                null,
                color,
                0,
                new Vector2(0, 0),
                scale,
                SpriteEffects.None, 0);
        }

        public void DrawRectangle(Vector2 startPoint, Vector2 size, Color color)
        {
            DrawPixel(startPoint, new Vector2(size.X, 1), color);
            DrawPixel(startPoint, new Vector2(1, size.Y), color);
            DrawPixel(new Vector2(startPoint.X, startPoint.Y + size.Y), new Vector2(size.X, 1), color);
            DrawPixel(new Vector2(size.X + startPoint.X, startPoint.Y), new Vector2(1, size.Y), color);
        }
    }
}
