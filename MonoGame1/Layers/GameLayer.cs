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
        private WorldActivityCalculator mapCalculator;
        //Draw parameters
        private TimeSpan stepTime = TimeSpan.FromSeconds(2);
        private TimeSpan elapsedTimeFromLastStep = TimeSpan.Zero;
        private Vector2 cellSize;
        private Color pixelColor = Color.Red;
        private const int pixelWidth = 2;
        private PathManager playerPathManager;        

        public GameLayer(Map map = null)
        {
            this.map = new Map();
            this.map.LoadMapFromNothing();
            playerPathManager = new PathManager(this.map);            
            playerPathManager.FindPathToClosestLootBox();
            mapCalculator = new WorldActivityCalculator(this.map);
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
                //if()
            }
                // move player and updateMap
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
