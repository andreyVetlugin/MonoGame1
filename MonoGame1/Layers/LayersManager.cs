using Microsoft.Xna.Framework;
using MonoGame1.InGameEnvironment;
using MonoGame1.RenderTools;

namespace MonoGame1.Layers
{
    public class LayersManager
    {
        private Game1 game;
        private GameLayer gameLayer;
        private MainMenuLayer mainMenuLayer;
        public ILayer CurrentLayer {  get; private set; }

        public LayersManager(Game1 game)
        {
            gameLayer = new GameLayer(new Map(game.PathForDownloadMap, game.LootBoxCount));
            mainMenuLayer = new MainMenuLayer(game.GraphicsData.MainMenuFont);
            mainMenuLayer.TryToSetHandleForButton("Exit", game.Exit);
            mainMenuLayer.TryToSetHandleForButton("Play", ChangeLayerToGame);
            this.game = game;
            mainMenuLayer.ExitLayer += game.Exit;
            ChangeLayerToMainMenu();
        }

        public void ChangeLayer(ILayer nextLayer)
        {
            CurrentLayer = nextLayer;
        }

        public void ChangeLayerToGame()
        {
            game.IsMouseVisible = false;
            CurrentLayer = gameLayer;
        }

        public void ChangeLayerToMainMenu()
        {
            game.IsMouseVisible = true;
            CurrentLayer = mainMenuLayer;
        }

        public void UpdateLayer(GameTime time)
        {
            //if(firstLayerCall) - не будет робить, апдейт вызывается раз 20 до отрисовки каждый раз
            CurrentLayer.Update(time);
        }

        public void DrawLayer(GameTime time, GraphicsData GraphicsData)
        {
            CurrentLayer.Draw(time,GraphicsData);
        }
    }
}
