using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame1.Layers;
using MonoGame1.RenderTools;
using System;
using System.Collections;

namespace MonoGame1
{
    public class Game1 : Game
    {
        GraphicsData graphicsData;
        LayersManager layersManager;

        public Game1()
        {
            graphicsData = new GraphicsData { Graphics = new GraphicsDeviceManager(this) };

        }

        protected override void Initialize()
        {
            graphicsData.SpriteBatch = new SpriteBatchExtended(graphicsData.Graphics.GraphicsDevice);
            layersManager = new LayersManager(this);
            layersManager.ChangeLayerToMainMenu();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            graphicsData.MainMenuFont = Content.Load<SpriteFont>("Content/File");
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
        protected override void Draw(GameTime gameTime) // сделать здесь на делегатах и событиях логику
        {
            graphicsData.Graphics.GraphicsDevice.Clear(Color.Black);
            layersManager.DrawLayer(gameTime, graphicsData);
            base.Draw(gameTime);
        }
    }
}