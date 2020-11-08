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
        private WorldActivityCalculator worldCalculator;
        //Draw parameters
        private TimeSpan stepTime = TimeSpan.FromSeconds(2);
        private TimeSpan elapsedTimeFromLastStep = TimeSpan.Zero;
        private Vector2 cellSize;
        private Color pixelColor = Color.Red;
        private const int pixelWidth = 2;
        private PathManager playerPathManager;        

        public GameLayer(Map map = null)
        {            
            var mapAsPattern = new Map();
            mapAsPattern.LoadMapFromNothing();

            worldCalculator = new WorldActivityCalculator(mapAsPattern);
            this.map = worldCalculator.currentMap;

            playerPathManager = new PathManager(mapAsPattern);            
            playerPathManager.FindPathToClosestLootBox();
            
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
            DrawMap();
            return;
        }

        public void Update(GameTime gameTime)
        {   
            elapsedTimeFromLastStep += gameTime.ElapsedGameTime;
            if (elapsedTimeFromLastStep >= stepTime)
            {
                elapsedTimeFromLastStep = TimeSpan.Zero;
                map.TryToMovePlayer(playerPathManager.GetNextStep());
                worldCalculator.Tick();
            }            
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
