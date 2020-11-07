using Microsoft.Xna.Framework;
using MonoGame1.InGameEnvironment;
using MonoGame1.InGameEnvironment.MapExtentions;
using MonoGame1.RenderTools;
using System;
using System.Collections.Generic;
using System.Text;

namespace MonoGame1.Layers
{
    public class GameLayer : ILayer
    {
        private Map map;

        //Draw parameters
        private Vector2 cellSize;
        private Color pixelColor = Color.Red;
        private const int pixelWidth = 2;

        public GameLayer()
        {
            map = new Map();
            map.LoadMapFromNothing();
        }

        public void DrawMap()
        {

        }

        private void DrawGrid(GraphicsData graphicsData)
        {
            cellSize = new Vector2(graphicsData.graphics.GraphicsDevice.Viewport.Width / map.Size.X, graphicsData.graphics.GraphicsDevice.Viewport.Height / map.Size.Y);  // потом менять только при изменении ока и при инициализации
            graphicsData.spriteBatch.Begin();
            for (int h = (int)cellSize.Y; h <= graphicsData.graphics.GraphicsDevice.Viewport.Height; h += (int)cellSize.Y)
                graphicsData.spriteBatch.DrawPixel(new Vector2(0, h), new Vector2(graphicsData.graphics.GraphicsDevice.Viewport.Width, pixelWidth), pixelColor);
            graphicsData.spriteBatch.End();
        }

        public void Draw(GameTime gameTime, GraphicsData graphicsData)
        {
            return;
        }

        public void Update(GameTime gameTime)
        {
            var activityActions = new WorldActivityCalculator(map);
            activityActions.Tick();
            activityActions.Tick();
            activityActions.Tick();


            var pathFinder = new PathFinder(map);
            var test = pathFinder.FindPathToClosestLootBox();
            return;
        }

        public void FirstDrawCall(GameTime gameTime, GraphicsData graphicsData)
        {
            DrawGrid(graphicsData);
        }

        public void FirstUpdateCAll(GameTime gameTime)
        {
            return;
        }
    }
}
