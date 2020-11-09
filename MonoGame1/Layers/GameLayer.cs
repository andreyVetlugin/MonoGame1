using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
        private TimeSpan stepTime = TimeSpan.FromSeconds(1);
        private TimeSpan elapsedTimeFromLastStep = TimeSpan.Zero;
        private Vector2 cellSize;
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

        public void DrawMap(GraphicsData graphicsData)
        {
            for (int x = 0; x < map.Size.X; x++)
            {
                for (int y = 0; y < map.Size.Y; y++)
                {
                    switch (map.Cells[x, y])
                    {
                        case MapCell.Player:
                            DrawMapObjectCell(graphicsData, Color.Green, x, y);
                            break;
                        case MapCell.Block:
                            DrawMapObjectCell(graphicsData, Color.Gray, x, y);
                            break;
                        case MapCell.LiveBlock:
                            DrawMapObjectCell(graphicsData, Color.GreenYellow, x, y);
                            break;
                        case MapCell.LootBox:
                            DrawMapObjectCell(graphicsData, Color.White, x, y);
                            break;
                            
                    }
                }
            }

        }

        private void DrawMapObjectCell(GraphicsData graphicsData, Color color,int x,int y)
        {
            graphicsData.SpriteBatch.DrawPixel(new Vector2(x * cellSize.X + 3, y * cellSize.Y + 3), new Vector2(cellSize.X - 4, cellSize.Y - 4), color);
        }
        private void DrawGrid(GraphicsData graphicsData)
        {
            var gridColor = Color.Red;
            cellSize = new Vector2(graphicsData.Graphics.GraphicsDevice.Viewport.Width / map.Size.X, graphicsData.Graphics.GraphicsDevice.Viewport.Height / map.Size.Y);  // потом менять только при изменении ока и при инициализации
            for (int h = 0; h <= graphicsData.Graphics.GraphicsDevice.Viewport.Height; h += (int)cellSize.Y)
                graphicsData.SpriteBatch.DrawPixel(new Vector2(0, h), new Vector2(graphicsData.Graphics.GraphicsDevice.Viewport.Width, pixelWidth), gridColor);
            for (int w = 0; w <= graphicsData.Graphics.GraphicsDevice.Viewport.Width; w += (int)cellSize.X)
                graphicsData.SpriteBatch.DrawPixel(new Vector2(w, 0), new Vector2(pixelWidth, graphicsData.Graphics.GraphicsDevice.Viewport.Height), gridColor);
        }

        public void Draw(GameTime gameTime, GraphicsData graphicsData)
        {
            graphicsData.SpriteBatch.Begin();
            DrawMap(graphicsData);
            DrawGrid(graphicsData);
            graphicsData.SpriteBatch.End();
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

        //public void FirstDrawCall(GameTime gameTime, GraphicsData graphicsData)
        //{
        //    DrawGrid(graphicsData);
        //}
     
    }
}
