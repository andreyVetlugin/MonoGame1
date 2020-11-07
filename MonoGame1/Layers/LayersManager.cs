using Microsoft.Xna.Framework;
using MonoGame1.RenderTools;
using System;
using System.Collections.Generic;
using System.Text;

namespace MonoGame1.Layers
{
    public class LayersManager
    {
        private GameLayer gameLayer;
        private MainMenuLayer mainMenuLayer;
        private bool firstLayerCall = true;
        public ILayer CurrentLayer {  get; private set; }

        public LayersManager()
        {
            gameLayer = new GameLayer();
            mainMenuLayer = new MainMenuLayer();
            CurrentLayer = gameLayer;
        }

        public void ChangeLayer(ILayer nextLayer)
        {
            CurrentLayer = nextLayer;
            firstLayerCall = true;
        }

        public void ChangeLayerToGame()
        {
        }

        public void ChangeLayerToMainMenu()
        {
        }

        public void UpdateLayer(GameTime time)
        {
            //if(firstLayerCall) - не будет робить, апдейт вызывается раз 20 до отрисовки каждый раз
            CurrentLayer.Update(time);
        }

        public void DrawLayer(GameTime time, GraphicsData graphicsData)
        {
            if (firstLayerCall)
                CurrentLayer.FirstDrawCall(time,graphicsData);
            CurrentLayer.Draw(time,graphicsData);
            firstLayerCall = false; // либо сделать отдельные флаги для update и draw
        }
    }
}
