using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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

        public MainMenuLayer()
        {
        }

        public void DrawButtonsToCenterOfScreen(GraphicsData graphicsData,string[] messageText)
        {
            var color = Color.Red;
            var font = graphicsData.MainMenuFont;
            var sizeOfMessage = font.MeasureString(messageText[0]);
            var intervalBetweenMessageStrings = 5;
            var centerOfScreen =
                new Vector2(graphicsData.Graphics.GraphicsDevice.Viewport.Width / 2, graphicsData.Graphics.GraphicsDevice.Viewport.Height / 2);
            var startPosition = new Vector2(centerOfScreen.X - sizeOfMessage.X, (float)(centerOfScreen.Y * 0.7));
            var currentPosition = startPosition;
            for (int i = 0; i < messageText.Length; i++)
            {
                sizeOfMessage = font.MeasureString(messageText[i]);
                currentPosition.X = centerOfScreen.X - sizeOfMessage.X / 2;
                graphicsData.SpriteBatch.DrawString(font, messageText[i], currentPosition, color);
                currentPosition.Y = currentPosition.Y + sizeOfMessage.Y + intervalBetweenMessageStrings;
            }
        }

        public void Draw(GameTime gameTime, GraphicsData graphicsData)
        {
            var button = new Button(graphicsData.MainMenuFont,"Play");
            graphicsData.SpriteBatch.Begin();
            button.BoundColor = Color.White;
            button.Draw(new Point(20, 20),graphicsData);
            //DrawTextToCenterOfScreen(graphicsData, new string[] { "Play", "Exit","MotherFucker sdklfjkasdlflkj asd; fkljsdklfj"});
            
            graphicsData.SpriteBatch.End();
        }

        public void Update(GameTime gameTime)
        {
            if (1 == 2) // если выбрано "выйти"
                ExitLayer?.Invoke();
        }
    }
}
