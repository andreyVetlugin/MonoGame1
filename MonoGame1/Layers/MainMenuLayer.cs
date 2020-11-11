using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame1.Gui;
using MonoGame1.RenderTools;
using System;
using System.Collections.Generic;
using System.Text;

namespace MonoGame1.Layers
{
    public class MainMenuLayer : ILayer
    {
        public event Action ExitLayer;

        private SpriteFont menuFont;
        private MenuBox menu;
        public MainMenuLayer(SpriteFont menuFont)
        {
            this.menuFont = menuFont;
            var button1 = new Button(menuFont, "Play");
            button1.BoundColor = Color.White;
            var button2 = new Button(menuFont, "Exit");
            button2.BoundColor = Color.White;
            menu = new MenuBox(new List<Button> { button1, button2 }, 10);
        }

        public bool TryToSetHandleForButton(string buttonText, Action handler)
        {
            foreach (var button in menu.Buttons)
            {
                if (button.MessageText == buttonText)
                {
                    button.ButtonRealised += handler;
                    return true;
                }
            }
            return false;
        }

        public void Draw(GameTime gameTime, GraphicsData graphicsData)
        {
            var coordinate = new Point(
                (graphicsData.Graphics.GraphicsDevice.Viewport.Width - menu.Size.X) / 2,
                (graphicsData.Graphics.GraphicsDevice.Viewport.Height - menu.Size.Y) / 2);
            graphicsData.SpriteBatch.Begin();

            menu.Draw(coordinate, graphicsData);
            graphicsData.SpriteBatch.End();
        }

        public void Update(GameTime gameTime)
        {
            var mouseState = Mouse.GetState();
            if (mouseState.LeftButton == ButtonState.Pressed)
                menu.HandleMouseClick(mouseState.Position);
        }
    }
}
