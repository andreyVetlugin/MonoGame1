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

        //public void DrawButtonsToCenterOfScreen(GraphicsData GraphicsData,string[] messageText)
        //{
        //    var color = Color.Red;
        //    var font = GraphicsData.MainMenuFont;
        //    var sizeOfMessage = font.MeasureString(messageText[0]);
        //    var intervalBetweenMessageStrings = 5;
        //    var centerOfScreen =
        //        new Vector2(GraphicsData.Graphics.GraphicsDevice.Viewport.Width / 2, GraphicsData.Graphics.GraphicsDevice.Viewport.Height / 2);
        //    var startPosition = new Vector2(centerOfScreen.X - sizeOfMessage.X, (float)(centerOfScreen.Y * 0.7));
        //    var currentPosition = startPosition;
        //    for (int i = 0; i < messageText.Length; i++)
        //    {
        //        sizeOfMessage = font.MeasureString(messageText[i]);
        //        currentPosition.X = centerOfScreen.X - sizeOfMessage.X / 2;
        //        GraphicsData.SpriteBatch.DrawString(font, messageText[i], currentPosition, color);
        //        currentPosition.Y = currentPosition.Y + sizeOfMessage.Y + intervalBetweenMessageStrings;
        //    }
        //}

        public void Draw(GameTime gameTime, GraphicsData graphicsData)
        {
            var coordinate = new Point(
                (graphicsData.Graphics.GraphicsDevice.Viewport.Width - menu.Size.X) / 2,
                (graphicsData.Graphics.GraphicsDevice.Viewport.Height - menu.Size.Y) / 2);
            graphicsData.SpriteBatch.Begin();
            //graphicsData.SpriteBatch.DrawPixel(coordinate.ToVector2(), new Vector2(10, 10), Color.Red);

            menu.Draw(coordinate, graphicsData);
            graphicsData.SpriteBatch.End();
        }

        public void Update(GameTime gameTime)
        {
            var mouseState = Mouse.GetState();
            if (mouseState.LeftButton == ButtonState.Pressed)
                menu.HandleMouseClick(mouseState.Position);
            //menu.HandleMouseClick();
        }
    }
}
