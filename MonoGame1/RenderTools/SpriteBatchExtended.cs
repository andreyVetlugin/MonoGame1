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
    }
}
