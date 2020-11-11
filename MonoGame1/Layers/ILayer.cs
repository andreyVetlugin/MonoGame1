﻿using Microsoft.Xna.Framework;
using MonoGame1.RenderTools;
using System;
using System.Collections.Generic;
using System.Text;

namespace MonoGame1.Layers
{
    public interface ILayer
    {
        public void Update(GameTime gameTime);

        public void Draw(GameTime gameTime, GraphicsData GraphicsData);

        //public void FirstDrawCall(GameTime gameTime, GraphicsData GraphicsData);

        //public void FirstUpdateCAll(GameTime gameTime);
    }
}
