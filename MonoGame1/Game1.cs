using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame1.Layers;
using MonoGame1.Logs;
using MonoGame1.RenderTools;
using System;
using System.Collections;
using System.IO;

namespace MonoGame1
{
    public class Game1 : Game
    {
        public GraphicsData GraphicsData;
        LayersManager layersManager;

        public readonly string PathForDownloadMap;
        public readonly string OutputPath;
        public readonly int LootBoxCount;

        public Game1(string pathForDownloadMap, string outPutPath, int lootBoxCount)
        {
            PathForDownloadMap = pathForDownloadMap;
            OutputPath = outPutPath;
            LootBoxCount = lootBoxCount;

            GraphicsData = new GraphicsData { Graphics = new GraphicsDeviceManager(this) };
        }

        protected override void Initialize()
        {
            base.Initialize();
            GraphicsData.SpriteBatch = new SpriteBatchExtended(GraphicsData.Graphics.GraphicsDevice);
            layersManager = new LayersManager(this);
            layersManager.ChangeLayerToMainMenu();
        }

        protected override void LoadContent()
        {
            GraphicsData.MainMenuFont = Content.Load<SpriteFont>("Content/File");
        }

        protected override void UnloadContent()
        {
            Content.Unload();
        }

        protected override void Update(GameTime gameTime)
        {
            layersManager.UpdateLayer(gameTime);
            base.Update(gameTime);
        }

        public void SaveGameLogToFile(GameInfoLog log)
        {
            using (var writer = new StreamWriter(OutputPath))
            {
                writer.WriteLine("Start position: ");
                writer.WriteLine(log.SpawnCoordinate.ToString());
                writer.WriteLine("Lootbooxes positions: ");
                for (var i = 0; i < log.LootBoxCoordinates.Count; i++)
                    writer.WriteLine(log.LootBoxCoordinates[i].ToString());
                writer.WriteLine("Path: ");
                for (var i = 0; i < log.Path.Count; i++)
                    writer.WriteLine(log.Path[i].ToString());
            }
        }

        protected override void Draw(GameTime gameTime) // сделать здесь на делегатах и событиях логику
        {
            GraphicsData.Graphics.GraphicsDevice.Clear(Color.Black);
            layersManager.DrawLayer(gameTime, GraphicsData);
            base.Draw(gameTime);
        }
    }
}