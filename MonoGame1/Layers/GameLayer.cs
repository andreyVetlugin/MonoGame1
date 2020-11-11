using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame1.InGameEnvironment;
using MonoGame1.InGameEnvironment.MapExtentions;
using MonoGame1.Logs;
using MonoGame1.RenderTools;
using System;

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

        public event Action<GameInfoLog> AcceptGameInfo;

        public GameLayer(Map map )
        {
            var mapAsPattern = map;

            worldCalculator = new WorldActivityCalculator(mapAsPattern);
            this.map = worldCalculator.currentMap;

            playerPathManager = new PathManager(mapAsPattern);
            playerPathManager.FindPathToClosestLootBox();
            AcceptGameInfo?.Invoke
                (new GameInfoLog(playerPathManager.GetCoordinatePath(),map.GetLootCoordinates(),map.PlayerPosition));
        }

        public void DrawMap(GraphicsData GraphicsData)
        {
            for (int x = 0; x < map.Size.X; x++)
            {
                for (int y = 0; y < map.Size.Y; y++)
                {
                    switch (map.Cells[x, y])
                    {
                        case MapCell.Player:
                            DrawMapObjectCell(GraphicsData, Color.Green, x, y);
                            break;
                        case MapCell.Block:
                            DrawMapObjectCell(GraphicsData, Color.Gray, x, y);
                            break;
                        case MapCell.LiveBlock:
                            DrawMapObjectCell(GraphicsData, Color.GreenYellow, x, y);
                            break;
                        case MapCell.LootBox:
                            DrawMapObjectCell(GraphicsData, Color.White, x, y);
                            break;
                    }
                }
            }
        }

        private void DrawMapObjectCell(GraphicsData GraphicsData, Color color,int x,int y)
        {
            GraphicsData.SpriteBatch.DrawPixel(new Vector2(x * cellSize.X + 3, y * cellSize.Y + 3), new Vector2(cellSize.X - 4, cellSize.Y - 4), color);
        }

        private void DrawGrid(GraphicsData GraphicsData)
        {
            var gridColor = Color.Red;
            cellSize = new Vector2(GraphicsData.Graphics.GraphicsDevice.Viewport.Width / map.Size.X, GraphicsData.Graphics.GraphicsDevice.Viewport.Height / map.Size.Y);  // потом менять только при изменении ока и при инициализации
            for (int h = 0; h <= GraphicsData.Graphics.GraphicsDevice.Viewport.Height; h += (int)cellSize.Y)
                GraphicsData.SpriteBatch.DrawPixel(new Vector2(0, h), new Vector2(GraphicsData.Graphics.GraphicsDevice.Viewport.Width, pixelWidth), gridColor);
            for (int w = 0; w <= GraphicsData.Graphics.GraphicsDevice.Viewport.Width; w += (int)cellSize.X)
                GraphicsData.SpriteBatch.DrawPixel(new Vector2(w, 0), new Vector2(pixelWidth, GraphicsData.Graphics.GraphicsDevice.Viewport.Height), gridColor);
        }

        public void Draw(GameTime gameTime, GraphicsData GraphicsData)
        {
            GraphicsData.SpriteBatch.Begin();
            DrawMap(GraphicsData);
            DrawGrid(GraphicsData);
            GraphicsData.SpriteBatch.End();
        }

        public void Update(GameTime gameTime)
        {
            elapsedTimeFromLastStep += gameTime.ElapsedGameTime;
            if (elapsedTimeFromLastStep >= stepTime)
            {
                elapsedTimeFromLastStep = TimeSpan.Zero;
                worldCalculator.Tick();
                map.TryToMovePlayer(playerPathManager.GetNextStep());
            }
        }
    }
}
