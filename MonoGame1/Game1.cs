using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame1.Layers;
using MonoGame1.RenderTools;
using System.Collections;

namespace MonoGame1
{
    public class Game1 : Game
    {
        GraphicsData graphicsData;

        LayersManager layersManager;

        public Game1()
        {
            graphicsData = new GraphicsData { graphics = new GraphicsDeviceManager(this) };
        }


        protected override void Initialize()
        {
            graphicsData.spriteBatch = new SpriteBatchExtended(graphicsData.graphics.GraphicsDevice);
            layersManager = new LayersManager();
            base.Initialize();
        }
        protected override void LoadContent()
        {
        }
        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            layersManager.UpdateLayer(gameTime);
            base.Update(gameTime);
        }
        protected override void Draw(GameTime gameTime) // сделать здесь на делегатах и событиях логику
        {
            //currentLayer.Draw(gameTime);
            layersManager.DrawLayer(gameTime, graphicsData);
            base.Draw(gameTime);
        }
    }
}